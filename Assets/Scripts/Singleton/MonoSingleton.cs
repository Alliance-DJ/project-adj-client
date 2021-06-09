using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> instance = new Lazy<T>(CreateInstance);

    public static T Instance => instance.Value;

    private static T CreateInstance()
    {
        return FindObjectOfType<T>();
    }
}

public class DontDestroyMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object lockObj = new object();

    public static T Instance
    {
        get
        {
            if (!instance.IsValid())
            {
                lock (lockObj)
                {
                    if (!instance.IsValid())
                    {
                        instance = FindObjectOfType<T>();
                        if (!instance.IsValid())
                        {
                            var obj = new GameObject($"{typeof(T).Name}(Singleton)");
                            instance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj);

                            Debug.LogWarning($"Not Exist Singleton, Create Dont Destroy Singleton : {typeof(T)}", obj);
                        }
                    }
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance.IsValid() && instance != this)
        {
            Debug.LogWarning($"Duplicate Dont Destroy Singleton : {typeof(T)}");

            var components = gameObject.GetComponents<Component>();
            if (components.Length <= 2)
                Destroy(gameObject);
            else
                Destroy(this);
        }
        else
        {
            instance = CreateInstance();

            if (instance.IsValid())
                DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private T CreateInstance()
    {
        return gameObject.GetComponent<T>();
    }
}
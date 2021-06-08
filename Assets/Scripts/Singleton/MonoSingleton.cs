using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static readonly Lazy<T> instance = new Lazy<T>(CreateInstance);

    public static T Instance => instance.Value;

    private static T CreateInstance()
    {
        return FindObjectOfType<T>();
    }
}

public class DontDestroyMonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    private static readonly object lockObj = new object();

    public static T Instance
    {
        get
        {
            if (!instance)
            {
                lock (lockObj)
                {
                    if (!instance)
                    {
                        instance = FindObjectOfType<T>();
                        if (!instance)
                        {
                            var obj = new GameObject($"{typeof(T).Name}(Singleton)");
                            instance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }
            }

            return instance;
        }
    }

    private bool duplicateObj = false;
    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = CreateInstance();

            if (instance != null)
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            duplicateObj = true;
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (!duplicateObj)
            instance = null;
    }

    private T CreateInstance()
    {
        return gameObject.GetComponent<T>();
    }
}
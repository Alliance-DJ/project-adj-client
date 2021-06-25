using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static Lazy<T> instance = new Lazy<T>(FindInstance);

    public static T Instance => instance.Value;

    protected void OnDestroy()
    {
        if (!instance.IsValueCreated) return;

        instance = new Lazy<T>(FindInstance);
    }

    private static T FindInstance()
    {
        var instance = FindObjectOfType<T>();
        if (!instance.IsValid())
        {
            Debug.LogWarning($"<color=orange>Not Found MonoSingleton : <b>{typeof(T)}</b></color>");
        }
        return instance;
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

                            Debug.LogWarning($"<color=orange>Not Found Singleton, Create Dont Destroy Singleton : <b>{typeof(T)}</b></color>", obj);
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
            Debug.LogWarning($"<color=orange>Duplicate Dont Destroy Singleton : <b>{typeof(T)}</b></color>");

            var components = gameObject.GetComponents<Component>();
            if (components.Length <= 2)
                Destroy(gameObject);
            else
                Destroy(this);
        }
        else
        {
            instance = FindInstance();

            if (instance.IsValid())
                DontDestroyOnLoad(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            Debug.LogWarning($"<color=red>Destroy Dont Destroy Singleton : <b>{typeof(T)}</b></color>");
            instance = null;
        }
    }

    private T FindInstance()
    {
        return this as T;
    }
}
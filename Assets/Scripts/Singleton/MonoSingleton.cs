using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
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
                    }
                }
            }

            if (!instance.IsValid())
            {
                Debug.LogWarning($"<color=red>Not Found MonoSingleton : <b>{typeof(T)}</b></color>");
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance.IsValid())
        {
            instance = FindInstance();

            if (!instance.IsValid())
                Debug.LogWarning($"<color=red>Not Found Dont Destroy MonoSingleton : <b>{typeof(T)}</b></color>");
        }
        else if (instance != this)
        {
            Debug.LogWarning($"<color=orange>Duplicate MonoSingleton : <b>{typeof(T)}</b></color>");

            var components = gameObject.GetComponents<Component>();
            if (components.Length <= 2)
                Destroy(gameObject);
            else
                Destroy(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance != this) return;

        instance = null;
    }

    private T FindInstance()
    {
        return this as T;
    }
}

public class DontDestroyMonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    private static readonly object lockObj = new object();

    private bool shuttingDown;

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
                    }
                }
            }

            if (!instance.IsValid())
            {
                Debug.LogWarning($"<color=red>Not Found Dont Destroy MonoSingleton : <b>{typeof(T)}</b></color>");
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance.IsValid())
        {
            instance = FindInstance();

            if (instance.IsValid())
                DontDestroyOnLoad(gameObject);
            else
                Debug.LogWarning($"<color=red>Not Found Dont Destroy MonoSingleton : <b>{typeof(T)}</b></color>");
        }
        else if (instance != this)
        {
            Debug.LogWarning($"<color=orange>Duplicate Dont Destroy MonoSingleton : <b>{typeof(T)}</b></color>");

            var components = gameObject.GetComponents<Component>();
            if (components.Length <= 2)
                Destroy(gameObject);
            else
                Destroy(this);
        }

    }

    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        if (instance != this) return;

        if (!shuttingDown)
        {
            Debug.LogWarning($"<color=red>Destroy Dont Destroy MonoSingleton : <b>{typeof(T)}</b></color>");
        }

        instance = null;
    }

    private T FindInstance()
    {
        return this as T;
    }
}
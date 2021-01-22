using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static Lazy<T> _instance;

    public static bool IsSingletonCreated => _instance != null && _instance.IsValueCreated;

    public static T Instance
    {
        get
        {
            _instance ??= new Lazy<T>(() =>
            {
                T instance = null;
                var types = FindObjectsOfType<T>();
                if (types.Length > 0)
                {
                    var type = types[0];
                    instance = type;

                    if (types.Length > 1)
                        Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
                }

                if (instance != null) return instance;

                var obj = new GameObject($"{typeof(T).Name}(Singleton)");
                instance = obj.AddComponent<T>();

                return instance;
            });

            return _instance.Value;
        }
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }
}

public class DontDestroyMonoSingleton<T> : MonoBehaviour where T : Component
{
    private static Lazy<T> _instance;

    public static bool IsSingletonCreated => _instance != null && _instance.IsValueCreated;

    public static T Instance
    {
        get
        {
            _instance ??= new Lazy<T>(() =>
            {
                T instance = null;
                var types = FindObjectsOfType<T>();
                if (types.Length > 0)
                {
                    var type = types[0];
                    instance = type;

                    if (types.Length > 1)
                        Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
                }

                if (instance != null) return instance;

                var obj = new GameObject($"{typeof(T).Name}(Singleton)");
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);

                return instance;
            });

            return _instance.Value;
        }
    }

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }
}
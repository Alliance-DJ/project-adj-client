using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static readonly Lazy<T> _instance =
        new Lazy<T>(() =>
        {
            T instance = null;
            var types = FindObjectsOfType<T>();
            if (types.Length > 0)
            {
                var type = types[0];
                instance = type;

                var obj = type.gameObject;
                obj.name = $"{typeof(T).Name}(Singleton)";
                DontDestroyOnLoad(obj);

                if (types.Length > 1)
                    Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
            }

            if (instance == null)
            {
                GameObject obj = new GameObject($"{typeof(T).Name}(Singleton)");
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        });

    public static bool IsSingletonCreated => _instance.IsValueCreated;
    public static T Instance => _instance.Value;
}
using System;

public class Singleton<T> where T : new()
{
    private static Lazy<T> _instance;

    public static bool IsSingletonCreated => _instance != null && _instance.IsValueCreated;

    public static T Instance
    {
        get
        {
            _instance ??= new Lazy<T>(() => new T());
            return _instance.Value;
        }
    }
}
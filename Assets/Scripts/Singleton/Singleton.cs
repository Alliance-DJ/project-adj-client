using System;

public class Singleton<T> : IDisposable where T : new()
{
    private static Lazy<T> instance = new Lazy<T>(() => new T());

    public static T Instance => instance.Value;

    public void Dispose()
    {
        if (!instance.IsValueCreated) return;

        instance = new Lazy<T>(() => new T());
    }
}
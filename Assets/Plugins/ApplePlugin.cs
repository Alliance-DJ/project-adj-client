#if UNITY_IOS
using System.Runtime.InteropServices;

public static class ApplePlugin
{
    [DllImport("__Internal")]
    private static extern float _GetNativeScaleFactor();

    public static float GetNativeScaleFactor() => _GetNativeScaleFactor();
}
#endif

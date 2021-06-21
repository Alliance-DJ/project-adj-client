using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasPixelToUIKitSize : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        Resize();
    }

    private void Resize()
    {
        if (Application.isEditor || !canvasScaler.IsValid()) return;

        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
#if UNITY_ANDROID
        canvasScaler.scaleFactor = Screen.dpi * 0.00625f; // Screen.dpi / 160f;
#elif UNITY_IOS
        canvasScaler.scaleFactor = ApplePlugin.GetNativeScaleFactor();
#endif
    }
}

using UnityEngine;

public class ColorSetter : PropertyMapper
{
    public Color Default = Color.white;

    protected override void OnValueChanged()
    {
        if (!Value.IsValid())
        {
            Value = Default;
        }

        if (ReflectionExtensions.TryConvertValue<Color>(Value, out var color))
        {
            if (TryGetComponent<UnityEngine.UI.Text>(out var uguiText))
            {
                uguiText.color = color;
                return;
            }

            if (TryGetComponent<TMPro.TextMeshProUGUI>(out var tmpUguiText))
            {
                tmpUguiText.color = color;
                return;
            }

            if (TryGetComponent<TMPro.TextMeshPro>(out var tmpText))
            {
                tmpText.color = color;
                return;
            }
        }
    }
}

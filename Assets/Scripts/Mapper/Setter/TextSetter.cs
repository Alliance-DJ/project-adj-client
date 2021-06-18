using UnityEngine;

public class TextSetter : PropertyMapper
{
    public string Default;
    [TextArea]
    public string format;

    public string Format(object o)
    {
        if (string.IsNullOrEmpty(format))
        {
            if (o.IsValid())
                return o.ToString();
            else
                return string.Empty;
        }

        return string.Format(format, o);
    }

    protected override void OnValueChanged()
    {
        if (!Value.IsValid())
        {
            Value = Default;
        }

        var formatValue = Format(Value);
        if (TryGetComponent<UnityEngine.UI.Text>(out var uguiText))
        {
            uguiText.text = formatValue;
            return;
        }

        if (TryGetComponent<TMPro.TextMeshProUGUI>(out var tmpUguiText))
        {
            tmpUguiText.SetText(formatValue);
            return;
        }

        if (TryGetComponent<TMPro.TextMeshPro>(out var tmpText))
        {
            tmpText.SetText(formatValue);
            return;
        }
    }
}

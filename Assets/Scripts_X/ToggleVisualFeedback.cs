using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToggleVisualGroupHandler : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text label;
    public Toggle toggle;
    public GameObject associatedUIObject;

    [Header("Font Styles")]
    public FontStyles normalStyle = FontStyles.Normal;
    public FontStyles selectedStyle = FontStyles.Bold | FontStyles.Underline;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(1f, 0.55f, 0f); // Default: orange

    [Header("Toggle Settings")]
    public string modeName = "Controller"; // e.g., "Controller", "HandGesture", "Multimodal"

    // Static property to access selected mode globally
    public static string SelectedMode { get; private set; }

    void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleChanged);
        UpdateVisual(toggle.isOn); // Initialize state

        if (toggle.isOn)
        {
            SelectedMode = modeName;
        }
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            SelectedMode = modeName;
        }

        UpdateVisual(isOn);
    }

    void UpdateVisual(bool isOn)
    {
        if (label != null)
        {
            label.fontStyle = isOn ? selectedStyle : normalStyle;
            label.color = isOn ? selectedColor : normalColor;
        }

        if (associatedUIObject != null)
        {
            associatedUIObject.SetActive(isOn);
        }
    }
}

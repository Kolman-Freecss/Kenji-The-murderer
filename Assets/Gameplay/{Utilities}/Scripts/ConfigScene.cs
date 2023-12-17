#region

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#endregion

/// <summary>
/// Utility class to change the font of all TextMeshProUGUI objects in the scene in Edit Mode.
/// </summary>
[ExecuteInEditMode]
public class ConfigScene : MonoBehaviour
{
    public TMP_FontAsset newFont;
    public Sprite backgroundButton;

    public GameObject[] objectsToChange;

    [Header("Sliders")] [SerializeField] float sliderWidthSize = 500f;

    [SerializeField] float sliderHeightSize = 30f;

    [Header("Buttons")] [Tooltip("Font size for buttons")] [SerializeField]
    float buttonFontSize = 24f;

    [SerializeField] float buttonWidthSize = 200f;

    [SerializeField] float buttonHeightSize = 50f;

    [SerializeField] Color buttonHighlightColor = Color.red;

    [Header("Texts")] [Tooltip("Font size for texts")] [SerializeField]
    float textFontSize = 20f;

    [SerializeField] bool changeTextFormat = false;

    [SerializeField] bool changeTextBoxSize = false;

    [SerializeField] float textWidthSize = 200f;

    [SerializeField] float textHeightSize = 50f;

    [SerializeField] Color textColor = Color.black;

    [ContextMenu("Change Fonts")]
    void ChangeFonts()
    {
        List<TextMeshProUGUI> components = new();
        if (objectsToChange == null || objectsToChange.Length == 0)
        {
            components = new List<TextMeshProUGUI>(FindObjectsOfType<TextMeshProUGUI>());
        }
        else
        {
            for (int i = 0; i < objectsToChange.Length; i++)
            {
                components.AddRange(objectsToChange[i].GetComponentsInChildren<TextMeshProUGUI>());
            }
        }

        foreach (TextMeshProUGUI textObject in components)
        {
            textObject.font = newFont;
            textObject.color = textColor;
            if (textObject.GetComponentInParent<Button>())
            {
                continue;
            }

            Undo.RecordObject(textObject, "Changed Font");
            if (changeTextFormat)
            {
                textObject.horizontalAlignment = HorizontalAlignmentOptions.Center;
                textObject.verticalAlignment = VerticalAlignmentOptions.Middle;
                textObject.fontSize = textFontSize;
                textObject.enableAutoSizing = false;
            }

            if (changeTextBoxSize)
                textObject.GetComponent<RectTransform>().sizeDelta = new Vector2(textWidthSize, textHeightSize);
        }
    }

    [ContextMenu("Change Buttons")]
    void ChangeButtons()
    {
        List<Button> components = new();
        if (objectsToChange == null || objectsToChange.Length == 0)
        {
            components = new List<Button>(FindObjectsOfType<Button>());
        }
        else
        {
            for (int i = 0; i < objectsToChange.Length; i++)
            {
                components.AddRange(objectsToChange[i].GetComponentsInChildren<Button>());
            }
        }

        foreach (Button button in components)
        {
            Undo.RecordObject(button, "Changed Button");
            Image image = button.GetComponent<Image>();
            if (backgroundButton)
            {
                image.sprite = backgroundButton;
            }

            button.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonWidthSize, buttonHeightSize);
            image.color = Color.white; // #4B4B4B en formato RGB
            button.transition = Selectable.Transition.ColorTint;
            ColorBlock colors = button.colors;
            colors.highlightedColor = buttonHighlightColor;
            button.colors = colors;
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            button.GetComponentInChildren<TextMeshProUGUI>().fontSize = buttonFontSize;
        }
    }

    [ContextMenu("Change Toogles")]
    void ChangeToogles()
    {
        List<Toggle> components = new();
        if (objectsToChange == null || objectsToChange.Length == 0)
        {
            components = new List<Toggle>(FindObjectsOfType<Toggle>());
        }
        else
        {
            for (int i = 0; i < objectsToChange.Length; i++)
            {
                components.AddRange(objectsToChange[i].GetComponentsInChildren<Toggle>());
            }
        }

        foreach (Toggle toggle in components)
        {
            Undo.RecordObject(toggle, "Changed Toggle");
            ColorBlock colors = toggle.colors;
            colors.normalColor = new Color32(236, 183, 183, 255);
            colors.highlightedColor = new Color32(255, 0, 0, 255);
            toggle.colors = colors;
        }
    }

    [ContextMenu("Change Sliders")]
    void ChangeSliders()
    {
        List<Slider> components = new();
        if (objectsToChange == null || objectsToChange.Length == 0)
        {
            components = new List<Slider>(FindObjectsOfType<Slider>());
        }
        else
        {
            for (int i = 0; i < objectsToChange.Length; i++)
            {
                components.AddRange(objectsToChange[i].GetComponentsInChildren<Slider>());
            }
        }

        foreach (Slider slider in components)
        {
            Undo.RecordObject(slider, "Changed Slider");
            RectTransform rectTransform = slider.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(sliderWidthSize, sliderHeightSize);

            slider.minValue = 0;
            slider.maxValue = 100;

            // Get Fill Area image
            Image fillArea = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            fillArea.color = new Color32(195, 99, 99, 255);
        }
    }
}
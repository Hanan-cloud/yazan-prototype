using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using AHAKuo.Signalia.LocalizationStandalone.Framework;
public class ReverseUILayout : MonoBehaviour
{
    [Header("Horizontal Layout Groups")]
    [SerializeField] private List<HorizontalLayoutGroup> horizontalGroups = new List<HorizontalLayoutGroup>();

    [Header("TMP Texts (Alignment)")]
    [SerializeField] private List<TMP_Text> texts = new List<TMP_Text>();

    [Header("Sliders (Direction)")]
    [SerializeField] private List<Slider> sliders = new List<Slider>();


    private void Start()
    {
        OnLanguageChanged();

        LocalizationEvents.Subscribe(OnLanguageChanged, gameObject);

    }


    void OnLanguageChanged()
    {
        if (LocalizationRuntime.CurrentLanguageCode == "ar")
        {
            ApplyRTL();
        }
        else
        {
            ApplyLTR();
        }

    }

    public void ApplyRTL()
    {
        foreach (var group in horizontalGroups)
            if (group != null) group.reverseArrangement = true;

        foreach (var text in texts)
            if (text != null) text.alignment = TextAlignmentOptions.Right;

        foreach (var slider in sliders)
            if (slider != null) slider.direction = Slider.Direction.RightToLeft;
    }

    public void ApplyLTR()
    {
        foreach (var group in horizontalGroups)
            if (group != null) group.reverseArrangement = false;

        foreach (var text in texts)
            if (text != null) text.alignment = TextAlignmentOptions.Left;

        foreach (var slider in sliders)
            if (slider != null) slider.direction = Slider.Direction.LeftToRight;
    }
}

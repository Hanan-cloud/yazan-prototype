using System;
using UnityEngine;
using AHAKuo.Signalia.LocalizationStandalone.Internal;

public class InGameStoryPanel : MonoBehaviour
{
    private const string TextKey = "InGameStory_";

    private int index = 1;
    private int displayedIndex;

    [SerializeField] private GameObject text;

    private SimpleLocalizedText simpleText;

    public int Index => displayedIndex;

    public event Action<int> OnStoryIndexChanged;

    private void Start()
    {
        simpleText = text.GetComponent<SimpleLocalizedText>();

        ResetPoint.OnNailsFalls += SetInGameStory;
        ResetPoint.OnNailsReset += ResetIndex;

        // Start with no available lore.
        index = 1;
        displayedIndex = 0;

        OnStoryIndexChanged?.Invoke(displayedIndex);
    }

    private void SetInGameStory()
    {
        if (index > 9)
            return;

        displayedIndex = index;

        simpleText.SetKey(TextKey + displayedIndex);

        OnStoryIndexChanged?.Invoke(displayedIndex);

        index++;
    }

    private void ResetIndex()
    {
        // Reset to Run 0: no active lore index.
        index = 1;
        displayedIndex = 0;

        OnStoryIndexChanged?.Invoke(displayedIndex);
    }

    private void OnDestroy()
    {
        ResetPoint.OnNailsFalls -= SetInGameStory;
        ResetPoint.OnNailsReset -= ResetIndex;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    [Header("Lore Objects")]
    [SerializeField] private List<GlowController> Lore1 = new();
    [SerializeField] private List<GlowController> Lore2 = new();
    [SerializeField] private List<GlowController> Lore3 = new();
    [SerializeField] private List<GlowController> Lore4 = new();
    [SerializeField] private List<GlowController> Lore5 = new();
    [SerializeField] private List<GlowController> Lore6 = new();
    [SerializeField] private List<GlowController> Lore7 = new();
    [SerializeField] private List<GlowController> Lore8 = new();
    [SerializeField] private List<GlowController> Lore9 = new();

    [Header("References")]
    [SerializeField] private InGameStoryPanel storyPanel;

    private const string SaveKeyPrefix = "LoreRead_";
    private const string LoreUnlockedKey = "LoreUnlockedAfterFirstRun";
    private const int LoreCount = 9;

    private int currentLoreIndex;
    private bool isLoreUnlocked;

    // Lore read during the current progress sequence.
    private readonly HashSet<int> readThisSession = new();

    private void OnEnable()
    {
        if (storyPanel != null)
            storyPanel.OnStoryIndexChanged += HandleStoryIndexChanged;

        ResetPoint.OnNailsFalls += UnlockLore;
    }

    private void Start()
    {
        if (storyPanel == null)
        {
            Debug.LogError(
                "LoreManager: Assign InGameStoryPanel in the Inspector.",
                this
            );

            return;
        }

        isLoreUnlocked = ES3.Load<bool>(LoreUnlockedKey, false);
        currentLoreIndex = storyPanel.Index;

        RefreshLoreStates();
    }

    private void OnDisable()
    {
        if (storyPanel != null)
            storyPanel.OnStoryIndexChanged -= HandleStoryIndexChanged;

        ResetPoint.OnNailsFalls -= UnlockLore;
    }

    private void UnlockLore()
    {
        if (!isLoreUnlocked)
        {
            isLoreUnlocked = true;
            ES3.Save(LoreUnlockedKey, true);
        }

        RefreshLoreStates();
    }

    private void HandleStoryIndexChanged(int newIndex)
    {
        // If progress goes backwards, allow those lores
        // to glow again when they become current.
        if (newIndex < currentLoreIndex)
        {
            int from = Mathf.Max(1, newIndex);
            readThisSession.RemoveWhere(lore => lore >= from);
        }

        currentLoreIndex = newIndex;

        RefreshLoreStates();
    }

    public bool CanInteractWithLore(int loreIndex)
    {
        if (!isLoreUnlocked)
            return false;

        if (loreIndex < 1 || loreIndex > LoreCount)
            return false;

        return loreIndex == currentLoreIndex;
    }

    public void RefreshLoreStates()
    {
        for (int i = 1; i <= LoreCount; i++)
        {
            List<GlowController> loreObjects = GetLoreList(i);

            if (loreObjects == null)
                continue;

            if (!isLoreUnlocked)
            {
                SetLoreOff(loreObjects);
                continue;
            }

            bool isCurrent = i == currentLoreIndex;
            bool readThisRun = readThisSession.Contains(i);
            bool savedAsRead = IsLoreRead(i);

            // The current lore can glow again after progress returns to it.
            if (isCurrent && !readThisRun)
            {
                SetLoreCanRead(loreObjects);
            }
            else if (savedAsRead || readThisRun)
            {
                SetLoreRead(loreObjects);
            }
            else
            {
                SetLoreOff(loreObjects);
            }
        }
    }

    public void MarkLoreAsRead(int loreIndex)
    {
        if (!CanInteractWithLore(loreIndex))
            return;

        if (loreIndex < 1 || loreIndex > LoreCount)
            return;

        ES3.Save(GetSaveKey(loreIndex), true);

        readThisSession.Add(loreIndex);

        RefreshLoreStates();
    }

    public bool IsLoreRead(int loreIndex)
    {
        if (loreIndex < 1 || loreIndex > LoreCount)
            return false;

        return ES3.Load<bool>(GetSaveKey(loreIndex), false);
    }

    private void SetLoreCanRead(List<GlowController> loreObjects)
    {
        foreach (GlowController glow in loreObjects)
        {
            if (glow != null)
                glow.CallCanRead();
        }
    }

    private void SetLoreRead(List<GlowController> loreObjects)
    {
        foreach (GlowController glow in loreObjects)
        {
            if (glow != null)
                glow.Callread();
        }
    }

    private void SetLoreOff(List<GlowController> loreObjects)
    {
        foreach (GlowController glow in loreObjects)
        {
            if (glow != null)
                glow.CallOff();
        }
    }

    private string GetSaveKey(int loreIndex)
    {
        return SaveKeyPrefix + loreIndex;
    }

    private List<GlowController> GetLoreList(int index)
    {
        switch (index)
        {
            case 1: return Lore1;
            case 2: return Lore2;
            case 3: return Lore3;
            case 4: return Lore4;
            case 5: return Lore5;
            case 6: return Lore6;
            case 7: return Lore7;
            case 8: return Lore8;
            case 9: return Lore9;

            default: return null;
        }
    }
}

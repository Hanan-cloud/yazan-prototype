using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    [Header("Lore Objects")]
    [SerializeField] private List<GlowController> Lore1 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore2 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore3 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore4 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore5 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore6 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore7 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore8 = new List<GlowController>();
    [SerializeField] private List<GlowController> Lore9 = new List<GlowController>();

    [Header("References")]
    [SerializeField] private InGameStoryPanel storyPanel;

    private const string SaveKeyPrefix = "LoreRead_";
    private const int LoreCount = 9;

    private int currentLoreIndex;

    // Lore the player has read since the story last reached them (current progress only).
    // Never saved. Cleared for a lore whenever progress goes back past it
    // (see HandleStoryIndexChanged), so an already-saved lore can glow again.
    private readonly HashSet<int> readThisSession = new HashSet<int>();

    // Index 0 is the starting state: lore 1 is the active lore there too.
    private int ActiveLore => Mathf.Max(1, currentLoreIndex);

    private void OnEnable()
    {
        if (storyPanel != null)
            storyPanel.OnStoryIndexChanged += HandleStoryIndexChanged;
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

        currentLoreIndex = storyPanel.Index;
        RefreshLoreStates();
    }

    private void OnDisable()
    {
        if (storyPanel != null)
            storyPanel.OnStoryIndexChanged -= HandleStoryIndexChanged;
    }

    private void HandleStoryIndexChanged(int newIndex)
    {
        // Progress went backwards (player lost progress / index reset):
        // forget "read this run" for every lore from the new position onward,
        // so they glow again when reached. Saved data is untouched.
        if (newIndex < currentLoreIndex)
        {
            int from = Mathf.Max(1, newIndex);
            readThisSession.RemoveWhere(lore => lore >= from);
        }

        currentLoreIndex = newIndex;
        RefreshLoreStates();
    }

    public void RefreshLoreStates()
    {
        for (int i = 1; i <= LoreCount; i++)
        {
            List<GlowController> loreObjects = GetLoreList(i);

            if (loreObjects == null)
                continue;

            bool isCurrent = i == ActiveLore;
            bool readThisRun = readThisSession.Contains(i);
            bool savedAsRead = IsLoreRead(i);

            foreach (GlowController glow in loreObjects)
            {
                if (glow == null)
                    continue;

                if (isCurrent && !readThisRun)
                {
                    // Current lore not yet read in this run -> glow,
                    // even if it was saved as read from a previous run.
                    glow.CallCanRead();
                }
                else if (savedAsRead || readThisRun)
                {
                    glow.Callread();
                }
                else
                {
                    glow.CallOff();
                }
            }
        }
    }

    public void MarkLoreAsRead(int loreIndex)
    {
        if (loreIndex < 1 || loreIndex > LoreCount)
        {
            Debug.LogWarning(
                $"LoreManager: Invalid lore index: {loreIndex}",
                this
            );

            return;
        }

        // Save is unchanged: still written to disk exactly as before.
        ES3.Save(GetSaveKey(loreIndex), true);

        // Session flag: switches the glow from "can read" to "read" for this run.
        readThisSession.Add(loreIndex);

        RefreshLoreStates();
    }

    public bool IsLoreRead(int loreIndex)
    {
        return ES3.Load<bool>(GetSaveKey(loreIndex), false);
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
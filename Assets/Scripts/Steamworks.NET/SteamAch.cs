using UnityEngine;
using Steamworks;
using System.Collections.Generic;

public class SteamAch : MonoBehaviour
{

    public static SteamAch instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

    }

    public enum AchievKey
    {
        FinishGame,
        AllAnomalies,
        StoryLore,
        NoMistake,
        FastRun
    }

    
    private Dictionary<AchievKey, string> Achievements = new Dictionary<AchievKey, string>()
    {
            { AchievKey.FinishGame,   "FINISH_GAME" },
            { AchievKey.AllAnomalies, "DISCOVER_ALL_ANOMALIES" },
            { AchievKey.StoryLore,    "STORY_LORE" },
            { AchievKey.NoMistake,    "NO_MISTAKE" },
            { AchievKey.FastRun,      "FAST_RUN" },
    };
    


    void Start()
    {
        if (SteamManager.Initialized)
        {
            Debug.Log("SteamManager.Initialized");

            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
        }
    
    }


    public void SetAchievment(AchievKey key)
    {
        if (!SteamManager.Initialized) return;

            Achievements.TryGetValue(key, out string achId);
            Steamworks.SteamUserStats.GetAchievement(achId, out bool achievementCompleted);

        if (!achievementCompleted)

            SteamUserStats.SetAchievement(achId);
            SteamUserStats.StoreStats();


    }




}

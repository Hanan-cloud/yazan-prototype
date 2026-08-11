using UnityEngine;
using Steamworks;
using System.Collections.Generic;

public class SteamAch : MonoBehaviour
{

    public static SteamAch instance;

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    private void OnEnable()
    {
       
    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
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

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

            Debug.Log("SteamManager.Initialized");
            string name = SteamFriends.GetPersonaName();
            Debug.Log(name);
        }
        else
        {
            Debug.Log("SteamManager.Initialized nooooooooo");
        }

        //SteamAPI.RunCallbacks();

    }
 
    private Dictionary<AchievKey, string> Achievements = new Dictionary<AchievKey, string>()
    {
            { AchievKey.FinishGame,   "FINISH_GAME" },
            { AchievKey.AllAnomalies, "DISCOVER_ALL_ANOMALIES" },
            { AchievKey.StoryLore,    "STORY_LORE" },
            { AchievKey.NoMistake,    "NO_MISTAKE" },
            { AchievKey.FastRun,      "FAST_RUN" },
    };
    


 


    public void SetAchievment(AchievKey key)
    {
        if (!SteamManager.Initialized) return;

            Achievements.TryGetValue(key, out string achId);
            SteamUserStats.GetAchievement(achId, out bool achievementCompleted);
        Debug.Log(achId + ": " + achievementCompleted);

        if (!achievementCompleted)
        {

           bool b= SteamUserStats.SetAchievement(achId);
            Debug.Log(achId +": "+ b);

            bool a=  SteamUserStats.StoreStats();
            Debug.Log(achId + ": " + a);




        }
    }


    public void Checkkkk(AchievKey key)
    {
        Achievements.TryGetValue(key, out string achId);

        Debug.Log(achId +": " + SteamUserStats.GetAchievement(achId, out bool isUnlocked));

    }

    public void ClearAchievement(AchievKey key)
    {
        Achievements.TryGetValue(key, out string achId);
        SteamUserStats.ClearAchievement(achId);
        SteamUserStats.StoreStats();
        Debug.Log($"[Steam] Reset achievement: {achId}");
    }

}

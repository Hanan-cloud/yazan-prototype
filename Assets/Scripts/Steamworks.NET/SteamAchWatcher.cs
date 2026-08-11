using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class SteamAchWatcher : MonoBehaviour
{
    public static SteamAchWatcher instance;

    WaitForSeconds wait1s = new WaitForSeconds(1);

    bool doesPlayerMistake= false;

    int timer = 0;
    [SerializeField] int timeToComplete = 300;

    private void Awake()
    {
        if (instance == null)
        instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {

        

        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {

        yield return wait1s;
        timer++;


    }


    void CheckOnEndAch()
    {


        if (doesPlayerMistake)
        {

            SteamAch.instance.SetAchievment(SteamAch.AchievKey.NoMistake);


        }

        if (timer >= timeToComplete)
        {

            SteamAch.instance.SetAchievment(SteamAch.AchievKey.FastRun);

        }









    }


    [ContextMenu("All anomaly ach")]
    public void AllAnomaliesDiscovered()
    {

        SteamAch.instance.SetAchievment(SteamAch.AchievKey.AllAnomalies);


    }
    [ContextMenu("check")]

    public void check()
    {

        SteamAch.instance.Checkkkk(SteamAch.AchievKey.AllAnomalies);

    }

    [ContextMenu("clear All anomaly ach")]
    public void ClearAllAnomaliesDiscovered()
    {

        SteamAch.instance.ClearAchievement(SteamAch.AchievKey.AllAnomalies);


    }

    void FinishStoryPanel()
    {
        // dictuinary 
        //if(All of the story panels have been read )
        //{ 
        //}
    }


    void FinishTheGame()
    {

        SteamAch.instance.SetAchievment(SteamAch.AchievKey.FinishGame);

    }
}

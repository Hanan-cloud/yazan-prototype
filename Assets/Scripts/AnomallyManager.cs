using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class AnomallyManager : MonoBehaviour
{
    public static AnomallyManager Instance;
    List<IAnomaly> anomalies = new();

    bool isAnomalyRun;
    public bool IsAnomalyRun { get => isAnomalyRun; }
    public IAnomaly CurrentAnomaly { get => currentAnomaly; }

    private List<IAnomaly> allItems;
    private Queue<IAnomaly> recentlyUsed = new Queue<IAnomaly>();
    private const int cooldownRounds = 3;

    bool isFirstRun = true;

    bool forceAnomaly;
    bool randomAnomaly;

    IAnomaly currentAnomaly;
    int normalRunCounter = 0;
    const int normalRunMax = 2;
    int forcedAnomalyCounter;
    int forcedAnomalyCount=3;

    private Dictionary<AnomalyList, bool> foundAnomaliesDic = new Dictionary<AnomalyList, bool>();
    readonly string FoundAnomalies = "FoundAnomalies";



    // new system 
    private Dictionary<AreaNames, List<IAnomaly> >  anomaliesCollection = new Dictionary<AreaNames, List<IAnomaly>>();






    private void Awake()
    {
        Instance = this;


        //1- prepare all area lists empty
        PrepareMTAreaList();



    }


    void PrepareMTAreaList()
    {
        foreach (AreaNames area in Enum.GetValues(typeof(AreaNames)))
            anomaliesCollection[area] = new List<IAnomaly>();
    }


    private void Start()
    {
        normalRunCounter = 0;

        isFirstRun = true;
        //SetAnomalyDic();
        isAnomalyRun=false;


        //2- find all anomalies => 3- categorise
        FindAllAnomalies();




        //print("anomaly count: "+anomalies.Count);

    }

    void FindAllAnomalies()
    {
        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (MonoBehaviour obj in allObjects)
        {
            if (obj is IAnomaly anomaly)
            {
                anomalies.Add(anomaly);
                // 3- categorize anomalies
                SeperateAnomalyByArea(anomaly);
                // 4- retrive saved data
                CallAllSavedAnomalyData(anomaly);
                Debug.Log(anomaly.AnomalyName + ", " +  anomaly.AreaName + " || " + anomaly.AppearanceCount + ", " +anomaly.IsDiscoverd);
            }

        }

    }
    void SeperateAnomalyByArea(IAnomaly anomaly)
    {

        anomaliesCollection[anomaly.AreaName].Add(anomaly);


    }

   // 4- retrive saved data
    void CallAllSavedAnomalyData(IAnomaly anomaly)
    {
        anomaly.SetData(AnomalySaveManager.instance.GetData(anomaly.AnomalyName.ToString()));
    }


    public void setAnomalyByName(string name)
    {
        ResetAnomaly();
       currentAnomaly = null;
        currentAnomaly = anomalies.FirstOrDefault(item => item.AnomalyName.ToString() == name);
        currentAnomaly.SetAnomaly();
        isAnomalyRun = true;

      //  Debug.Log("##anomaly name: " + currentAnomaly.AnomalyName);

    }



    public void SetAnomalyProbability()
    {
        if (currentAnomaly != null)
        {
            ResetAnomaly();

        }

        randomAnomaly = !isFirstRun && forcedAnomalyCounter == 0 && UnityEngine.Random.value > 0.3f;

        if (forcedAnomalyCounter > 0 || randomAnomaly)
        {
            if (forcedAnomalyCounter > 0)
                forcedAnomalyCounter--;

            isAnomalyRun = true;
            SetAnomaly();
            normalRunCounter = 0;
        }
        else
        {
            normalRunCounter++;

            if (normalRunCounter >= normalRunMax)
                forcedAnomalyCounter = forcedAnomalyCount;

            isFirstRun = false;
            isAnomalyRun = false;
           // print("No anomaly");
        }


    }
    
    public void SaveFoundAnomaly()
    {
       // if (foundAnomaliesDic[currentAnomaly.AnomalyName] == true) { return; }


       // //Debug.Log("##anomaly name: "+currentAnomaly.AnomalyName);
       // foundAnomaliesDic[currentAnomaly.AnomalyName] = true;
       // ES3.Save(FoundAnomalies, foundAnomaliesDic);
       //// Debug.Log("File saved");
       // if (checkAllTrue(foundAnomaliesDic) == true)
       // {
       //    // Debug.Log("steam ach"); 
       //     SteamAchWatcher.instance.AllAnomaliesDiscovered();
       // }

    }
    public void SetAnomaly()
    {
        //==========================================================





        currentAnomaly = GetRandomItem();

       // Debug.Log("##anomaly name: " + currentAnomaly.AnomalyName);

        currentAnomaly.SetAnomaly();
        

    }

  

    public IAnomaly GetRandomItem()
    {
        List<IAnomaly> available = anomalies
            .Where(item => !recentlyUsed.Contains(item))
            .ToList();

        if (available.Count == 0)
            available = anomalies;

        IAnomaly chosen = available[UnityEngine.Random.Range(0, available.Count)];

        recentlyUsed.Enqueue(chosen);
        if (recentlyUsed.Count > cooldownRounds)
            recentlyUsed.Dequeue();

        return chosen;
    }
    public void ResetAnomaly()
    {
        if (currentAnomaly != null)

        currentAnomaly.ResetAnomaly();
    }


    void SetAnomalyDic()
    {

        //if (ES3.KeyExists(FoundAnomalies))
        //{
        //    foundAnomaliesDic = ES3.Load<Dictionary<AnomalyList, bool>>(FoundAnomalies);
        //}
        //else
        //{
        //    // Optional fallback: loads a blank dictionary or populates default values
        //    foundAnomaliesDic = ES3.Load<Dictionary<AnomalyList, bool>>(FoundAnomalies, new Dictionary<AnomalyList, bool>());
           
        //    foreach (AnomalyList type in Enum.GetValues(typeof(AnomalyList)))
        //    {
        //        // Adds each enum value as a key, and sets the value to false
        //        foundAnomaliesDic.Add(type, false);

        //    }
        //    ES3.Save(FoundAnomalies, foundAnomaliesDic);

        //}


    }

    bool checkAllTrue(Dictionary<AnomalyList, bool> dict)
    {
        foreach (bool value in dict.Values)
        {
            if (value == false)
            {
                return false;
            }
        }
        return true;
    }


  //  GUIStyle style = new GUIStyle();


  //  void OnGUI()
  //  {

  //      //style.fontSize = 30;
  //      //style.normal.textColor = Color.black;
  //      //GUI.Label(
  //      //    new Rect(20, 60, 300, 50),
  //      //    "is Anomaly " + isAnomalyRun,
  //      //    style
  //      //);

  //      GUI.Label(
  //    new Rect(20, 60, 300, 50),
  //    "normal run counter " + normalRunCounter,
  //    style
  //);


  //      if (currentAnomaly == null) return;
  //      GUI.Label(
  //          new Rect(20, 90, 300, 50),
  //          "anomaly name " + currentAnomaly.AnomalyName,
  //          style
  //      );
  //  }
}

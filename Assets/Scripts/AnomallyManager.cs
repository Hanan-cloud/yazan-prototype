using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Steamworks;

public class AnomallyManager : MonoBehaviour
{
    public static AnomallyManager Instance;
    List<IAnomaly> anomalies = new();

    bool isAnomalyRun;
    public bool IsAnomalyRun { get => isAnomalyRun; }
    public IAnomaly CurrentAnomaly { get => currentAnomaly; }

    private List<IAnomaly> allItems;
    private List<IAnomaly> discoverdAnomaly = new();
    private Queue<IAnomaly> recentlyUsed = new Queue<IAnomaly>();
    private const int cooldownRounds = 3;


    int anomalyRunCounter;
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
    List<IAnomaly> candidates = new List<IAnomaly>();

    string lastAreaSelected = null;




    private void Awake()
    {
        Instance = this;



    }




    private void Start()
    {
        normalRunCounter = 0;

        isFirstRun = true;
        //SetAnomalyDic();
        isAnomalyRun=false;


        //2- find all anomalies => 3- categorise
        FindAllAnomalies();


        SelectCandidates();

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
                // 4- retrive saved data
                CallAllSavedAnomalyData(anomaly);
            }

        }

    }


    // 4- retrive saved data
    void CallAllSavedAnomalyData(IAnomaly anomaly)
    {
        anomaly.SetSavedData(AnomalySaveManager.instance.GetData(anomaly.AnomalyName.ToString()));
       Debug.Log(anomaly.AnomalyName.ToString() + ", " + anomaly.AreaName + " || " + anomaly.AppearanceCount + ", " + anomaly.IsDiscoverd);

    }



    //5- Select candidates
    private void SelectCandidates()
    {
        candidates.Clear();

        candidates = anomalies;

    }

    // Weight
    float GetWeight(IAnomaly anomaly, string lastArea)
    {
        float weight = 1f;

        // Never shown before = very important
        if (anomaly.AppearanceCount == 0)
        {
            weight += 5f;
        }

        // Player has not discovered it yet
        if (!anomaly.IsDiscoverd)
        {
            weight += 3f;
        }


        //Debug.Log($"Anomaly Area: [{anomaly.AreaName}]");
        //Debug.Log($"Last Area: [{lastArea}]");
        //Debug.Log($"Are Equal: {anomaly.AreaName.ToString() == lastArea}");


        if (anomaly.AreaName.ToString() == lastArea)
        {
            Debug.Log($"<color=green> last area </color>");

            weight *= 0.7f;
        }
        // The more we showed it, the less we want it
        weight -= anomaly.AppearanceCount * 0.5f;

        // Don't allow weight to become zero or negative
        weight = Mathf.Max(weight, 0.1f);

        return weight;
    }


    // 
    private IAnomaly PickWeighted(List<IAnomaly> candidates, string lastArea)
    {
        // First, calculate the total weight.
        float totalWeight = 0f;
        foreach (IAnomaly anomaly in candidates)
        {
            totalWeight += GetWeight(anomaly, lastAreaSelected);
        }


        // Pick a random point somewhere between 0 and totalWeight.
        float randomValue = UnityEngine.Random.Range(0f, totalWeight);


        // Walk through the anomalies one by one.
        foreach (IAnomaly anomaly in candidates)
        {
            // Remove this anomaly's "area" from our random value.
            randomValue -= GetWeight(anomaly, lastAreaSelected);

            Debug.Log($"<color=green> aaAAAa {anomaly.AreaName.ToString()} its num {randomValue} </color>");

            // If we reached zero, this is the winner.
            if (randomValue <= 0f)
            {

                return anomaly;
            }
        }


        // Safety fallback.
        return candidates[candidates.Count - 1];
    }


    void SaveAnomaly()
    {
        AnomalyData data = new AnomalyData();
        data.isDiscovered = currentAnomaly.IsDiscoverd;
        data.appearanceCount = currentAnomaly.AppearanceCount;

        AnomalySaveManager.instance.Save(currentAnomaly.AnomalyName.ToString(), data);


    }


    [ContextMenu("new anomaly random")]
    //public void CallNewRandom()
    //{
    //    IAnomaly a = PickWeighted(candidates,lastAreaSelected);
    //    Debug.Log("#####anomaly: "+ a.AnomalyName + " , " +a.AppearanceCount + " , " + a.AreaName);
    //    AnomalyData d = new AnomalyData();
    //    d.isDiscovered = false;
    //    d.appearanceCount = 1;
    //    //a.SetData(d);
    //    lastAreaSelected = a.AreaName.ToString();
    //}

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

            if(anomalyRunCounter > 8 && UnityEngine.Random.value < 0.6f )
            {
                SetDiscoverdAnomaly();
                Debug.Log($"<color=green> CHOOSE FROM => anomalyRunConter </color>");

            }
            else
            {

                SetAnomaly();
            }

         


            normalRunCounter = 0;
        }
        else
        {
            currentAnomaly=null;
            normalRunCounter++;

            if (normalRunCounter >= normalRunMax)
                forcedAnomalyCounter = forcedAnomalyCount;

            isFirstRun = false;
            isAnomalyRun = false;
           // print("No anomaly");
        }


    }
    



    void SetDiscoverdAnomaly()
    {


        currentAnomaly = discoverdAnomaly[UnityEngine.Random.Range(0, 6)];
        currentAnomaly.UpdateAppearanceCount();
        Debug.Log($"<color=green>  => currentAnomaly <= {currentAnomaly.AnomalyName} </color>");

        lastAreaSelected = currentAnomaly.AreaName.ToString();
        currentAnomaly.SetAnomaly();

        discoverdAnomaly.Clear();
        anomalyRunCounter = 0;

    }

    [ContextMenu("anomaly data")]
    public void PrintAllAnomalyData()
    {
        foreach (var item in anomalies)
        {
            Debug.Log($"<color=green> {item.AnomalyName} , {item.AppearanceCount} , {item.IsDiscoverd} </color>");


        }


    }
    public void SetAnomaly()
    {
        //==========================================================



        currentAnomaly = PickWeighted(candidates, lastAreaSelected);
        Debug.Log("#####anomaly: " + currentAnomaly.AnomalyName + " , " + currentAnomaly.AppearanceCount + " , " + currentAnomaly.AreaName);

        currentAnomaly.UpdateAppearanceCount();
        lastAreaSelected = currentAnomaly.AreaName.ToString();


       // Debug.Log("##anomaly name: " + currentAnomaly.AnomalyName);

        currentAnomaly.SetAnomaly();
        

    }

 
    public void ResetAnomaly(bool isWin = false)
    {
        if (currentAnomaly == null) return;

        if (isWin)
        {
            anomalyRunCounter++;
            Debug.Log($"<color=orange> anomalyRunConter: {anomalyRunCounter} </color>");

            currentAnomaly.UpdateDiscover();
            discoverdAnomaly.Add(currentAnomaly);
            if (discoverdAnomaly.Count > 0) 
            {
                Debug.Log($"<color=orange> discoverdAnomaly last item {discoverdAnomaly[discoverdAnomaly.Count-1]} </color>");

            }
        }
        SaveAnomaly();
        currentAnomaly.ResetAnomaly();
    }

 

   

    bool checkIfAllAnomalyDiscoverd(Dictionary<AnomalyList, bool> dict)
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

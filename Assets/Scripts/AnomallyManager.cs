using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    IAnomaly currentAnomaly;

    private Dictionary<AnomalyList, bool> foundAnomaliesDic = new Dictionary<AnomalyList, bool>();

    readonly string FoundAnomalies = "FoundAnomalies";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        isFirstRun = true;
        SetAnomalyDic();
        isAnomalyRun=false;

        MonoBehaviour[] allObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);


        foreach (MonoBehaviour obj in allObjects)
        {
            if (obj is IAnomaly anomaly)
            {
                anomalies.Add(anomaly);
            }
        }


        print("anomaly count: "+anomalies.Count);

    }






    public void SetAnomalyProbability()
    {
        if (currentAnomaly != null)
        {
            ResetAnomaly();

        }
        if (UnityEngine.Random.value > 0.3f && isFirstRun == false) // 11/2 % 0 
        {
            isAnomalyRun = true;
            //print("anomaly");
            SetAnomaly();

        }
        else
        {
            isFirstRun= false;
            isAnomalyRun = false;
            print("No anomaly");

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

        Debug.Log("##anomaly name: " + currentAnomaly.AnomalyName);

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


    //GUIStyle style = new GUIStyle();


    //void OnGUI()
    //{

    //    style.fontSize = 30;
    //    style.normal.textColor = Color.black;
    //    GUI.Label(
    //        new Rect(20, 60, 300, 50),
    //        "is Anomaly " + isAnomalyRun,
    //        style
    //    );



    //    if (currentAnomaly == null ) return; 
    //    GUI.Label(
    //        new Rect(20, 90, 300, 50),
    //        "anomaly name " + currentAnomaly.AnomalyName,
    //        style
    //    );
    //}
}

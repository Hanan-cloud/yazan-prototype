using UnityEngine;
using System.Collections.Generic;


public class AnomallyManager : MonoBehaviour
{
    public static AnomallyManager Instance;
    [SerializeField] List<IAnomaly> anomalies = new();
    bool isAnomalyRun;
    public bool IsAnomalyRun { get => isAnomalyRun; }

    IAnomaly currentAnomaly;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
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
        if (true)
        {
            isAnomalyRun = true;
            print("anomaly");
            SetAnomaly();

        }
        else
        {
            isAnomalyRun = false;
            print("No anomaly");

        }


    }


    public void SetAnomaly()
    {

        currentAnomaly = anomalies[Random.Range(0, anomalies.Count)];
        currentAnomaly.SetAnomaly();


    }




    public void ResetAnomaly()
    {
        if(currentAnomaly != null)     
        currentAnomaly.ResetAnomaly();
    }

    GUIStyle style = new GUIStyle();


    void OnGUI()
    {

        style.fontSize = 30;
        style.normal.textColor = Color.black;
        GUI.Label(
            new Rect(20, 60, 300, 50),
            "is Anomaly " + isAnomalyRun,
            style
        );




        GUI.Label(
            new Rect(20, 90, 300, 50),
            "Player direction " + PlayerController.Instance.PlayerCurrentDir,
            style
        );
    }
}

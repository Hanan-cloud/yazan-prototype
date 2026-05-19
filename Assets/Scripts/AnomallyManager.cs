using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class AnomallyManager : MonoBehaviour
{
    public static AnomallyManager Instance;
    [SerializeField] List<Anomaly> anomalies = new();
    bool isAnomalyRun;
    public bool IsAnomalyRun { get => isAnomalyRun; }

    Anomaly currentAnomaly;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        isAnomalyRun=false;
    }



    public void SetAnomalyProbability()
    {
        if (currentAnomaly != null)
        {
            ResetAnomaly();

        }
        if (Random.Range(1, 11) % 2 == 0)
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

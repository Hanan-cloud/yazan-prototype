using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AnomalyScore : MonoBehaviour
{
    private const string Key = "anomaliesData";

    private Dictionary<string, AnomalyData> data = new();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI discoveredCountText;

    private int discoveredCount;

    void Start()
    {
        LoadAndCountDiscovered();
    }

    public int LoadAndCountDiscovered()
    {
        discoveredCount = 0;

        if (ES3.KeyExists(Key))
        {
            data = ES3.Load<Dictionary<string, AnomalyData>>(Key);

            if (data != null)
            {
                foreach (var anomaly in data.Values)
                {
                    if (anomaly != null && anomaly.isDiscovered)
                    {
                        discoveredCount++;
                    }
                }
            }
        }

        if (discoveredCountText != null)
        {
            discoveredCountText.text = $" U Found {discoveredCount.ToString()} out of 15";
        }

        Debug.Log($"Discovered Anomalies: {discoveredCount}");

        return discoveredCount;
    }
}
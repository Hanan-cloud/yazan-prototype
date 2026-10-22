using UnityEngine;
using System.Collections.Generic;
using TMPro;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using AHAKuo.Signalia.LocalizationStandalone.Framework;

public class AnomalyScore : MonoBehaviour
{
    private const string Key = "anomaliesData";

    private Dictionary<string, AnomalyData> data = new();

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI discoveredCountText;

    private int discoveredCount;

    [SerializeField] private SimpleLocalizedText simpleText;

    private LocalizationEvents.LanguageChangeSubscription localizationSubscription;

    private string discoveredCountLabel = string.Empty;

    private void Awake()
    {
        if (simpleText != null)
        {
            simpleText.enabled = false;
        }
        else if (discoveredCountText != null)
        {
            discoveredCountLabel = discoveredCountText.text;
        }
    }

    private void Start()
    {
        localizationSubscription = LocalizationEvents.Subscribe(UpdateDiscoveredCountText, gameObject);
        LoadAndCountDiscovered();
    }

    private void OnDestroy()
    {
        localizationSubscription?.Dispose();
    }

    public void LoadAndCountDiscovered()
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

        Debug.Log($"Discovered Anomalies: {discoveredCount}");
        UpdateDiscoveredCountText();
    }

    private void UpdateDiscoveredCountText()
    {
        if (discoveredCountText != null)
        {
            if (simpleText != null)
            {
                simpleText.UpdateText();
                discoveredCountLabel = discoveredCountText.text;
            }

            if(LocalizationRuntime.CurrentLanguageCode == "ar")
            discoveredCountText.text = $"{discoveredCountLabel}{discoveredCount}/51";
            else
            discoveredCountText.text = $"{discoveredCountLabel}{discoveredCount}/15";
        }
    }
}
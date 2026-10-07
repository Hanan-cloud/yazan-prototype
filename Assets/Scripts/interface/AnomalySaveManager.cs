using System.Collections.Generic;
using UnityEngine;

public class AnomalySaveManager : MonoBehaviour
{
    public static AnomalySaveManager instance;

    const string Key = "anomaliesData";

    Dictionary<string, AnomalyData> data = new();
    private void Awake()
    {
        if (instance == null)
        instance = this;
  
    }

    public void Load(string anomalyId)
    {
     
        if (ES3.KeyExists(Key))
        {
            data = ES3.Load<Dictionary<string, AnomalyData>>(Key);
        }
        else
        {
            data = new Dictionary<string, AnomalyData>();
            Save(Key, data); // first run: write an empty save

        }

    }

    

    public void Save(string k, Dictionary<string, AnomalyData> d)
    {
        ES3.Save(k, d); 
    
    }

    // Returns the anomaly's data, creating a default entry if it doesn't exist yet
    public AnomalyData GetData(string id)
    {
        if (!data.ContainsKey(id))
            data[id] = new AnomalyData();

        return data[id];
    }


    void InitializeSaveData(string Key)
    {
        Dictionary<string, AnomalyData> dataIni = new();
        List<string> anomalyNames = new();



        for (int i = 0; i < anomalyNames.Count; i++) 
        {
            dataIni[anomalyNames[i]] = new AnomalyData();

        }
        ES3.Save(Key, data);
    }

}

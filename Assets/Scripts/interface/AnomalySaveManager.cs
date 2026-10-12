using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnomalySaveManager : MonoBehaviour
{
    public static AnomalySaveManager instance;

    const string Key = "anomaliesData";

    Dictionary<string, AnomalyData> data = new();
    private void Awake()
    {
        if (instance == null)
        instance = this;
        Load();
    }


    private void Start()
    {
        
    }

    public void Load()
    {
     
        if (ES3.KeyExists(Key))
        {
            data = ES3.Load<Dictionary<string, AnomalyData>>(Key);
        }
        else
        {
            data = new Dictionary<string, AnomalyData>();
            SaveAllDic( data); // first run: write an empty save

        }


    }


    public void Save(string a , AnomalyData d )
    {
        data[a] = d;
        SaveAllDic(data);


    }

    void SaveAllDic(Dictionary<string, AnomalyData> d)
    {
        ES3.Save(Key, data);
    }

    // Returns the anomaly's data, creating a default entry if it doesn't exist yet
    public AnomalyData GetData(string id)
    {
        if (data.TryGetValue(id, out AnomalyData value))
        {
            return value;
        }

        value = new AnomalyData();
        data[id] = value;

        return value;
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

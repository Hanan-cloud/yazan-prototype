using System;
using UnityEngine;

[RequireComponent(typeof(AnomalyNameSetter))]
public class Anomaly : MonoBehaviour, IAnomaly
{

    [SerializeField] GameObject normal;
    [SerializeField] GameObject abnormal;

    [SerializeField]
    private AnomalyList anomalyName;

    public AnomalyList AnomalyName
    {
        get => anomalyName;

    }

    private void Start()
    {
        SetAnomalyName();
    }
    public void SetAnomaly()
    {

        normal.SetActive(false);
        abnormal.SetActive(true);

    }
    
    
    public void ResetAnomaly()
    {

        normal.SetActive(true);
        abnormal.SetActive(false);

    }



    public void SetAnomalyName()
    {
        if (Enum.TryParse(GetComponent<AnomalyNameSetter>().AnomalyName, out AnomalyList name))
        {

            anomalyName = name;
        }
        else
        {
            Debug.LogWarning("Enum Doesn't exist");
        }
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternFlameAnomaly : MonoBehaviour,IAnomaly
{

    [SerializeField]
    private AnomalyList anomalyName;

    public AnomalyList AnomalyName
    {
        get => anomalyName;
    }


    [SerializeField] Color originalColor;
    [SerializeField] Color anomalyColor;

    [SerializeField] List<Light2D> flames;

    private void Start()
    {
        SetAnomalyName();
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
    public void ResetAnomaly()
    {
        foreach (Light2D light in flames) { 
        
            light.color= originalColor;
        }

    }

    public void SetAnomaly()
    {
        foreach (Light2D light in flames)
        {

            light.color = anomalyColor;
        }
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternFlameAnomaly : AnomalyBase
{

    //[SerializeField]
    //private AnomalyList anomalyName;

    //public AnomalyList AnomalyName
    //{
    //    get => anomalyName;
    //}


    [SerializeField] Color originalColor;
    [SerializeField] Color anomalyColor;

    [SerializeField] List<Light2D> flames;

    private void Start()
    {
        SetAnomalyName();
    }


 
    public override void ResetAnomaly()
    {
        foreach (Light2D light in flames) { 
        
            light.color= originalColor;
        }

    }

    public override void SetAnomaly()
    {
        foreach (Light2D light in flames)
        {

            light.color = anomalyColor;
        }
    }

}

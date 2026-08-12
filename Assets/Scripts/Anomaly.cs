using System;
using UnityEngine;

[RequireComponent(typeof(AnomalyNameSetter))]
public class Anomaly : AnomalyBase
{

    [SerializeField] GameObject normal;
    [SerializeField] GameObject abnormal;

    //[SerializeField]
    //private AnomalyList anomalyName;

    //public AnomalyList AnomalyName
    //{
    //    get => anomalyName;

    //}

    private void Start()
    {
        SetAnomalyName();
    }
    public override void SetAnomaly()
    {

        normal.SetActive(false);
        abnormal.SetActive(true);

    }
    
    
    public override void ResetAnomaly()
    {

        normal.SetActive(true);
        abnormal.SetActive(false);

    }



   

}

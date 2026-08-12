using System;
using UnityEngine;
[RequireComponent(typeof(AnomalyNameSetter))]
public class MirrorRefAnomaly : AnomalyBase
{
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
    public  override void ResetAnomaly()
    {
        gameObject.SetActive(true);

    }

    public override void SetAnomaly()
    {
        gameObject.SetActive(false);
    }


   

}

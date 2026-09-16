using System;
using UnityEngine;
[RequireComponent(typeof(AnomalyNameSetter))]

public class OppositeShadowAnomaly : AnomalyBase, ILateSet
{

    [SerializeField] GameObject yazanClone;


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
    public override void ResetAnomaly()
    {
        yazanClone.SetActive(false);

    }


    [ContextMenu("startAnom")]
    public override void SetAnomaly()
    {
        yazanClone.SetActive(true);
    }

    public void LateSetAnomaly()
    {
        yazanClone.SetActive(false);
        yazanClone.SetActive(true);

    }
}

using System.Collections.Generic;
using UnityEngine;

public class GeneralSwapper : AnomalyBase
{


    [SerializeField] List<SwapPair> pairs;
    public override void ResetAnomaly()
    {
        foreach(var item in pairs)
        {
            if(item != null)
            {
                item.original.SetActive(true);
                item.replacement.SetActive(false);
            }
        }    
    }

    public override void SetAnomaly()
    {
        foreach(var item in pairs)
        {
            if(item != null)
            {
                item.original.SetActive(false);
                item.replacement.SetActive(true);
            }
        }
    }

    void Start()
    {
        SetAnomalyName();
    }


}

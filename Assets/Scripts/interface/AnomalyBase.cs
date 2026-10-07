using System;
using UnityEngine;

[RequireComponent(typeof(AnomalyNameSetter))]
public abstract class AnomalyBase : MonoBehaviour, IAnomaly
{
    [SerializeField] protected AnomalyList anomalyName;
    [SerializeField] protected AreaNames areaName;
    
    bool isDiscoverd = false;
    
    int appearanceCount = 0;


    public AnomalyList AnomalyName => anomalyName;
    public AreaNames AreaName => areaName;

    public bool IsDiscoverd
    {
        get => isDiscoverd;

    }

    public int AppearanceCount
    {
        get => appearanceCount;
    }

    public void SetData(AnomalyData d) 
    {
        isDiscoverd = d.isDiscovered;
        appearanceCount = d.appearanceCount;


    }

    public virtual void SetAnomalyName()
    {
        if (TryGetComponent<AnomalyNameSetter>(out AnomalyNameSetter names))
        {
            anomalyName = names.AnomalyName;
            areaName = names.AreaName;
        }
        else
        {
            Debug.LogWarning("Enum Doesn't exist");
        }
    }

    public abstract void SetAnomaly();
    public abstract void ResetAnomaly();
}
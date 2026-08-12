using System;
using UnityEngine;

[RequireComponent(typeof(AnomalyNameSetter))]
public abstract class AnomalyBase : MonoBehaviour, IAnomaly
{
    [SerializeField] protected AnomalyList anomalyName;

    public AnomalyList AnomalyName => anomalyName;

    public virtual void SetAnomalyName()
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

    public abstract void SetAnomaly();
    public abstract void ResetAnomaly();
}
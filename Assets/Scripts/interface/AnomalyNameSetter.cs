using UnityEngine;

public class AnomalyNameSetter : MonoBehaviour
{


    [SerializeField] AnomalyList anomalyName;
    [SerializeField] AreaNames areaName;

    public AnomalyList AnomalyName { get => anomalyName; }
    public AreaNames AreaName { get => areaName; }
}

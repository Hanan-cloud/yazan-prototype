using UnityEngine;

public class AnomalyNameSetter : MonoBehaviour
{


    [SerializeField] string anomalyName;

    public string AnomalyName { get => anomalyName; }
}

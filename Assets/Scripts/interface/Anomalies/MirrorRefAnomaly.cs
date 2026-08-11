using System;
using UnityEngine;
[RequireComponent(typeof(AnomalyNameSetter))]
public class MirrorRefAnomaly : MonoBehaviour, IAnomaly
{
    [SerializeField]
    private AnomalyList anomalyName;

    public AnomalyList AnomalyName
    {
        get => anomalyName;
    }

    private void Start()
    {
        SetAnomalyName();
    }
    public void ResetAnomaly()
    {
        gameObject.SetActive(true);

    }

    public void SetAnomaly()
    {
        gameObject.SetActive(false);
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

}

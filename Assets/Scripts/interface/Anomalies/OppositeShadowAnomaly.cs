using System;
using UnityEngine;
[RequireComponent(typeof(AnomalyNameSetter))]

public class OppositeShadowAnomaly : MonoBehaviour, IAnomaly
{

    [SerializeField] GameObject yazanClone;


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
        yazanClone.SetActive(false);

    }


    [ContextMenu("startAnom")]
    public void SetAnomaly()
    {
        yazanClone.SetActive(true);
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

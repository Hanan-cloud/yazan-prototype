using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternFlameAnomaly : MonoBehaviour,IAnomaly
{
    [SerializeField] Color originalColor;
    [SerializeField] Color anomalyColor;

    [SerializeField] List<Light2D> flames;
    public void ResetAnomaly()
    {
        foreach (Light2D light in flames) { 
        
            light.color= originalColor;
        }

    }

    public void SetAnomaly()
    {
        foreach (Light2D light in flames)
        {

            light.color = anomalyColor;
        }
    }

}

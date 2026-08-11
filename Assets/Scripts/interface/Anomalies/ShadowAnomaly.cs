using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(AnomalyNameSetter))]

public class ShadowAnomaly : MonoBehaviour, IAnomaly
{
    [SerializeField] SpriteRenderer sprite;


    Tween t;

    [SerializeField]
    private AnomalyList anomalyName;

    public AnomalyList AnomalyName
    {
        get => anomalyName;

    }

    public void ResetAnomaly()
    {
        t.Kill();
       t= sprite.DOFade(0, 2);
    }

    public void SetAnomaly()
    {
        t.Kill();

        t = sprite.DOFade(0.7f, 30);
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

    void Start()
    {
        SetAnomalyName();
        sprite.DOFade(0, 0);

    }


}

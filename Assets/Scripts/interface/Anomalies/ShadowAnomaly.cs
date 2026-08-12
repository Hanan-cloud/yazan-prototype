using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(AnomalyNameSetter))]

public class ShadowAnomaly : AnomalyBase
{
    [SerializeField] SpriteRenderer sprite;


    Tween t;

    //[SerializeField]
    //private AnomalyList anomalyName;

    //public AnomalyList AnomalyName
    //{
    //    get => anomalyName;

    //}

    public override void ResetAnomaly()
    {
        t.Kill();
       t= sprite.DOFade(0, 2);
    }

    public override void SetAnomaly()
    {
        t.Kill();

        t = sprite.DOFade(0.7f, 30);
    }

 

    void Start()
    {
        SetAnomalyName();
        sprite.DOFade(0, 0);

    }


}

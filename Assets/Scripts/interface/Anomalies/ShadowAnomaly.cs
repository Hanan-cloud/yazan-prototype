using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AnomalyNameSetter))]

public class ShadowAnomaly : AnomalyBase, ILateSet
{
    [SerializeField] SpriteRenderer sprite;


    Tween t;

    private void Awake()
    {
        SetAnomalyName();

    }
    public void LateSetAnomaly()
    {
        t.Kill();

        t = sprite.DOFade(0.7f, 30);
    }



    public override void ResetAnomaly()
    {
        t.Kill();
       t= sprite.DOFade(0, 0.5f);
    }

    public override void SetAnomaly()
    {

    }

 

    void Start()
    {
        sprite.DOFade(0, 0);

    }


}

using UnityEngine;
using DG.Tweening;
public class ShadowAnomaly : MonoBehaviour, IAnomaly
{
    [SerializeField] SpriteRenderer sprite;


    Tween t;
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

    void Start()
    {
        sprite.DOFade(0, 0);

    }


}

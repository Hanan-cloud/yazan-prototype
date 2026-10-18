
using System;
using DG.Tweening;
using UnityEngine;

[Serializable] 
public class GlowSettings
{
    
public Color color;
public float intensity;

}

public class GlowController : MonoBehaviour
{
    static readonly int ColorId = Shader.PropertyToID("_Color");

    [Header("OffSetting")]
    [SerializeField] GlowSettings off;
    [SerializeField] GlowSettings canRead;
    [SerializeField] GlowSettings read;



    [SerializeField] float duration = 5f;

    SpriteRenderer sr;
    MaterialPropertyBlock mpb;

    Color currentColor = Color.black;
    float currentIntensity = 3f;

    Tween tween;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    currentColor = Color.black;
       //Apply(currentColor, currentIntensity);
    }

    [ContextMenu("Off")]

    public void CallOff()
    {
        FadeGlow(off);
    }
    [ContextMenu("can read")]

    public void CallCanRead()
    {
        FadeGlow(canRead);
    }
    [ContextMenu("read")]

       public void Callread()
    {
        FadeGlow(read);
    }

    public void FadeGlow(GlowSettings g)
    {
        tween?.Kill();

        Color startColor = currentColor;
        float startIntensity = currentIntensity;

        tween = DOVirtual.Float(0f, 1f, duration, t =>
        {
            Color color = Color.Lerp(startColor, g.color, t);
            float intensity = Mathf.Lerp(startIntensity, g.intensity, t);

            Apply(color, intensity);
        })
        .SetEase(Ease.Linear);
    }

    void Apply(Color color, float intensity)
    {
        currentColor = color;
        currentIntensity = intensity;

        sr.GetPropertyBlock(mpb);

        mpb.SetColor(ColorId, color * intensity);

        sr.SetPropertyBlock(mpb);
    }

    void OnDestroy()
    {
        tween?.Kill();
    }
}

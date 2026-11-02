using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ObjectScaler : MonoBehaviour
{
  [Header("Objects")]
    [SerializeField] private List<Transform> objectsToScale = new List<Transform>();

    [Header("Scale Settings")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private float scaleAmount = 0.5f;

    private Dictionary<Transform, Vector3> originalScales =
        new Dictionary<Transform, Vector3>();

    private List<Tween> activeTweens = new List<Tween>();

    private void Awake()
    {
        SaveOriginalScales();
    }

    private void SaveOriginalScales()
    {
        originalScales.Clear();

        foreach (Transform obj in objectsToScale)
        {
            if (obj == null) continue;

            if (!originalScales.ContainsKey(obj))
            {
                originalScales.Add(obj, obj.localScale);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        ScaleUpObjects();
    }

    public void ScaleUpObjects()
    {
        KillActiveTweens();

        foreach (Transform obj in objectsToScale)
        {
            if (obj == null || !originalScales.ContainsKey(obj))
                continue;

            Vector3 originalScale = originalScales[obj];

            Vector3 targetScale = originalScale + Vector3.one * scaleAmount;

            Tween tween = obj.DOScale(targetScale, duration)
                .SetEase(Ease.OutSine);

            activeTweens.Add(tween);
        }
    }

    public void ResetScales()
    {
        KillActiveTweens();

        foreach (Transform obj in objectsToScale)
        {
            if (obj == null || !originalScales.ContainsKey(obj))
                continue;

            obj.localScale = originalScales[obj];
        }
    }

    private void KillActiveTweens()
    {
        foreach (Tween tween in activeTweens)
        {
            if (tween != null && tween.IsActive())
                tween.Kill();
        }

        activeTweens.Clear();
    }

    private void OnDisable()
    {        
        KillActiveTweens();

        ResetScales();
    }
    private void OnDestroy()
    {
    }
}

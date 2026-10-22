
using UnityEngine;
using DG.Tweening;

public class CreditsScroll : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform creditsText;

    [Header("Scroll Settings")]
    [SerializeField] private float duration = 40f;
    [SerializeField] private float extraDistance = 500f;

    private Vector2 startPosition;
    private Tween scrollTween;

    private void Awake()
    {
        if (creditsText != null)
            startPosition = creditsText.anchoredPosition;
    }

    public void OpenCredits()
    {
        gameObject.SetActive(true);

        // Wait until the Canvas is enabled and its layout is updated.
        Canvas.ForceUpdateCanvases();

        scrollTween?.Kill();

        creditsText.anchoredPosition = startPosition;

        float distance = creditsText.rect.height + extraDistance;

        scrollTween = creditsText
            .DOAnchorPosY(startPosition.y + distance, duration)
            .SetDelay(2f)
            .SetEase(Ease.Linear)
            .SetUpdate(true);
    }

    public void CloseCredits()
    {
        scrollTween?.Kill();
        scrollTween = null;

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        scrollTween?.Kill();
        scrollTween = null;
    }

    private void OnDestroy()
    {
        scrollTween?.Kill();
    }
}

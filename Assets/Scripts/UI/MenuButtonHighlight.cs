
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MenuButtonHighlight : MonoBehaviour,
    ISelectHandler,
    IDeselectHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Highlight Images")]
    [SerializeField] private Image pointer;
    [SerializeField] private Image line;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.2f;

    private bool isSelected;
    private bool isHovered;

    private Tween pointerTween;
    private Tween lineTween;

    private void Awake()
    {
        SetAlpha(pointer, 0f);
        SetAlpha(line, 0f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        UpdateHighlight();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        UpdateHighlight();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateHighlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        float targetAlpha = (isSelected || isHovered) ? 1f : 0f;

        pointerTween?.Kill();
        lineTween?.Kill();

        if (pointer != null)
        {
            pointerTween = pointer
                .DOFade(targetAlpha, fadeDuration)
                .SetUpdate(true);
        }

        if (line != null)
        {
            lineTween = line
                .DOFade(targetAlpha, fadeDuration)
                .SetUpdate(true);
        }
    }

    private void SetAlpha(Image image, float alpha)
    {
        if (image == null) return;

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private void OnDestroy()
    {
        pointerTween?.Kill();
        lineTween?.Kill();
    }
}

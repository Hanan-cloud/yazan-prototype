using UnityEngine;
using DG.Tweening;
using System;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PauseMenuVisualController : MonoBehaviour
{
    [Header("Pause Background")]
    [SerializeField] private RectTransform pauseBgRect;

    [SerializeField] private float slideDistance = 900f;
    [SerializeField] private float slideDuration = 0.6f;
    [SerializeField] private Ease openEase = Ease.OutCubic;
    [SerializeField] private Ease closeEase = Ease.InCubic;

    [Header("Pause Background Fade")]
    [SerializeField] private float pauseBgFadeDuration = 0.3f;

    private Image pauseBgImage;

    private Vector2 targetPosition;
    private Vector2 hiddenPosition;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private bool isArabic;

    private Tween pauseBgTween;


    // --------------------------------------------------
    // PAUSE MASK ANIMATION
    // --------------------------------------------------

    [Header("Pause Mask Animation")]
    [SerializeField] private RectMask2D pauseMask;
[SerializeField] private float maskOpenDuration = 0.6f;
[SerializeField] private float maskCloseDuration = 0.6f;
    private Tween maskTween;

    private Vector4 maskOpenPadding;
    private Vector4 maskClosedPadding;


    // --------------------------------------------------
    // PAUSE WORD
    // --------------------------------------------------

    [Header("Pause Word")]
    [SerializeField] private GameObject pauseWordObject;
    [SerializeField] private TMP_Text pauseWordText;
    [SerializeField] private float pauseWordFadeDuration = 0.3f;

    private Tween pauseWordTween;


    // --------------------------------------------------
    // GAME OVERLAY COLOR
    // --------------------------------------------------

    [Header("Game Overlay Color")]
    [SerializeField] private Image gameOverlayColor;
    [SerializeField] private float overlayAlpha = 0.67f;
    [SerializeField] private float overlayFadeDuration = 0.3f;

    private Tween overlayTween;


    // --------------------------------------------------
    // PAUSE BUTTONS ANIMATION
    // --------------------------------------------------

    [Header("Pause Buttons Animation")]
    [SerializeField] private CanvasGroup pauseButtonsCanvasGroup;
    [SerializeField] private List<VerticalLayoutGroup> VerticalGroups;
    [SerializeField] private HorizontalLayoutGroup settingTitleGroup;
    [SerializeField] private HorizontalLayoutGroup settingsGroup;

    [SerializeField] private float buttonsFadeDuration = 0.3f;

    private Tween buttonsFadeTween;


    // --------------------------------------------------
    // AWAKE
    // --------------------------------------------------

    private void Awake()
    {
        isArabic =
            LocalizationRuntime.CurrentLanguageCode == "ar";


        // PAUSE BACKGROUND

        if (pauseBgRect != null)
        {
            pauseBgImage = pauseBgRect.GetComponent<Image>();

            SetButtonsAlignment();

            targetPosition = pauseBgRect.anchoredPosition;

            originalScale = pauseBgRect.localScale;
            targetScale = originalScale;

            if (isArabic)
            {
                targetScale.x = -Mathf.Abs(originalScale.x);
            }
            else
            {
                targetScale.x = Mathf.Abs(originalScale.x);
            }

            pauseBgRect.localScale = targetScale;

            float direction = isArabic ? 1f : -1f;

            hiddenPosition = targetPosition +
                new Vector2(slideDistance * direction, 0f);

            if (pauseBgImage != null)
                SetAlpha(pauseBgImage, 0f);
        }


        // PAUSE MASK

        if (pauseMask != null)
        {
            Canvas.ForceUpdateCanvases();

            float maskWidth = pauseMask.rectTransform.rect.width;

            // Open state: no padding
            maskOpenPadding = Vector4.zero;

            // RectMask2D Padding:
            // x = Left
            // y = Bottom
            // z = Right
            // w = Top

            if (isArabic)
            {
                // Arabic: reveal from Right to Left
                maskClosedPadding =
                    new Vector4(maskWidth, 0f, 0f, 0f);
            }
            else
            {
                // English: reveal from Left to Right
                maskClosedPadding =
                    new Vector4(0f, 0f, maskWidth, 0f);
            }

            pauseMask.padding = maskClosedPadding;
        }


        // GAME OVERLAY

        if (gameOverlayColor != null)
        {
            SetAlpha(gameOverlayColor, 0f);
        }


        // INITIAL PAUSE WORD STATE

        if (pauseWordObject != null)
            pauseWordObject.SetActive(false);

        if (pauseWordText != null)
            SetTextAlpha(pauseWordText, 0f);
    }


    // --------------------------------------------------
    // PAUSE BACKGROUND OPEN
    // --------------------------------------------------

    public void PlayOpenAnimation(bool showPauseWord = true)
    {
        if (pauseBgRect == null)
            return;

        pauseBgTween?.Kill();
        overlayTween?.Kill();
        maskTween?.Kill();

        pauseBgRect.localScale = targetScale;
        pauseBgRect.anchoredPosition = hiddenPosition;

        if (pauseBgImage != null)
            SetAlpha(pauseBgImage, 0f);

        if (gameOverlayColor != null)
            SetAlpha(gameOverlayColor, 0f);


        // Reset Mask to Closed State

        if (pauseMask != null)
        {
            pauseMask.padding = maskClosedPadding;
        }


        Sequence openSequence = DOTween.Sequence();

        openSequence.SetUpdate(true);


        // Background Slide In

        openSequence.Append(
            pauseBgRect
                .DOAnchorPos(targetPosition, slideDuration)
                .SetEase(openEase)
        );


        // Background Fade In

        if (pauseBgImage != null)
        {
            openSequence.Join(
                pauseBgImage
                    .DOFade(1f, pauseBgFadeDuration)
                    .SetEase(Ease.OutCubic)
            );
        }


        // Game Overlay Fade In

        if (gameOverlayColor != null)
        {
            openSequence.Join(
                gameOverlayColor
                    .DOFade(overlayAlpha, overlayFadeDuration)
                    .SetEase(Ease.OutCubic)
            );
        }


        // Mask Expand

        if (pauseMask != null)
        {
            openSequence.Join(
                AnimatePauseMask(true)
            );
        }


        pauseBgTween = openSequence;


        // Pause Word Fade In Only When Requested

        if (showPauseWord)
        {
            SetPauseWordVisible(true, false);
        }
    }


    // --------------------------------------------------
    // PAUSE BACKGROUND CLOSE
    // --------------------------------------------------

    public void PlayCloseAnimation(Action onComplete = null)
    {
        if (pauseBgRect == null)
        {
            onComplete?.Invoke();
            return;
        }

        pauseBgTween?.Kill();
        overlayTween?.Kill();
        maskTween?.Kill();


        // Fade Out PAUSE Word Independently

        SetPauseWordVisible(false, false);


        Sequence closeSequence = DOTween.Sequence();

        closeSequence.SetUpdate(true);


        // Background Slide Out

        closeSequence.Append(
            pauseBgRect
                .DOAnchorPos(hiddenPosition, slideDuration)
                .SetEase(closeEase)
        );


        // Background Fade Out

        if (pauseBgImage != null)
        {
            closeSequence.Join(
                pauseBgImage
                    .DOFade(0f, pauseBgFadeDuration)
                    .SetEase(Ease.InCubic)
            );
        }


        // Game Overlay Fade Out

        if (gameOverlayColor != null)
        {
            closeSequence.Join(
                gameOverlayColor
                    .DOFade(0f, overlayFadeDuration)
                    .SetEase(Ease.InCubic)
            );
        }


        // Mask Collapse

        if (pauseMask != null)
        {
            closeSequence.Join(
                AnimatePauseMask(false)
            );
        }


        closeSequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });


        pauseBgTween = closeSequence;
    }


    // --------------------------------------------------
    // PAUSE MASK ANIMATION
    // --------------------------------------------------

   private Tween AnimatePauseMask(bool opening)
{
    if (pauseMask == null)
        return null;

    maskTween?.Kill();

    Vector4 targetPadding = opening
        ? maskOpenPadding
        : maskClosedPadding;

    float duration = opening
        ? maskOpenDuration
        : maskCloseDuration;

    maskTween = DOTween.To(
        () => pauseMask.padding,
        value => pauseMask.padding = value,
        targetPadding,
        duration
    )
    .SetEase(opening ? openEase : closeEase)
    .SetUpdate(true);

    return maskTween;
}

    // --------------------------------------------------
    // PAUSE WORD VISIBILITY
    // --------------------------------------------------

    public void SetPauseWordVisible(bool visible, bool instant)
    {
        if (pauseWordObject == null || pauseWordText == null)
            return;

        pauseWordTween?.Kill();
        pauseWordTween = null;


        if (visible)
        {
            pauseWordObject.SetActive(true);

            if (instant)
            {
                SetTextAlpha(pauseWordText, 1f);
            }
            else
            {
                SetTextAlpha(pauseWordText, 0f);

                pauseWordTween = pauseWordText
                    .DOFade(1f, pauseWordFadeDuration)
                    .SetEase(Ease.OutCubic)
                    .SetUpdate(true);
            }
        }
        else
        {
            if (instant)
            {
                SetTextAlpha(pauseWordText, 0f);
                pauseWordObject.SetActive(false);
            }
            else
            {
                pauseWordTween = pauseWordText
                    .DOFade(0f, pauseWordFadeDuration)
                    .SetEase(Ease.InCubic)
                    .SetUpdate(true)
                    .OnComplete(() =>
                    {
                        pauseWordObject.SetActive(false);
                    });
            }
        }
    }


    // --------------------------------------------------
    // IMAGE / TEXT ALPHA
    // --------------------------------------------------

    private void SetAlpha(Image image, float alpha)
    {
        if (image == null)
            return;

        Color color = image.color;

        color.a = alpha;

        image.color = color;
    }


    private void SetTextAlpha(TMP_Text text, float alpha)
    {
        if (text == null)
            return;

        Color color = text.color;

        color.a = alpha;

        text.color = color;
    }


    // --------------------------------------------------
    // BUTTONS ALIGNMENT
    // --------------------------------------------------

    public void SetButtonsAlignment()
    {
        bool isArabic =
            LocalizationRuntime.CurrentLanguageCode == "ar";


        if (VerticalGroups != null)
        {
            foreach (var group in VerticalGroups)
            {
                if (group != null)
                {
                    group.childAlignment = isArabic
                        ? TextAnchor.MiddleRight
                        : TextAnchor.MiddleLeft;
                }
            }
        }


        if (settingTitleGroup != null)
        {
            settingTitleGroup.childAlignment = isArabic
                ? TextAnchor.UpperRight
                : TextAnchor.UpperLeft;
        }


        if (settingsGroup != null)
        {
            settingsGroup.childAlignment = isArabic
                ? TextAnchor.MiddleRight
                : TextAnchor.MiddleLeft;
        }
    }


    // --------------------------------------------------
    // PAUSE BUTTONS FADE
    // --------------------------------------------------

    public void PlayButtonsFadeIn()
    {
        if (pauseButtonsCanvasGroup == null)
        {
            Debug.LogError("Pause Buttons Canvas Group is NULL!");
            return;
        }

        buttonsFadeTween?.Kill();

        pauseButtonsCanvasGroup.alpha = 0f;

        buttonsFadeTween = pauseButtonsCanvasGroup
            .DOFade(1f, buttonsFadeDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
    }


    public void PlayButtonsFadeOut(Action onComplete = null)
    {
        if (pauseButtonsCanvasGroup == null)
        {
            Debug.LogError("Pause Buttons Canvas Group is NULL!");

            onComplete?.Invoke();

            return;
        }

        buttonsFadeTween?.Kill();

        buttonsFadeTween = pauseButtonsCanvasGroup
            .DOFade(0f, buttonsFadeDuration)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }


    // --------------------------------------------------
    // CLEANUP
    // --------------------------------------------------

    private void OnDisable()
    {
        pauseBgTween?.Kill();
        pauseBgTween = null;

        overlayTween?.Kill();
        overlayTween = null;

        buttonsFadeTween?.Kill();
        buttonsFadeTween = null;

        pauseWordTween?.Kill();
        pauseWordTween = null;

        maskTween?.Kill();
        maskTween = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

public class ComicCutsceneManager : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private List<ComicPage> pages = new List<ComicPage>();

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textDisplay;

    [Header("Image Animation")]
    [SerializeField] private float imageFadeDuration = 0.5f;
    [SerializeField] private float imageDelay = 0.3f;

    [Header("Page Transition")]
    [SerializeField] private float pageFadeOutDuration = 0.5f;

    [Header("Text Animation")]
    [SerializeField] private float textFadeDuration = 0.25f;

    [Header("Events")]
    public UnityEvent onCutsceneFinished;

    // Current State
    private int currentPageIndex = -1;
    private int currentTextIndex = 0;

    private bool isTransitioning = false;
    private bool waitingForInput = false;

    // Store original image scales
    private Dictionary<Transform, Vector3> originalScales =
        new Dictionary<Transform, Vector3>();


    private void Start()
    {
        InitializeCutscene();

        StartCoroutine(PlayPageRoutine(0));
    }


    #region Initialization

    private void InitializeCutscene()
    {
        foreach (ComicPage page in pages)
        {
            if (page == null || page.images == null)
                continue;

            if (page.images.Count > 5)
            {
                Debug.LogWarning(
                    "Comic Page contains more than 5 images. Only the first 5 will be used."
                );
            }

            int count = Mathf.Min(page.images.Count, 5);

            for (int i = 0; i < count; i++)
            {
                ComicImage image = page.images[i];

                if (image == null || image.spriteRenderer == null)
                    continue;

                SpriteRenderer sprite = image.spriteRenderer;

                // Store original scale
                if (!originalScales.ContainsKey(sprite.transform))
                {
                    originalScales.Add(
                        sprite.transform,
                        sprite.transform.localScale
                    );
                }

                // Hide image
                SetSpriteAlpha(sprite, 0);

                // Reset scale
                sprite.transform.localScale =
                    originalScales[sprite.transform];
            }
        }

        // Hide text
        if (textDisplay != null)
        {
            SetTextAlpha(0);
            textDisplay.text = "";
        }
    }

    #endregion


    #region Page Animation

    private IEnumerator PlayPageRoutine(int pageIndex)
    {
        isTransitioning = true;
        waitingForInput = false;

        // Check if cutscene is finished
        if (pageIndex >= pages.Count)
        {
            FinishCutscene();
            yield break;
        }

        ComicPage page = pages[pageIndex];

        if (page == null)
        {
            currentPageIndex = pageIndex;
            yield return PlayPageRoutine(pageIndex + 1);
            yield break;
        }

        currentPageIndex = pageIndex;
        currentTextIndex = 0;

        int imageCount = Mathf.Min(page.images.Count, 5);

        // Show images one by one
        for (int i = 0; i < imageCount; i++)
        {
            ComicImage image = page.images[i];

            if (image == null || image.spriteRenderer == null)
                continue;

            yield return ShowImage(image);

            if (imageDelay > 0)
                yield return new WaitForSeconds(imageDelay);
        }

        // Show first text
        if (page.texts != null && page.texts.Count > 0)
        {
            ShowText(0);

            if (textDisplay != null)
                yield return FadeText(1).WaitForCompletion();

            waitingForInput = true;
            isTransitioning = false;
        }
        else
        {
            // No texts: automatically move to next page
            yield return GoToNextPageRoutine();
        }
    }


    private IEnumerator ShowImage(ComicImage image)
    {
        SpriteRenderer sprite = image.spriteRenderer;

        SetSpriteAlpha(sprite, 0);

        Sequence sequence = DOTween.Sequence();

        // Fade In
        sequence.Join(
            DOTween.To(
                () => sprite.color.a,
                value => SetSpriteAlpha(sprite, value),
                1f,
                imageFadeDuration
            )
        );

        // Zoom In
        if (image.enableZoom)
        {
            Vector3 originalScale = originalScales[sprite.transform];

            Vector3 targetScale =
                originalScale * image.zoomScale;

            sequence.Join(
                sprite.transform
                    .DOScale(targetScale, image.zoomDuration)
                    .SetEase(image.zoomEase)
            );
        }

        yield return sequence.WaitForCompletion();
    }

    #endregion


    #region Next Button

    // Connect this function to your UI Button
    public void Next()
    {
        if (!waitingForInput || isTransitioning)
            return;

        waitingForInput = false;
        isTransitioning = true;

        ComicPage page = pages[currentPageIndex];

        // Check if there are more texts
        if (currentTextIndex < page.texts.Count - 1)
        {
            StartCoroutine(ShowNextTextRoutine());
        }
        else
        {
            // All texts finished
            StartCoroutine(GoToNextPageRoutine());
        }
    }


    private IEnumerator ShowNextTextRoutine()
    {
        // Fade out current text
        if (textDisplay != null)
            yield return FadeText(0).WaitForCompletion();

        // Move to next text
        currentTextIndex++;

        ShowText(currentTextIndex);

        // Fade in next text
        if (textDisplay != null)
            yield return FadeText(1).WaitForCompletion();

        waitingForInput = true;
        isTransitioning = false;
    }

    #endregion


    #region Page Transition

    private IEnumerator GoToNextPageRoutine()
    {
        isTransitioning = true;
        waitingForInput = false;

        // Fade out text
        if (textDisplay != null && textDisplay.color.a > 0)
        {
            yield return FadeText(0).WaitForCompletion();
        }

        // Fade out all current page images
        if (currentPageIndex >= 0 &&
            currentPageIndex < pages.Count)
        {
            ComicPage page = pages[currentPageIndex];

            if (page != null)
                yield return FadeOutPage(page);
        }

        // Start next page
        int nextPageIndex = currentPageIndex + 1;

        if (nextPageIndex >= pages.Count)
        {
            FinishCutscene();
            yield break;
        }

        yield return PlayPageRoutine(nextPageIndex);
    }


    private IEnumerator FadeOutPage(ComicPage page)
    {
        Sequence sequence = DOTween.Sequence();

        int imageCount = Mathf.Min(page.images.Count, 5);

        for (int i = 0; i < imageCount; i++)
        {
            ComicImage image = page.images[i];

            if (image == null || image.spriteRenderer == null)
                continue;

            SpriteRenderer sprite = image.spriteRenderer;

            // Fade Out
            sequence.Join(
                DOTween.To(
                    () => sprite.color.a,
                    value => SetSpriteAlpha(sprite, value),
                    0f,
                    pageFadeOutDuration
                )
            );

            // Restore original scale
            if (originalScales.TryGetValue(
                sprite.transform, out Vector3 originalScale))
            {
                sequence.Join(
                    sprite.transform.DOScale(
                        originalScale,
                        pageFadeOutDuration
                    )
                );
            }
        }

        if (sequence.Duration() > 0)
            yield return sequence.WaitForCompletion();
    }

    #endregion


    #region Text

    private void ShowText(int index)
    {
        if (textDisplay == null)
            return;

        ComicPage page = pages[currentPageIndex];

        textDisplay.text = page.texts[index];
    }


    private Tween FadeText(float targetAlpha)
    {
        if (textDisplay == null)
            return null;

        return DOTween.To(
            () => textDisplay.color.a,
            SetTextAlpha,
            targetAlpha,
            textFadeDuration
        );
    }


    private void SetTextAlpha(float alpha)
    {
        if (textDisplay == null)
            return;

        Color color = textDisplay.color;
        color.a = alpha;

        textDisplay.color = color;
    }

    #endregion


    #region Helpers

    private void SetSpriteAlpha(
        SpriteRenderer sprite,
        float alpha)
    {
        if (sprite == null)
            return;

        Color color = sprite.color;
        color.a = alpha;

        sprite.color = color;
    }


    private void FinishCutscene()
    {
        waitingForInput = false;
        isTransitioning = true;

        Debug.Log("Comic Cutscene Finished!");

        onCutsceneFinished?.Invoke();
    }

    #endregion


    private void OnDisable()
    {
        // Kill active tweens when cutscene is disabled
        foreach (var pair in originalScales)
        {
            if (pair.Key != null)
                pair.Key.DOKill();
        }

        if (textDisplay != null)
            textDisplay.DOKill();
    }
}

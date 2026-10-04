using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using AHAKuo.Signalia.LocalizationStandalone.Internal;

[System.Serializable]
public class CutsceneSlide
{
    public VideoClip shot;
    public VideoClip loopShot;
    public string textKey;
}

public class CutsceneManager : MonoBehaviour
{
    [Header("3 video players (Render Mode = Camera)")]
    [SerializeField] private VideoPlayer playerA;
    [SerializeField] private VideoPlayer playerB;
    [SerializeField] private VideoPlayer playerC;

    [Header("UI")]
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button skipButton;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeOverlay;
    [SerializeField] private float fadeDuration = 0.3f;

    [SerializeField] private CutsceneSlide[] slides;
    [SerializeField] private string nextSceneName;
    [SerializeField] private string textKey = "Panel_";

    private SimpleLocalizedText simpleText;

    private VideoPlayer activePlayer;
    private VideoPlayer loopPlayer;
    private VideoPlayer nextPlayer;

    private int index = -1;
    private bool isPlayingLoop = false;

    private int loopForIndex = -1;
    private bool loopPrepared = false;

    private int nextForIndex = -1;
    private bool nextPrepared = false;

    private bool pendingAdvance = false;

    private void Start()
    {
        simpleText = storyText.gameObject.GetComponent<SimpleLocalizedText>();

        nextButton.onClick.AddListener(OnNextPressed);
        skipButton.onClick.AddListener(Finish);

        foreach (var vp in new[] { playerA, playerB, playerC })
        {
            vp.enabled = true;
            vp.isLooping = false;
            vp.targetCameraAlpha = 0f;
            vp.loopPointReached += OnVideoFinished;
        }

        activePlayer = playerA;
        loopPlayer = playerB;
        nextPlayer = playerC;

        fadeOverlay.alpha = 1f;

        StartCoroutine(StartCutscene());
    }

    private void OnDestroy()
    {
        foreach (var vp in new[] { playerA, playerB, playerC })
            vp.loopPointReached -= OnVideoFinished;
    }

    private IEnumerator StartCutscene()
    {
        index = 0;
        LoadActiveDirect(slides[index]);

        yield return new WaitUntil(() => activePlayer.isPlaying);
        yield return Fade(0f);

        PrepareLoop(index);
        PrepareNext(index + 1);
    }

    private void LoadActiveDirect(CutsceneSlide slide)
    {
        isPlayingLoop = false;

        activePlayer.clip = slide.shot;
        activePlayer.isLooping = false;
        activePlayer.prepareCompleted -= OnActivePrepared;
        activePlayer.prepareCompleted += OnActivePrepared;
        activePlayer.Prepare();

        simpleText.SetKey(textKey + index);
    }

    private void OnActivePrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnActivePrepared;
        vp.targetCameraAlpha = 1f;
        vp.Play();
    }

    private void PrepareLoop(int forIndex)
    {
        loopForIndex = forIndex;
        loopPrepared = false;

        if (slides[forIndex].loopShot == null) return;

        loopPlayer.clip = slides[forIndex].loopShot;
        loopPlayer.isLooping = true;
        loopPlayer.targetCameraAlpha = 0f;
        loopPlayer.prepareCompleted -= OnLoopPrepared;
        loopPlayer.prepareCompleted += OnLoopPrepared;
        loopPlayer.Prepare();
    }

    private void OnLoopPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnLoopPrepared;
        vp.Play();
        vp.Pause();
        loopPrepared = true;
    }

    private void PrepareNext(int forIndex)
    {
        nextForIndex = forIndex;
        nextPrepared = false;

        if (forIndex >= slides.Length) return;

        nextPlayer.clip = slides[forIndex].shot;
        nextPlayer.isLooping = false;
        nextPlayer.targetCameraAlpha = 0f;
        nextPlayer.prepareCompleted -= OnNextPrepared;
        nextPlayer.prepareCompleted += OnNextPrepared;
        nextPlayer.Prepare();
    }

    private void OnNextPrepared(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnNextPrepared;
        vp.Play();
        vp.Pause();
        nextPrepared = true;

        if (pendingAdvance)
        {
            pendingAdvance = false;
            AdvanceToNextSlide();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (vp != activePlayer) return;
        if (isPlayingLoop) return;

        if (loopPrepared && loopForIndex == index)
        {
            VideoPlayer oldActive = SwapActiveTo(loopPlayer);
            isPlayingLoop = true;

            loopPlayer = oldActive;
            loopPrepared = false;
            loopForIndex = -1;
        }
    }

    private void OnNextPressed()
    {
        int target = index + 1;

        if (target >= slides.Length)
        {
            StartCoroutine(EndCutscene());
            return;
        }

        if (nextPrepared && nextForIndex == target)
        {
            AdvanceToNextSlide();
        }
        else
        {
            pendingAdvance = true;
        }
    }

    private void AdvanceToNextSlide()
    {
        int target = index + 1;

        VideoPlayer oldActive = SwapActiveTo(nextPlayer);
        VideoPlayer oldLoop = loopPlayer;

        index = target;
        isPlayingLoop = false;
        simpleText.SetKey(textKey + index);

        loopPlayer = oldActive;
        nextPlayer = oldLoop;

        PrepareLoop(index);
        PrepareNext(index + 1);
    }

    private VideoPlayer SwapActiveTo(VideoPlayer newActive)
    {
        newActive.targetCameraAlpha = 1f;
        newActive.Play();

        VideoPlayer oldActive = activePlayer;
        oldActive.targetCameraAlpha = 0f;
        oldActive.Pause();

        activePlayer = newActive;
        return oldActive;
    }

    private IEnumerator EndCutscene()
    {
        yield return Fade(1f);
        Finish();
    }

    private IEnumerator Fade(float target)
    {
        float start = fadeOverlay.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeOverlay.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        fadeOverlay.alpha = target;
    }

    private void Finish()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
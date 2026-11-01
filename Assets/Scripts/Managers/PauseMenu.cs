
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.InputSystem;
using System;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public static event Action<bool> StoppedStateChanged;

    [SerializeField] private InputActionReference cancelAction;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButtonsPanel;
    [SerializeField] private GameObject dollPanel;
    [SerializeField] private GameObject SettingsPanel;

    [Header("Doll Animation")]
    [SerializeField] private RectTransform dollRect;
    [SerializeField] private float dollDuration = 0.4f;
    [SerializeField] private float startScale = 0.01f;
    [SerializeField] private float stretchX = 1.08f;
    [SerializeField] private float stretchY = 0.92f;
    [SerializeField] private float closeDuration = 0.25f;

    [Header("Audio")]
    [SerializeField] private AudioClip ClipOnOpen;
    [SerializeField] private AudioClip ClipOnClose;

    [Header("Events")]
    [SerializeField] private UnityEvent OnSetContinueButton;
    [SerializeField] private UnityEvent OnSetMasterSlider;

    private bool isPaused;
    private bool isDolled;
    private int pauseClosedFrame = -1;

    private bool isDollAnimating;
    private bool isDollClosing;

    private AudioSource audioSource;
    private Tween dollTween;

    public bool IsStopped => isPaused || isDolled;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        if (dollRect == null && dollPanel != null)
            dollRect = dollPanel.GetComponent<RectTransform>();

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SubscribeInputEvents();

        if (cancelAction != null && cancelAction.action != null)
            cancelAction.action.performed += OnCancelPerformed;
    }

    private void Start()
    {
        Cursor.visible = false;

        SetPausedState(false);
        SetDolledState(false);
        isDollAnimating = false;
        isDollClosing = false;

        Time.timeScale = 1f;

        SubscribeInputEvents();

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (dollPanel != null)
            dollPanel.SetActive(false);
    }

    private void OnDisable()
    {
        UnsubscribeInputEvents();

        if (cancelAction != null && cancelAction.action != null)
            cancelAction.action.performed -= OnCancelPerformed;

        dollTween?.Kill();

        dollTween = null;

        // Do not leave the game paused if this component is disabled.
        if (isPaused || isDolled || isDollClosing)
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
        }

        SetPausedState(false);
        SetDolledState(false);
        isDollAnimating = false;
        isDollClosing = false;

        if (instance == this)
            instance = null;
    }

    private void SubscribeInputEvents()
    {
        if (InputManager.Instance == null)
            return;

        // Remove first to prevent duplicate subscriptions.
        InputManager.Instance.PauseEvent -= TogglePause;
        InputManager.Instance.DollEvent -= ToggleDoll;

        InputManager.Instance.PauseEvent += TogglePause;
        InputManager.Instance.DollEvent += ToggleDoll;
    }

    private void UnsubscribeInputEvents()
    {
        if (InputManager.Instance == null)
            return;

        InputManager.Instance.PauseEvent -= TogglePause;
        InputManager.Instance.DollEvent -= ToggleDoll;
    }

    // --------------------------------------------------
    // BACK / CANCEL
    // --------------------------------------------------

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        // Cancel is for pause-menu navigation only.
        // Gameplay DollEvent remains a separate input path.
        if (!context.performed || !isPaused)
            return;

        if (SettingsPanel != null && SettingsPanel.activeSelf)
        {
            SettingsPanel.SetActive(false);

            if (pauseButtonsPanel != null){
                pauseButtonsPanel.SetActive(true);
                OnSetContinueButton?.Invoke();
            }
            return;
        }

        if (pauseButtonsPanel != null && pauseButtonsPanel.activeSelf)
            Resume();
    }

    // --------------------------------------------------
    // PAUSE MENU
    // --------------------------------------------------

    public void TogglePause()
    {
        if (isDolled || isDollClosing)
            return;

        if (!isPaused)
        {
            SetPausedState(true);
            Time.timeScale = 0f;
            Cursor.visible = true;

            OpenPauseButton();
        }
        else
        {
            Resume();
        }
    }

    public void OpenPauseButton()
    {
        if (!isPaused)
            return;

        if (SettingsPanel != null)
            SettingsPanel.SetActive(false);

        if (pauseButtonsPanel != null)
            pauseButtonsPanel.SetActive(true);

        OnSetContinueButton?.Invoke();

        OpenPausePanel();
    }

    public void OpenSetting()
    {
        if (!isPaused)
            return;

        if (pauseButtonsPanel != null)
            pauseButtonsPanel.SetActive(false);

        if (SettingsPanel != null)
            SettingsPanel.SetActive(true);

        OnSetMasterSlider?.Invoke();
        OpenPausePanel();
    }

    private void OpenPausePanel()
    {
        if (pausePanel == null)
            return;

        if (!pausePanel.activeSelf)
        {
            pausePanel.SetActive(true);
            PlaySound(ClipOnOpen);
        }
    }

    public void Resume()
    {
        if (!isPaused)
            return;

        if (pausePanel != null && pausePanel.activeSelf)
        {
            PlaySound(ClipOnClose);
            pausePanel.SetActive(false);
        }

        FinishPauseClose();
    }

    private void FinishPauseClose()
    {
        SetPausedState(false);

        Time.timeScale = isDolled ? 0f : 1f;
        Cursor.visible = isDolled;
    }

    // --------------------------------------------------
    // DOLL FEEDBACK WINDOW
    // --------------------------------------------------

    public void ToggleDoll()
    {
        // Never let gameplay Doll input interfere with the pause menu.
        if (isPaused || Time.frameCount == pauseClosedFrame)
            return;

        if (isDollAnimating || isDollClosing)
            return;

        if (!isDolled)
        {
            if (dollPanel == null || dollRect == null)
                return;

            SetDolledState(true);
            Time.timeScale = 0f;

            dollPanel.SetActive(true);
            PlaySound(ClipOnOpen);

            isDollAnimating = true;
            PlayOpenAnimation(dollRect, ref dollTween, dollDuration);
        }
        else
        {
            if (dollPanel == null || dollRect == null)
            {
                FinishDollClose();
                return;
            }

            if (!dollPanel.activeSelf)
            {
                FinishDollClose();
                return;
            }

            isDollClosing = true;

            PlaySound(ClipOnClose);

            PlayCloseAnimation(
                dollRect,
                ref dollTween,
                dollPanel,
                closeDuration,
                FinishDollClose
            );
        }
    }

    private void FinishDollClose()
    {
        SetDolledState(false);
        isDollAnimating = false;
        isDollClosing = false;

        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
    }

    private void SetPausedState(bool paused)
    {
        bool wasStopped = IsStopped;
        if (isPaused && !paused)
            pauseClosedFrame = Time.frameCount;

        isPaused = paused;
        NotifyStoppedStateChanged(wasStopped);
    }

    private void SetDolledState(bool dolled)
    {
        bool wasStopped = IsStopped;
        isDolled = dolled;
        NotifyStoppedStateChanged(wasStopped);
    }

    private void NotifyStoppedStateChanged(bool wasStopped)
    {
        if (wasStopped != IsStopped)
            StoppedStateChanged?.Invoke(IsStopped);
    }

    // --------------------------------------------------
    // SHARED UI ANIMATIONS
    // --------------------------------------------------

    private void PlayOpenAnimation(
        RectTransform target,
        ref Tween tween,
        float animDuration)
    {
        if (target == null)
        {
            isDollAnimating = false;
            return;
        }

        tween?.Kill();

        if (target.localScale.x <= startScale * 1.5f ||
            target.localScale.y <= startScale * 1.5f)
        {
            target.localScale = new Vector3(
                startScale,
                startScale,
                1f
            );
        }

        Sequence sequence = DOTween.Sequence()
            .SetUpdate(true);

        sequence.Append(
            target.DOScale(
                new Vector3(stretchX, stretchY, 1f),
                animDuration * 0.7f
            ).SetEase(Ease.OutCubic)
        );

        sequence.Append(
            target.DOScale(Vector3.one, animDuration * 0.3f)
                .SetEase(Ease.OutBack)
        );

        sequence.OnComplete(() =>
        {
            if (target == dollRect)
                isDollAnimating = false;
        });

        tween = sequence;
    }

    private void PlayCloseAnimation(
        RectTransform target,
        ref Tween tween,
        GameObject targetObject,
        float animDuration,
        TweenCallback onComplete = null)
    {
        if (target == null || targetObject == null ||
            !targetObject.activeSelf)
        {
            onComplete?.Invoke();
            return;
        }

        tween?.Kill();

        tween = target.DOScale(
                new Vector3(startScale, startScale, 1f),
                animDuration
            )
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                targetObject.SetActive(false);
                onComplete?.Invoke();
            });
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(clip);
    }
}

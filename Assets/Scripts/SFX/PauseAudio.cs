
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

public class PauseAudio : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string pauseVolumeParameter = "PauseVolume";

    [Header("Pause Audio")]
    [Range(0f, 1f)]
    [SerializeField] private float pauseVolume = 0.3f;

    [SerializeField] private float transitionDuration = 0.25f;

    private float currentVolumeDB;
    private Tween volumeTween;

    private void Awake()
    {
        currentVolumeDB = 0f;

        if (audioMixer != null)
            audioMixer.SetFloat(pauseVolumeParameter, 0f);
    }

    private void OnEnable()
    {
        PauseMenu.StoppedStateChanged += SetPauseVolume;

        if (PauseMenu.instance != null && PauseMenu.instance.IsStopped)
        {
            SetPauseVolume(true);
        }
    }

    private void SetPauseVolume(bool paused)
    {
        if (audioMixer == null)
            return;

        volumeTween?.Kill();

        float targetDB = paused
            ? (pauseVolume > 0.0001f
                ? Mathf.Log10(pauseVolume) * 20f
                : -80f)
            : 0f;

        if (transitionDuration <= 0f)
        {
            currentVolumeDB = targetDB;
            audioMixer.SetFloat(
                pauseVolumeParameter, currentVolumeDB);
            return;
        }

        volumeTween = DOTween.To(
                () => currentVolumeDB,
                value =>
                {
                    currentVolumeDB = value;
                    audioMixer.SetFloat(
                        pauseVolumeParameter, value);
                },
                targetDB,
                transitionDuration)
            .SetUpdate(true);
    }

    private void OnDisable()
    {
        volumeTween?.Kill();
        volumeTween = null;

        if (audioMixer != null)
            audioMixer.SetFloat(pauseVolumeParameter, 0f);

        currentVolumeDB = 0f;
        PauseMenu.StoppedStateChanged -= SetPauseVolume;
    }
}

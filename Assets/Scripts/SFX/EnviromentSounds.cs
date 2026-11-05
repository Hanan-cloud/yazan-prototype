using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class EnviromentSounds : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Tooltip("Name of the EXPOSED Lowpass cutoff parameter in the mixer (must match exactly).")]
    [SerializeField] private string cutoffFreq = "CuttoffFreq";

    [SerializeField] private float defaultFrequency = 22000f;

    [Tooltip("Smoothing time in seconds. Lower values make the filter transition faster.")]
    [SerializeField] private float smoothTime = 0.08f;

    public float DefaultFrequency => defaultFrequency;

    private const float MinHz = 10f;
    private const float MaxHz = 22000f;

    // Frequency is stored and smoothed in log space (ln Hz) so that the
    // sweep sounds even instead of rushing through the low frequencies.
    private float currentLog;
    private float targetLog;
    private float logVelocity;

    [Space(15)]

    [Header("Audio Sources")]
    [SerializeField] private AudioSource tentAreaAudio;
    [SerializeField] private AudioSource camelAreaAudio;
    [SerializeField] private AudioSource ruinsAreaAudio;

    private AudioSource currentAudioSource;

    [Space(15)]

    [Header("Ambient Clips")]
    [SerializeField] private List<AudioClip> tentAreaClips;
    [SerializeField] private List<AudioClip> camelAreaClips;
    [SerializeField] private List<AudioClip> ruinsAreaClips;

    private List<AudioClip> currentClips;

    private float chance;
    private bool inAera;

    private void Start()
    {
        currentLog = targetLog = HzToLog(defaultFrequency);
        ApplyFrequency();

        StartCoroutine(AreaAmbient());
    }

    private void Update()
    {
        UpdateFilterFrequency();
    }

    private void UpdateFilterFrequency()
    {
        currentLog = Mathf.SmoothDamp(
            currentLog,
            targetLog,
            ref logVelocity,
            smoothTime
        );

        ApplyFrequency();
    }

    private void ApplyFrequency()
    {
        if (mixer != null)
        {
            mixer.SetFloat(cutoffFreq, Mathf.Exp(currentLog));
        }
    }

    private static float HzToLog(float hz)
    {
        return Mathf.Log(Mathf.Clamp(hz, MinHz, MaxHz));
    }

    public void SetFilterFrequency(float frequency)
    {
        targetLog = HzToLog(frequency);
    }

    public void ResetFilterFrequency()
    {
        SetFilterFrequency(defaultFrequency);
    }

    IEnumerator AreaAmbient()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(10f, 20f)
            );

            if (Random.Range(0f, 101f) < chance && inAera)
            {
                if (currentAudioSource != null &&
                    currentClips != null &&
                    currentClips.Count > 0)
                {
                    AudioClip clip = currentClips[
                        Random.Range(0, currentClips.Count)
                    ];

                    currentAudioSource.PlayOneShot(clip);
                }
            }
        }
    }

    public void TentAreaParameter()
    {
        inAera = true;
        chance = 50f;

        currentClips = tentAreaClips;
        currentAudioSource = tentAreaAudio;
    }

    public void CamelAreaParameter()
    {
        inAera = true;
        chance = 50f;

        currentClips = camelAreaClips;
        currentAudioSource = camelAreaAudio;
    }

    public void RuinsAreaParameter()
    {
        inAera = true;
        chance = 50f;

        currentClips = ruinsAreaClips;
        currentAudioSource = ruinsAreaAudio;
    }

    public void InAreaOff()
    {
        inAera = false;
    }
}
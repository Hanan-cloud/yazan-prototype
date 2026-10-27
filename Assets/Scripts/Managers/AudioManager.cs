using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider anomalySlider;

    [Header("UI Sound")]
    [SerializeField] private AudioClip sliderSound;

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";
    private const string ANOMALY_KEY = "AnomalyVolume";

    private AudioSource sliderAudioSource;

    private void Awake()
    {
        sliderAudioSource = GetComponent<AudioSource>();
        if (sliderAudioSource == null)
            sliderAudioSource = gameObject.AddComponent<AudioSource>();

        sliderAudioSource.playOnAwake = false;
        sliderAudioSource.spatialBlend = 0f;
    }

    private void Start()
    {
        LoadSettings();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        masterSlider.onValueChanged.AddListener(PlaySliderSound);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        musicSlider.onValueChanged.AddListener(PlaySliderSound);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        sfxSlider.onValueChanged.AddListener(PlaySliderSound);
        anomalySlider.onValueChanged.AddListener(SetAnomalyVolume);
        anomalySlider.onValueChanged.AddListener(PlaySliderSound);
    }

    public void SetMasterVolume(float value) => SetVolume(MASTER_KEY, value);
    public void SetMusicVolume(float value) => SetVolume(MUSIC_KEY, value);
    public void SetSFXVolume(float value) => SetVolume(SFX_KEY, value);
    public void SetAnomalyVolume(float value) => SetVolume(ANOMALY_KEY, value);

    private void PlaySliderSound(float _)
    {
        if (sliderSound != null && !sliderAudioSource.isPlaying)
            sliderAudioSource.PlayOneShot(sliderSound);
    }

    private void SetVolume(string parameterName, float sliderValue)
    {
        float dB = sliderValue > 0.0001f ? Mathf.Log10(sliderValue) * 20f : -80f;

        audioMixer.SetFloat(parameterName, dB);
        PlayerPrefs.SetFloat(parameterName, sliderValue);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float masterVal = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVal = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVal = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float anomalyVal = PlayerPrefs.GetFloat(ANOMALY_KEY, 1f);

        masterSlider.value = masterVal;
        musicSlider.value = musicVal;
        sfxSlider.value = sfxVal;
        anomalySlider.value = anomalyVal;

        SetVolume(MASTER_KEY, masterVal);
        SetVolume(MUSIC_KEY, musicVal);
        SetVolume(SFX_KEY, sfxVal);
        SetVolume(ANOMALY_KEY, anomalyVal);
    }
}
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

    private const string MASTER_KEY = "MasterVolume";
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private void Start()
    {
        LoadSettings();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMasterVolume(float value) => SetVolume(MASTER_KEY, value);
    public void SetMusicVolume(float value) => SetVolume(MUSIC_KEY, value);
    public void SetSFXVolume(float value) => SetVolume(SFX_KEY, value);

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

        masterSlider.value = masterVal;
        musicSlider.value = musicVal;
        sfxSlider.value = sfxVal;

        SetVolume(MASTER_KEY, masterVal);
        SetVolume(MUSIC_KEY, musicVal);
        SetVolume(SFX_KEY, sfxVal);
    }
}
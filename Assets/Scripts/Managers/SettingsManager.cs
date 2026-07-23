using UnityEngine;
using UnityEngine.UI;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using System.Collections.Generic;
using AHAKuo.Signalia.LocalizationStandalone.Framework;
using TMPro;


public class SettingsManager : MonoBehaviour
{
    [Header("Screen Mode")]
    [SerializeField] private Toggle fullscreenToggle; 

    private const string FULLSCREEN_KEY = "FullScreenMode";




    [SerializeField] TextMeshProUGUI screenModeTxt;
    [SerializeField] TextMeshProUGUI languagesTxt;







 
    private void Start()
    {
        LoadSettings();

        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(SetFullScreen);



     
    }

    // ------------------ ??? ?????? ------------------

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ------------------ ????? ------------------

  



    // ------------------ ????? ????????? ???????? ------------------

    private void LoadSettings()
    {
        bool isFullScreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;
        Screen.fullScreen = isFullScreen;

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = isFullScreen;
    }
}
using AHAKuo.Signalia.LocalizationStandalone.Framework;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI modeLabel;
    [SerializeField]  SimpleLocalizedText screenModeTxt;

    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;

    private const string SCREEN_MODE_KEY = "ScreenMode";



    private readonly FullScreenMode[] modes = new FullScreenMode[]
    {
        FullScreenMode.FullScreenWindow, // Borderless
        FullScreenMode.Windowed
    };

    private readonly string[] modeNames = new string[]
    {
        "Fullscreen",
        "Windowed"
    };

    private int currentIndex = 0;

    private void Awake()
    {
        SIGS.InitializeLocalization();

    }

    private void Start()
    {
        SimpleLocalizedText screenModeTxt = GetComponent<SimpleLocalizedText>();

        LoadScreenMode();

        leftArrowButton.onClick.AddListener(SelectPrevious);
        rightArrowButton.onClick.AddListener(SelectNext);
    }

    public void SelectNext()
    {
        currentIndex = (currentIndex + 1) % modes.Length;
        ApplyAndSave();
    }

    public void SelectPrevious()
    {
        currentIndex = (currentIndex - 1 + modes.Length) % modes.Length;
        ApplyAndSave();
    }

    private void ApplyAndSave()
    {
        Screen.fullScreenMode = modes[currentIndex];
        //modeLabel.text = modeNames[currentIndex];

        screenModeTxt.SetKey(modeNames[currentIndex]);


        PlayerPrefs.SetInt(SCREEN_MODE_KEY, currentIndex);
        PlayerPrefs.Save();
    }

    private void LoadScreenMode()
    {
        currentIndex = PlayerPrefs.GetInt(SCREEN_MODE_KEY, 0);

        Screen.fullScreenMode = modes[currentIndex];
        //modeLabel.text = modeNames[currentIndex];

        screenModeTxt.SetKey(modeNames[currentIndex]);


    }
}
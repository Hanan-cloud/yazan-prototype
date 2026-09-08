using System;
using UnityEngine;
using UnityEngine.Events;


public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButtonsPanel;
    [SerializeField] private GameObject dollPanel;
    [SerializeField] private GameObject SettingsPanel;

    private bool isPaused = false;
    private bool isDolled = false;



    [SerializeField] UnityEvent OnSetContinueButton;
    [SerializeField] UnityEvent OnSetMasterSlider;

    public bool IsStopped { get => isPaused || isDolled; }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(instance);
        }
    }

    private void Start()


    {
        isPaused = false;
        isDolled = false;

        InputManager.Instance.PauseEvent += TogglePause;
        InputManager.Instance.DollEvent += ToggleDoll;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }


    private void OnDisable()
    {
        InputManager.Instance.PauseEvent -= TogglePause;
        InputManager.Instance.DollEvent -= ToggleDoll;


    }

    public void TogglePause()
    {
        Paused();

        
    }

   public void ToggleDoll()
    {
        if (isPaused) return;

        if (isDolled == false)
        {


            isDolled = true;
   

            Time.timeScale = 0f;

        }
        else
        {
            isDolled = false;


            Time.timeScale = 1f;



        }
        dollPanel.SetActive(isDolled);


    }


    private void Paused()
    {
        if (isPaused == false)
        {


            isPaused = true;
            Time.timeScale = 0f;
            OpenPauseButton();

        }
        else 
        {

            Resume();


        }
       


    }


    public void OpenSetting()
    {

        SettingsPanel.SetActive(true);
        pausePanel.SetActive(true);
        pauseButtonsPanel.SetActive(false);
        OnSetMasterSlider?.Invoke();


    }
    public void OpenPauseButton()
    {

        SettingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        pauseButtonsPanel.SetActive(true);
        OnSetContinueButton?.Invoke();



    }



    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
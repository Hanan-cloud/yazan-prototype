using System;
using UnityEngine;
using UnityEngine.Events;


public class PauseMenu : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject dollPanel;

    private bool isPaused = false;

    public static event Action<bool> OnPauseChanged;

    [SerializeField] UnityEvent OnPause;
    private void Start()
    {

        InputManager.Instance.PauseEvent += TogglePause;
        InputManager.Instance.DollEvent += ToggleDoll;
        isPaused = false;
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

        OnPause?.Invoke();
        pausePanel.SetActive(isPaused);
    }

   public void ToggleDoll()
    {
        Paused();
        dollPanel.SetActive(isPaused);
    }


    private void Paused()
    {
        isPaused = !isPaused;
        OnPauseChanged?.Invoke(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
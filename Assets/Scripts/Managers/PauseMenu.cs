using System;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;

    public static event Action<bool> OnPauseChanged;
    private void Start()
    {

        InputManager.Instance.PauseEvent += TogglePause;
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }


    private void OnDisable()
    {
        InputManager.Instance.PauseEvent -= TogglePause;

    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
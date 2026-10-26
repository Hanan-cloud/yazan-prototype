
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Main Menu Pages")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private UnityEvent onCancelEvent;

    [Header("Input")]
    [SerializeField] private InputActionReference cancelAction;

    private void OnEnable()
    {
        if (cancelAction != null && cancelAction.action != null)
        {
            cancelAction.action.performed += OnCancelPerformed;
        }

                Debug.Log($"Cancel enabled: {cancelAction.action.enabled}");

    }

    private void OnDisable()
    {
        if (cancelAction != null && cancelAction.action != null)
        {
            cancelAction.action.performed -= OnCancelPerformed;
        }
    }

    private void OnCancelPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

            onCancelEvent?.Invoke();
        // Credits has priority if multiple pages are active.
        if (creditsPanel != null && creditsPanel.activeSelf)
        {
            creditsPanel.SetActive(false);

            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);

            return;
        }

        if (settingsPanel != null && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);

            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
        }
    }
}

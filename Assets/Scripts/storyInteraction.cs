
using UnityEngine;

public class storyInteraction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject interactionButton;
    [SerializeField] private GameObject storyCanvas;

    [Header("Lore References")]
    [SerializeField] private LoreManager loreManager;
    [SerializeField] private InGameStoryPanel storyPanel;

    private Transform playerPos;
    private Vector3 lastPos;

    private bool isInRange;
    private bool isTextOpen;

    private int openedLoreIndex;

    private AudioSource audioSource;

    private void Start()
    {
        isInRange = false;
        isTextOpen = false;

        audioSource = GetComponent<AudioSource>();

        interactionButton.SetActive(false);
        storyCanvas.SetActive(false);

        if (InputManager.Instance != null)
        {
            InputManager.Instance.InteractionEvent += ShowStory;
        }
        else
        {
            Debug.LogError(
                "storyInteraction: InputManager.Instance is null.",
                this
            );
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.InteractionEvent -= ShowStory;
        }
    }

    private void Update()
    {
        if (!isTextOpen)
        {
            interactionButton.SetActive(
                isInRange &&
                playerPos != null &&
                CanInteract()
            );

            return;
        }

        if (playerPos == null)
        {
            CloseStory();
            return;
        }

        float distanceMoved = Vector3.Distance(
            playerPos.position,
            lastPos
        );

        if (distanceMoved > 0.5f)
        {
            CloseStory();
        }
    }

    private bool CanInteract()
    {
        return loreManager != null &&
               storyPanel != null &&
               loreManager.CanInteractWithLore(storyPanel.Index);
    }

    private void ShowStory()
    {
        if (PauseMenu.instance != null &&
            PauseMenu.instance.IsStopped)
        {
            return;
        }

        if (!isInRange || playerPos == null)
            return;

        if (!isTextOpen)
        {
            if (!CanInteract())
                return;

            isTextOpen = true;

            // Store the exact lore index opened by the player.
            openedLoreIndex = storyPanel.Index;

            storyCanvas.SetActive(true);
            interactionButton.SetActive(false);

            lastPos = playerPos.position;
        }
        else
        {
            CloseStory();
        }
    }

    private void CloseStory()
    {
        if (isTextOpen)
        {
            // Save the lore that was actually opened,
            // even if the current story index has changed.
            if (loreManager != null && openedLoreIndex > 0)
            {
                loreManager.MarkLoreAsRead(openedLoreIndex);
            }

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }

        isTextOpen = false;

        storyCanvas.SetActive(false);

        interactionButton.SetActive(
            isInRange &&
            playerPos != null &&
            CanInteract()
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        isInRange = true;
        playerPos = collision.transform;
        lastPos = playerPos.position;

        interactionButton.SetActive(CanInteract());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        isInRange = false;

        CloseStory();

        playerPos = null;

        interactionButton.SetActive(false);
    }
}

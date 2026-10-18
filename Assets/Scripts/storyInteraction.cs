
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

    AudioSource audioSource;

    private void Start()
    {
        isInRange = false;
        isTextOpen = false;
        audioSource=GetComponent<AudioSource>();
        interactionButton.SetActive(false);
        storyCanvas.SetActive(false);

        if (InputManager.Instance != null)
        {
            InputManager.Instance.InteractionEvent += ShowStory;
        }
        else
        {
            Debug.LogError("storyInteraction: InputManager.Instance is null.", this);
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.InteractionEvent -= ShowStory;
        }
    }

    private void ShowStory()
    {
        if (PauseMenu.instance != null &&
            PauseMenu.instance.IsStopped)
            return;

        if (!isInRange || playerPos == null)
            return;

        if (!isTextOpen)
        {
            isTextOpen = true;
            storyCanvas.SetActive(true);
            interactionButton.SetActive(false);

            lastPos = playerPos.position;
        }
        else
        {
            // Save the lore as read when the player closes the story.
            if (loreManager != null && storyPanel != null)
            {
                loreManager.MarkLoreAsRead(storyPanel.Index);
            }

            CloseStory();
        }
    }

    private void Update()
    {
        if (!isTextOpen || playerPos == null)
            return;

        float distanceMoved =
            Vector3.Distance(playerPos.position, lastPos);

        if (distanceMoved > 0.5f)
        {
            CloseStory();
        }
    }

    private void CloseStory()
    {    
        
    if (isTextOpen && audioSource != null )
    {
        audioSource.Play();
    }

        isTextOpen = false;

        storyCanvas.SetActive(false);

        interactionButton.SetActive(
            isInRange && playerPos != null
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        isInRange = true;
        playerPos = collision.transform;
        lastPos = playerPos.position;

        interactionButton.SetActive(true);
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

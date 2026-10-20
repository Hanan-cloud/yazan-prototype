using UnityEngine;

public class GateKeeperInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;
    [SerializeField] private GameObject storyCanvas;    
      private Transform playerPos;
    private Vector3 lastPos;
    private bool isTextOpen;

    private bool isInRange;
    void Start()
    {
        isInRange = false;
        isTextOpen = false;


        interactionButton.SetActive(false);
        storyCanvas.SetActive(false);

        if (InputManager.Instance != null)
        {
           // InputManager.Instance.InteractionEvent += ShowStory;
        }
        else
        {
            Debug.LogError(
                "storyInteraction: InputManager.Instance is null.",
                this
            );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

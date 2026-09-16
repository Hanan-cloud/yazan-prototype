using System;
using TMPro;
using UnityEngine;

public class storyInteraction : MonoBehaviour
{


    [SerializeField] GameObject interactionButton;
    [SerializeField] GameObject storyCanvas;


    Transform playerPos;
    Vector3 lastPos;

    bool isInRange;
    bool isTextOpen=false;


    private void Start()
    {
        isInRange = false;
        InputManager.Instance.InteractionEvent += ShowStory;
        interactionButton.SetActive(false);
    }

    private void OnDisable()
    {
        InputManager.Instance.InteractionEvent -= ShowStory;

    }

    void ShowStory()
    {
        if(PauseMenu.instance.IsStopped) return;

        if (isInRange) {

            if (isTextOpen==false)
            {
                isTextOpen = true;
                storyCanvas.SetActive(true);
                interactionButton.SetActive(false);
                lastPos = playerPos.position;

            }
            else
            {

                isTextOpen = false;
                storyCanvas.SetActive(false);
                interactionButton.SetActive(true);
                lastPos = playerPos.position;


            }




        }


    }


    private void Update()
    {

        if (playerPos != null)
        {
           // Debug.Log(isInRange);


            float distanceMoved = Vector3.Distance(playerPos.position, lastPos);
            if (distanceMoved > 0.5f)
            {
                HideText();
                distanceMoved = 0f;
            }

        }
    }

    private void HideText()
    {
        storyCanvas.SetActive(false);
        interactionButton.SetActive(true);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactionButton.SetActive(true);
            playerPos = collision.transform;
            lastPos = collision.transform.position;


        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionButton.SetActive(false);

            isInRange = false;
            playerPos = null;


        }
    }


}

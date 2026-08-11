using System;
using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    [SerializeField] GameObject RunStarterLeft;
    [SerializeField] GameObject RunStarterRight;

    public static Action OnNailsFalls;
    public static Action OnNailsReset;

    bool playerInTrigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(playerInTrigger== true)  return;
            playerInTrigger = true;
           
            {

                
                // I Only care if the player dir = coorect dir
                  if(PlayerController.Instance.PlayerCurrentDir == RunManager.Instance.CorrectDirection)
                {

                    // Debug.Log("n-1");
                    AnomallyManager.Instance.SaveFoundAnomaly();
                    GameManager.Instance.Nails -= 1;
                    OnNailsFalls?.Invoke();
                    // n-1 
                    //progress

                }else
                {
                    // reset
                    //n=10
                   // Debug.Log("n=10");
                    GameManager.Instance.Nails =10;
                    OnNailsReset?.Invoke();



                }

            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger= false;
            // Set Anomaly proability
            AnomallyManager.Instance.SetAnomalyProbability();
            RunStarterLeft.SetActive(true);
            RunStarterRight.SetActive(true);

            this.gameObject.SetActive(false);


        }
    }

}

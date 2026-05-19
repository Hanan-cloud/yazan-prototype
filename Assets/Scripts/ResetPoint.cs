using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    [SerializeField] GameObject RunStarterLeft;
    [SerializeField] GameObject RunStarterRight;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            {

                // I Only care if the player dir = coorect dir
                  if(PlayerController.Instance.PlayerCurrentDir == RunManager.Instance.CorrectDirection)
                {

                    Debug.Log("n-1");
                    GameManager.Instance.Nails -= 1;
                    // n-1 
                    //progress

                }else
                {
                    // reset
                    //n=10
                    Debug.Log("n=10");
                    GameManager.Instance.Nails =10;


                }

            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            // Set Anomaly proability
            AnomallyManager.Instance.SetAnomalyProbability();
            RunStarterLeft.SetActive(true);
            RunStarterRight.SetActive(true);

            this.gameObject.SetActive(false);


        }
    }

}

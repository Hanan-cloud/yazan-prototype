using UnityEngine;

public class SetNewRun : MonoBehaviour
{

    [SerializeField] Directions runDir;

    [SerializeField] GameObject otherRunStarter;
    [SerializeField] GameObject ResetPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

          
            RunManager.Instance.SetRunDir(runDir);

            if (AnomallyManager.Instance.IsAnomalyRun)
                RunManager.Instance.SetCorrectDir(CalculateDir());
            else
            {
                RunManager.Instance.SetCorrectDir(runDir);

            }

            gameObject.SetActive(false); 
            otherRunStarter.SetActive(false);
            ResetPoint.SetActive(true);

        }
    }



    private Directions CalculateDir()
    {

        if(runDir == Directions.Right)
        {
            return Directions.Left;
        }
        else
        {
            return Directions.Right;
        }



    }

}

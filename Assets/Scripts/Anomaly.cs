using UnityEngine;

public class Anomaly : MonoBehaviour
{

    [SerializeField] GameObject normal;
    [SerializeField] GameObject abnormal;




    public void SetAnomaly()
    {

        normal.SetActive(false);
        abnormal.SetActive(true);

    }
    
    
    public void ResetAnomaly()
    {

        normal.SetActive(true);
        abnormal.SetActive(false);

    }






}

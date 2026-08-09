using UnityEngine;

public class MirrorRefAnomaly : MonoBehaviour, IAnomaly
{
    public void ResetAnomaly()
    {
        gameObject.SetActive(true);

    }

    public void SetAnomaly()
    {
        gameObject.SetActive(false);
    }

   

  
}

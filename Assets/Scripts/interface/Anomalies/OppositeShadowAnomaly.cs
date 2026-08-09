using UnityEngine;

public class OppositeShadowAnomaly : MonoBehaviour, IAnomaly
{

    [SerializeField] GameObject yazanClone;
    public void ResetAnomaly()
    {
        yazanClone.SetActive(false);

    }


    [ContextMenu("startAnom")]
    public void SetAnomaly()
    {
        yazanClone.SetActive(true);
    }


 

}

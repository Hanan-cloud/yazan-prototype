using UnityEngine;
[RequireComponent(typeof(AnomalyNameSetter))]
public class MirrorRefAnomaly : AnomalyBase
{

    [SerializeField] GameObject mirrorRef;

    private void Start()
    {
        SetAnomalyName();
    }

    public  override void ResetAnomaly()
    {
        mirrorRef.SetActive(true);

    }

    public override void SetAnomaly()
    {
        mirrorRef.SetActive(false);
    }


   

}

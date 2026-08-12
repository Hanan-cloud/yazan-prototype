using UnityEngine;

public class ReplaceAnomaly : AnomalyBase
{


    [SerializeField] GameObject originalObject;
    [SerializeField] GameObject anomalyObject;

    private void Start()
    {
        anomalyObject.SetActive(false);

        SetAnomalyName();
    }
    public override void SetAnomaly()
    {
        anomalyObject.SetActive(true);
        originalObject.SetActive(false);

    }


    public override void ResetAnomaly()
    {
        anomalyObject.SetActive(false);

        originalObject.SetActive(true);

    }


}

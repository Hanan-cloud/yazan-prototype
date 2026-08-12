using UnityEngine;

public class PopUpAnomaly : AnomalyBase
{
    [SerializeField] GameObject anomaly;

    private void Start()
    {
        anomaly.SetActive(false);

        SetAnomalyName();
    }
    public override void SetAnomaly()
    {

        anomaly.SetActive(true);

    }


    public override void ResetAnomaly()
    {

        anomaly.SetActive(false);

    }

}

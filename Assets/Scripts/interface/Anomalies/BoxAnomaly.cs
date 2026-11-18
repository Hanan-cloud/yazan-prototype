using UnityEngine;

public class BoxAnomaly : AnomalyBase
{


    [SerializeField] Animator animator;
    void Start()
    {
        SetAnomalyName();
    }

    public override void ResetAnomaly()
    {
        
        animator.SetBool("isOpen", false);
    }

    public override void SetAnomaly()
    {
        animator.SetBool("isOpen", true);
    }


}

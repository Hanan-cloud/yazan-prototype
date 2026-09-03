using UnityEngine;
using UnityEngine.UI;

public class DollAnomaly : AnomalyBase
{

    [SerializeField] GameObject doll;
    Image dollImg;
    [SerializeField] Sprite normalDoll;
    Animator animator;
    public override void ResetAnomaly()
    {
        animator.enabled = false;
        dollImg.sprite = normalDoll;

    }

    public override void SetAnomaly()
    {
        animator.enabled = true;

    }

    void Start()
    {
        dollImg = doll.GetComponent<Image>();
        animator = doll.GetComponent<Animator>();
    }

}

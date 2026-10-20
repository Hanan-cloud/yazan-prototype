using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class StoryInput : MonoBehaviour
{
 public static StoryInput instance;

    float holdTimer = 0;
    float holdDuration = 1; 
    private bool isSkip;
    private bool isSkipped=false;

    float timer =3;
    [SerializeField] Image load;

    [SerializeField] CanvasGroup nextPanelCanvasGroup;


   
    [SerializeField] CutsceneManager controller;
    private bool restoreNextPanelAlpha;

    private void Awake()
    {
        if(instance == null)
            instance = this;


    }
    private void Start()
    {
        isSkipped=false;
        timer =4;
        restoreNextPanelAlpha = true;
        InputManager.Instance.NextEvent += Next;
        InputManager.Instance.SkipEvent += SkipOn;
        InputManager.Instance.SkipEventCancel += SkipOff;

    }


    private void OnDisable()
    {
        InputManager.Instance.NextEvent -= Next;
        InputManager.Instance.SkipEvent -= SkipOn;
        InputManager.Instance.SkipEventCancel -= SkipOff;

  
    }
    private void Next()
    {
        if(timer>0 ) return;
        controller.OnNextPressed();
        timer=3f;
        restoreNextPanelAlpha = true;
        // Hide Next during its cooldown, then restore it when the timer expires.
        FadeNextPanel(0f);


    }
    private void SkipOn()
    {

        isSkip = true;
    }


    private void SkipOff()
    {

        isSkip = false;
    }


    private void Update()
    {

        if (isSkip)
        {
            holdTimer += Time.deltaTime;
            load.fillAmount = holdTimer / holdDuration;
            if (holdTimer > holdDuration && isSkipped == false)
            {
               isSkip=true;
               StartCoroutine(controller.EndCutscene());
            }
        }
        else 
        {
            holdTimer = 0;
            load.fillAmount = 0;

        }

        // Debug.Log(isSkip);


        if (timer > 0)
        {
            timer-= Time.deltaTime ;
            if (timer <= 0 && restoreNextPanelAlpha)
            {
                restoreNextPanelAlpha = false;
                FadeNextPanel(1f);
            }
        }

    }


    private void FadeNextPanel(float alpha)
    {
        if (nextPanelCanvasGroup == null)
        {
            Debug.LogError("Next Panel CanvasGroup is not assigned.", this);
            return;
        }

        nextPanelCanvasGroup.DOKill();
        nextPanelCanvasGroup.DOFade(alpha, 0.3f);
    }

}

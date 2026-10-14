using UnityEngine;
using UnityEngine.UI;



public class StoryInput : MonoBehaviour
{
 public static StoryInput instance;

    float holdTimer = 0;
    float holdDuration = 1; 
    private bool isSkip;
    private bool isSkipped=false;

    float timer =2;
    [SerializeField] Image load;


    [SerializeField] GameObject NextPanel;
    [SerializeField] GameObject SkipPanel;
   
    [SerializeField] CutsceneManager controller;
    [SerializeField] SceneController sceneController;
    private void Awake()
    {
        if(instance == null)
            instance = this;


        SetNextPanel(true);
    }
    private void Start()
    {
        isSkipped=false;
        timer =2;
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
        timer=0.5f;


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
        }

    }




    public void SetNextPanel(bool b)
    {

        NextPanel.SetActive(b);
        SkipPanel.SetActive(b);

    }

}

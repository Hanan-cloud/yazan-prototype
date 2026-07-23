using UnityEngine;
using UnityEngine.UI;

public class StorySceneInput : MonoBehaviour
{

    public static StorySceneInput instance;

    float holdTimer = 0;
    float holdDuration = 1; 
    private bool isSkip;
    [SerializeField] Image load;


    [SerializeField] GameObject NextPanel;
    [SerializeField] GameObject SkipPanel;
    [SerializeField] GameObject controllerObj;
    [SerializeField]  PanelsController controller;

    private void Awake()
    {
        if(instance == null)
            instance = this;


        SetNextPanel(false);
        //controller = GetComponent<PanelsController>();
    }
    private void Start()
    {
        InputManager.Instance.NextEvent += Next;
        InputManager.Instance.SkipEvent += SkipOn;
        InputManager.Instance.SkipEventCancel += SkipOff;

        controller.OnNextAvailabilityChanged += SetNextPanel;
    }


    private void OnDisable()
    {
        InputManager.Instance.NextEvent -= Next;
        InputManager.Instance.SkipEvent -= SkipOn;
        InputManager.Instance.SkipEventCancel -= SkipOff;

        controller.OnNextAvailabilityChanged -= SetNextPanel;
    }
    private void Next()
    {
        if(controller.CanNext ==false) return;
        controller.Next();
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
            if (holdTimer > holdDuration)
            {
                // GameManager.instanance.FadeInToLevel1();
            }
        }
        else
        {
            holdTimer = 0;
            load.fillAmount = 0;

        }

        Debug.Log(isSkip);

    }




    public void SetNextPanel(bool b)
    {

        NextPanel.SetActive(b);

    }


}

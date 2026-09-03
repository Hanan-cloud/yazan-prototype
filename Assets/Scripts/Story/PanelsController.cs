using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using System;
using UnityEngine.UI;
using TMPro;
using AHAKuo.Signalia.LocalizationStandalone.Internal;


public class PanelsController : MonoBehaviour
{

    [SerializeField] List<PanelsData> panels;
    //[SerializeField] List<RectTransform> Poses;
    [SerializeField] String textKey = "Panel_";

    [SerializeField] Transform target;
    Vector3 currentCameraPos = Vector3.zero;
    Vector3 originalFocusPoint = Vector3.zero;

    float currentCameraZoom = 3f;
    float originalCameraZoom = 4.13f;
    float zoomTransitionTime = 1;

    int Panel_index = 0;
    int Shot_Index = 0;
    int textIndex = 0;

    [SerializeField] Image textBg;
    [SerializeField] TextMeshProUGUI text;
    SimpleLocalizedText simpleText;
    bool canNext;


    public event Action<bool> OnNextAvailabilityChanged;
    public bool CanNext { get => canNext; }


    [SerializeField] CinemachineCamera cam;

    



    void StartPanel()
    {

        //  == step1: go to position while preparing the simple text ==
        if (Panel_index == panels.Count) return;

        Debug.Log("Panel Index: "+ Panel_index);
        Shot_Index = 0;

        if (panels[Panel_index].shots[Shot_Index].textBg != null)
        {
            simpleText = panels[Panel_index].shots[Shot_Index].text.gameObject.GetComponent<SimpleLocalizedText>();
            text = panels[Panel_index].shots[Shot_Index].text;
            textBg = panels[Panel_index].shots[Shot_Index].textBg;
        }

        panels[Panel_index].sprite.gameObject.GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() => SetShotCamera());





    }
    private void Start()
    {
       // simpleText = text.gameObject.GetComponent<SimpleLocalizedText>();

       // posesIndex = 0;
        Panel_index = 0;
        textIndex = 1;
        canNext = false;


        StartPanel();





        //Debug.Log(panels[Panel_index].shots.Count);
    }


    public void ShowNextPanel()
    {

        Panel_index++;

    }

    private void SetShotCamera()
    {
        //  == step2: Go throu shots ==
        SetTextOff();
        
        // get focus position 
        currentCameraPos = panels[Panel_index].shots[Shot_Index].transitionPoint.position;


        // move camera to current shot pos
        target.DOMove(currentCameraPos, 1);
        currentCameraZoom = panels[Panel_index].shots[Shot_Index].zoom;


        // Do orthognal  and give player permition to next or skip
        DOTween.To(
            () => cam.Lens.OrthographicSize,
            x => cam.Lens.OrthographicSize = x,
            currentCameraZoom,
            zoomTransitionTime
        ).SetEase(Ease.Linear).OnComplete(() => { canNext = true;
            OnNextAvailabilityChanged?.Invoke(canNext);
            SetShotText();
        });
    }

    private void SetTextOff()
    {

        text.DOFade(0, 0);
        textBg.DOFade(0, 0);



    }

    private void SetShotText()
    {
        //  == Step3: Set Text ==

        // if(Panel_index>= Poses.Count ) return;

        if (panels[Panel_index].shots[Shot_Index].textBg != null)
        {

            simpleText.SetKey(((textKey + textIndex).ToString())); ;


            textBg.gameObject.transform.position = panels[Panel_index].shots[Shot_Index].textPos.position;
            text.DOFade(1, 0.5f);
            textBg.DOFade(1f, 0.5f);
        }




       
        textIndex++;


        // Loop Stops untill player press next
    }
    public void Next()
    {

        //  == step4: player press next == 

        // player can't control
        canNext = false;
        OnNextAvailabilityChanged?.Invoke(canNext);
        StorySceneInput.instance.SetNextPanel(false);


        if (Panel_index >= panels.Count) {
            Debug.LogWarning("RETURN");

            return;
        }
        Debug.LogWarning("after RETURN");



        // check if there is other shots
        Shot_Index++;

        if (Shot_Index >= panels[Panel_index].shots.Count)
        {
            // if no Shot
           SetTextOff();

            target.DOMove(originalFocusPoint, 1);


            DOTween.To(
            () => cam.Lens.OrthographicSize,
            x => cam.Lens.OrthographicSize = x,
            originalCameraZoom,
            1
            ).SetEase(Ease.Linear);

            panels[Panel_index].sprite.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() => StartPanel());



            Shot_Index = 0;
            Panel_index++;


        }
        else
        {

            // next shot
            SetShotCamera();
        }

    }


 



    //GUIStyle style;


    //void OnGUI()
    //{

    //    style = new GUIStyle();

    //    style.fontSize = 28;
    //    style.fontStyle = FontStyle.Bold;

    //    style.normal.textColor = Color.white;

    //    style.alignment = TextAnchor.UpperLeft;
    //    GUI.Label(
    //        new Rect(20, 20, 500, 50),
    //        $"index : {index}",
    //        style);
        
        
        
        
    //    GUI.Label(
    //        new Rect(20, 20*2, 500, 50),
    //        $"focusPointsIndex : {focusPointsIndex}",
    //        style);
    //}


    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}


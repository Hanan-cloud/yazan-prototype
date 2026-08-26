using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Cinemachine;
using System;
using UnityEngine.UI;
using TMPro;
using AHAKuo.Signalia.LocalizationStandalone.Internal;
using Unity.VisualScripting;


public class PanelsController : MonoBehaviour
{

    [SerializeField] List<PanelsData> panels;
    [SerializeField] List<RectTransform> Poses;
    [SerializeField] String textKey = "Panel_";
    [SerializeField] Transform target;
    Vector3 currentFocusPoint = Vector3.zero;
    Vector3 originalFocusPoint = Vector3.zero;
    float currentCameraZoom = 3f;
    float originalCameraZoom = 4.13f;
    float zoomTransitionTime = 1;

    int index = 0;
    int posesIndex = 0;
    int textIndex = 0;
    int focusPointsIndex = 0;

    [SerializeField] Image textBg;
    [SerializeField] TextMeshProUGUI text;
    SimpleLocalizedText simpleText;
    bool canNext;


    public event Action<bool> OnNextAvailabilityChanged;
    public bool CanNext { get => canNext; }


    [SerializeField] CinemachineCamera cam;

    
    private void GetAllPoints()
    {



        StartPanel();

    }


    void StartPanel()
    {
        if (index == panels.Count) return;

        Debug.LogWarning("Index: "+ index);
        panels[index].sprite.gameObject.GetComponent<SpriteRenderer>().DOFade(1, 1).OnComplete(() => MoveToPoint());
    }
    private void Start()
    {
        simpleText = text.gameObject.GetComponent<SimpleLocalizedText>();

        posesIndex = 0;
        index = 0;
        textIndex = 1;
        canNext = false;
        GetAllPoints();
        textBg.DOFade(0,0);
        text.DOFade(0,0);   
        Debug.Log(panels[index].shots.Count);
    }


    public void ShowNextPanel()
    {

        index++;

    }

    private void MoveToPoint()
    {
        SetTextOff();
        currentFocusPoint = panels[index].shots[focusPointsIndex].transitionPoint.position;
        target.DOMove(currentFocusPoint, 1);
       

        DOTween.To(
            () => cam.Lens.OrthographicSize,
            x => cam.Lens.OrthographicSize = x,
            currentCameraZoom,
            zoomTransitionTime
        ).SetEase(Ease.Linear).OnComplete(() => { canNext = true;
            OnNextAvailabilityChanged?.Invoke(canNext);
            SetTextOn();
        });
    }

    private void SetTextOff()
    {

        text.DOFade(0, 0);
        textBg.DOFade(0, 0);



    }

    private void SetTextOn()
    {


        if(index>= Poses.Count ) return;

        simpleText.SetKey(((textKey + textIndex).ToString())); ;


        textBg.gameObject.transform.position = Poses[posesIndex].position;
        text.DOFade(1, 0.5f);
        textBg.DOFade(0.4f, 0.5f);
        posesIndex++;
        textIndex++;

    }
    public void Next()
    {

        canNext = false;
        OnNextAvailabilityChanged?.Invoke(canNext);
        StorySceneInput.instance.SetNextPanel(false);
        if (index >= panels.Count) {
            Debug.LogWarning("RETURN");

            return;
        }
        Debug.LogWarning("after RETURN");

        focusPointsIndex++;




        if(focusPointsIndex >= panels[index].shots.Count)
        {

           SetTextOff();

            target.DOMove(originalFocusPoint, 1);


            DOTween.To(
            () => cam.Lens.OrthographicSize,
            x => cam.Lens.OrthographicSize = x,
            originalCameraZoom,
            1
            ).SetEase(Ease.Linear);

            panels[index].sprite.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() => StartPanel());



            focusPointsIndex = 0;
            index++;


        }
        else
        {
            //Debug.LogWarning("calling move to point function");


            MoveToPoint();
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


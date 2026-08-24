using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : AnomalyBase
{

    [SerializeField] GameObject soundObject;
    AudioSource soundSource;

    [SerializeField] List<Transform> rocks;

    float currentX;
    Vector3 jumpEndValue =  new Vector3(0,1,0);

    Tween tween;
    private void Start()
    {
        soundSource = soundObject.GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        currentX = soundObject.transform.localScale.x;
    }

    public override void ResetAnomaly()
    {
        soundObject.SetActive(false);


        soundObject.transform.DOMoveX(currentX, 0);

        tween.Kill();




    }


    [ContextMenu("footsetp sound anomaly")]
    public override void SetAnomaly()
    {
        soundObject.SetActive(true);
        soundSource.Play();

        tween = soundObject.transform.DOLocalMoveX(currentX - 2, 8).SetLoops(-1, LoopType.Yoyo);


        //for (int i = 0; i < rocks.Count; i++) { rocks[i].DOLocalJump(rocks[i].localPosition + jumpEndValue, 1, 1, 0.5f); }


    }


}

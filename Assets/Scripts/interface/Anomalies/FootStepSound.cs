using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class FootStepSound : AnomalyBase
{

    [SerializeField] GameObject soundObject;
    AudioSource soundSource;

    [SerializeField] List<AudioClip> footstep;

    [SerializeField] List<Transform> rocks;

    float currentX;
    Vector3 jumpEndValue =  new Vector3(0,1,0);
    [SerializeField] float shakeStrength;
    [SerializeField] float shakeDuration;

    Tween tween;

    public static Action<float> OnMonsterStep;
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
       // soundSource.Play();

        tween = soundObject.transform.DOLocalMoveX(currentX - 1.5f, 15).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);



        StartCoroutine(rockShake());
    }


    IEnumerator rockShake()
    {
        while (true)
        {


            for (int i = 0; i < footstep.Count; i++)
            {

                soundSource.PlayOneShot(footstep[i]);
                yield return new WaitForSeconds(0.2f);

                OnMonsterStep?.Invoke(transform.localPosition.x);
                //for (int j = 0;j < rocks.Count; j++)
                //{
    

                //    rocks[j].DOShakeRotation(shakeDuration, shakeStrength);
                //}
                yield return new WaitForSeconds(1.3f);

            }


        }
    }


}

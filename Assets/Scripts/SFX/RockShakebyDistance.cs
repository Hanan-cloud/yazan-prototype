using DG.Tweening;
using UnityEngine;

public class RockShakebyDistance : MonoBehaviour
{

    Tween tween;    

    private void Start()
    {

        FootStepSound.OnMonsterStep += ShakeRock;

    }



    void ShakeRock(float x)
    {

        float distX = Mathf.Abs( transform.position.x- x);
       
        float maxDistance = 5f;

        float t = Mathf.InverseLerp(maxDistance, 0f, distX);

        float value = Mathf.Lerp(5f, 25f, t);

       

        tween.Kill();
       tween = transform.DOShakeRotation(0.5f, value);



    }
    private void OnDisable()
    {
        FootStepSound.OnMonsterStep -= ShakeRock;
    }


}

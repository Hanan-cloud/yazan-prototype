using DG.Tweening;
using UnityEngine;

public class RockShakebyDistance : MonoBehaviour
{

    

    private void Start()
    {

        FootStepSound.OnMonsterStep += ShakeRock;

    }



    void ShakeRock(float x)
    {

        float distX = Mathf.Abs( transform.localPosition.x- x);
        Debug.Log("===== distance: "+distX);

        float maxDistance = 5f;

        float t = Mathf.InverseLerp(maxDistance, 0f, distX);

        float value = Mathf.Lerp(1f, 10f, t);

        Debug.Log(value);


        transform.DOShakeRotation(0.3f, value);



    }



}

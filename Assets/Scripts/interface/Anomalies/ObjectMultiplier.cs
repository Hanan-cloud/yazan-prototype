using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMultiplier : AnomalyBase
{

     [SerializeField] List<GameObject> items;
     [SerializeField] float timeBetweenRespawn;


    void Start()
    {
        SetAnomalyName();
    }

    public override void ResetAnomaly()
    {

         for(int i = 0; i< items.Count; i++)
        {
            items[i].SetActive(false);
        }

        StopCoroutine(MultyplyItem());

    }

    public override void SetAnomaly()
    {
        items.Shuffle();
        StartCoroutine(MultyplyItem());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    IEnumerator MultyplyItem()
    {
        
        for(int i = 0; i< items.Count; i++)
        {
            yield return new WaitForSeconds(timeBetweenRespawn);
            items[i].SetActive(true);
        }


    }


}

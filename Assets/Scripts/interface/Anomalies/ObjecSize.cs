using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
public class ObjecSize :AnomalyBase
{

    [SerializeField] List<Transform> objects = new();
    List<Vector3> objectsOriginalSize= new();


    [SerializeField] float AdditionalSize = 2;
    [SerializeField] int duration = 25;


    Vector3 AddedSize= new ();
    private void Start()
    {

       AddedSize= new Vector3 (AdditionalSize, AdditionalSize, 0);

        SetAnomalyName();


        GetOriginalSize();
    }

    void GetOriginalSize()
    {
        for (int i = 0; i < objects.Count; i++) {
            objectsOriginalSize.Add(objects[i].localScale); 
        
        }

    }



    public override void SetAnomaly()
    {
        for (int i = 0; i < objects.Count; i++)
        {

            objects[i].DOScale(objectsOriginalSize[i] + AddedSize, duration);


        }


    }


    public override void ResetAnomaly()
    {

        for (int i = 0; i < objects.Count; i++)
        {

            objects[i].localScale = objectsOriginalSize[i];
       

        }

    }
}

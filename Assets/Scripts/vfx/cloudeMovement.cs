using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class cloudeMovement : MonoBehaviour
{



    [SerializeField] List<Transform> cloudes;
    void Start()
    {
        for (int i = 0; i < cloudes.Count; i++)
        {
            cloudes[i].DOMoveX(100, 5000);
        }
    }


}

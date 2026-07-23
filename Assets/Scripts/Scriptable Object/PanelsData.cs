using UnityEngine;
using System.Collections.Generic;
using System;


[Serializable]

public class PanelsData
{


    public SpriteRenderer sprite;


    public List<ShotInfo> shots;



 
}

[Serializable]
public class ShotInfo
{

    public Transform transitionPoint;

    public float zoom;

    public float transitionTime;

}
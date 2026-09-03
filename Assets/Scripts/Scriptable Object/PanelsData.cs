using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;


[Serializable]

public class PanelsData
{


    public SpriteRenderer sprite;


    public List<ShotInfo> shots;



 
}

[Serializable]
public class ShotInfo
{
    [Header("camera")]

    public Transform transitionPoint;

    public float zoom;

    public float transitionTime;


    [Header("Text")]
    public Transform textPos;
    public Image textBg;
    public TextMeshProUGUI text;


}
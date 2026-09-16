using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;


[Serializable]

public class PanelsData
{


    public SpriteRenderer sprite;


    public List<ShotInfo> shots;


    [Header("Other between Panels ?")]
    public UnityEvent otherAction;

}

[Serializable]
public class ShotInfo
{
    [Header("==========")]

    [Header("Camera")]

    public Transform transitionPoint;

    public float zoom;

    public float transitionTime;


    [Header("Text")]
    public Transform textPos;
    public Image textBg;
    public TextMeshProUGUI text;

    [Space(5)]
    [Header("Other between Lines?")]
    public UnityEvent otherAction;

}
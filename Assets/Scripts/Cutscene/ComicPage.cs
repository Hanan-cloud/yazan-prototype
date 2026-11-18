
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public class ComicImage
{
    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;

    [Header("Zoom Settings")]
    public bool enableZoom = false;

    [Min(1f)]
    public float zoomScale = 1.5f;

    [Min(0f)]
    public float zoomDuration = 1f;

    public Ease zoomEase = Ease.OutSine;
}

[Serializable]
public class ComicPage
{
    [Header("Images (Max 5)")]
    public List<ComicImage> images = new List<ComicImage>();

    [Header("Texts")]
    [TextArea(2, 5)]
    public List<string> texts = new List<string>();
}

using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] List<GameObject> nailImages;

    [SerializeField] GameObject dollButton;

    [Space(15)]
    [Header("Colors")]
    [SerializeField] Color original;
    [SerializeField] Color fallColor;
    [SerializeField] Color resetColor;

    [Space(15)]
    [Header("audios")]
    [SerializeField] AudioClip nailsFall;
    [SerializeField] AudioClip nailsReset;
    AudioSource nailsSfx;
    [SerializeField] UnityEvent onNailFinish;

    Image doll;
    const int nailsCount=6;
    int nails;

    private void Awake()
    {
        Instance = this;
        nailsSfx = GetComponent<AudioSource>();
    }


    public int Nails { get => nails; set => nails = value; }

    private void Start()
    {
        doll = dollButton.GetComponent<Image>();
        nails = 6;


    }

    [ContextMenu("doll button effect")]
    public void NailFalls()
    {
        nails--;
        nailImages[nails].SetActive(false);

        nailsSfx.pitch = UnityEngine.Random.Range(0.8f, 1.3f);
        nailsSfx.PlayOneShot(nailsFall);

        dollButton.transform.DOShakeScale(0.5f,0.3f);
        doll.DOColor(fallColor, 0.5f).OnComplete(() => doll.DOColor(original, 0.1f));
        // play sound 
        // effects images

        if (nails == 0) { 
        
        onNailFinish?.Invoke();
        }

     


    }

  

    [ContextMenu("doll button reset ")]

    public void NailsReset()
    {

        nails = nailsCount;
        if (nails >nailsCount || nails < 0) return;

        nailsSfx.pitch = 1f;
        nailsSfx.PlayOneShot(nailsReset);

        dollButton.transform.DOShakeScale(0.5f, 0.3f);
        doll.DOColor(resetColor, 0.5f).OnComplete(() => doll.DOColor(original, 0.1f));

        for (int i = 0; i < nailImages.Count; i++) 
        {
            nailImages[i].SetActive(true);
        
        }

    }


}

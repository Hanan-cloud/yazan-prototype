using DG.Tweening;
using UnityEngine;

public class StoryAudio : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    [Header("Clips")]
    [SerializeField] private AudioClip clipA;
    [SerializeField] private AudioClip clipB;

    private bool isPlayingA = true;
    [SerializeField] float fadeInDuration=1;
    [SerializeField] float fadeOutDuration=2;
    private void Awake()
    {
        source.clip = clipA;
        source.volume = 0.18f;
        source.loop = true;
        source.Play();
    }

   
    public void Crossfade()
    {
        AudioClip nextClip = isPlayingA ? clipB : clipA;
        isPlayingA = !isPlayingA;

        source.DOFade(0f, fadeOutDuration).OnComplete(() =>
        {
            source.clip = nextClip;
            source.Play();
            source.DOFade(0.18f, fadeInDuration);
        });
    }
}

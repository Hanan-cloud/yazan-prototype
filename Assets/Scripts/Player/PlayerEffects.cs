using UnityEngine;
using System.Collections.Generic;


public class PlayerEffects : MonoBehaviour
{

    AudioSource audioSource;
    [SerializeField] List<AudioClip> clipList;
    AudioClip stepSoundClip;
    [SerializeField]  ParticleSystem stepEffect;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }




    public void Walk()
    {
        if (InputManager.Instance.Dir.x == 0) return;
            
            StepSounds();
            stepEffect.Play();


    }


    void StepSounds()
    {
        if(audioSource.isPlaying) return;

        stepSoundClip = clipList[Random.Range(0, clipList.Count)];
        audioSource.pitch = Random.Range(0.8f, 1.2f);

        audioSource.PlayOneShot(stepSoundClip);


        
    }


}

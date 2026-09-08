using UnityEngine;
using System.Collections.Generic;


public class PlayerEffects : MonoBehaviour
{

    AudioSource audio;
    [SerializeField] List<AudioClip> clipList;
    AudioClip stepSoundClip;
    [SerializeField]  ParticleSystem stepEffect;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }




    public void Walk()
    {
        if (InputManager.Instance.Dir.x == 0) return;
            
            StepSounds();
            stepEffect.Play();


    }


    void StepSounds()
    {
        if(audio.isPlaying) return;

        stepSoundClip = clipList[Random.Range(0, clipList.Count)];
        audio.pitch = Random.Range(0.8f, 1.2f);

        audio.PlayOneShot(stepSoundClip);


        
    }


}

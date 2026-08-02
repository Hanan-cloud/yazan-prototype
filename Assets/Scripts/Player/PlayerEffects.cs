using UnityEngine;
using System.Collections.Generic;


public class PlayerEffects : MonoBehaviour
{

    AudioSource audio;
    [SerializeField] List<AudioClip> clipList;
    AudioClip stepSoundClip;
    ParticleSystem leftLeg;
    [SerializeField]  ParticleSystem rightLeg;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void WalkLeftLeg()
    {
        StepSounds();
        leftLeg.Play();

    }


    public void WalkRightLeg()
    {
        StepSounds();
        rightLeg.Play();


    }


    void StepSounds()
    {
        stepSoundClip = clipList[Random.Range(0, clipList.Count)];
        audio.pitch = Random.Range(1.0f, 1.5f);

        audio.PlayOneShot(stepSoundClip);


        
    }


}

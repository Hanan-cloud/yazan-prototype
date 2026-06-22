using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EnviromentSounds : MonoBehaviour
{



    [SerializeField] private AudioMixer mixer;
    [Space(15)]
    [SerializeField] private AudioSource tentAreaAudio;
    [SerializeField] private AudioSource camelAreaAudio;
    [SerializeField] private AudioSource ruinsAreaAudio;

    AudioSource currentAudioSource;
    [Space(15)]

    [SerializeField] List<AudioClip> tentAreaClips;
    [SerializeField] List<AudioClip> camelAreaClips;
    [SerializeField] List<AudioClip> ruinsAreaClips;

    List<AudioClip> currentClips;

    [Space(15)]


    [SerializeField] private string cuttOffFreq = "CuttoffFreq";


    WaitForSeconds waitFor = new WaitForSeconds(5);


    int maxWaiting;
    int minWaiting;
    float chance;



    public void SetFilterFrequency(float frequency)
    {
        if (mixer != null)
        {
            mixer.SetFloat(cuttOffFreq, frequency);
        }
    }


    

    IEnumerator RoadAreaAmbient()
    {
        while (true)
        {

            yield return waitFor;


            if (Random.Range(0f, 101f) < chance)
            {
                currentAudioSource.PlayOneShot(currentClips[Random.Range(0, currentClips.Count)]);

            }

        }
    }

    public void TentAreaParameter()
    {
        chance = 50;
        currentClips = tentAreaClips;
        currentAudioSource = tentAreaAudio;

    }

    public void CamelAreaParameter()
    {
        chance = 50;
        currentClips = camelAreaClips;
        currentAudioSource = camelAreaAudio;

    }
    public void RuinsAreaParameter()
    {
        chance = 50;
        currentClips = ruinsAreaClips;
        currentAudioSource = ruinsAreaAudio;

    }

}

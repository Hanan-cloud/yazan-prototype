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


    private string cuttOffFreq = "CuttoffFreq";


    WaitForSeconds waitFor = new WaitForSeconds(5);


    int maxWaiting;
    int minWaiting;
    float chance;

    bool inAera=false;

    public void SetFilterFrequency(float frequency)
    {
        if (mixer != null)
        {
            mixer.SetFloat(cuttOffFreq, frequency);
        }
    }


    private void Start()
    {
        inAera = false;
        StartCoroutine(AreaAmbient());
    }

    IEnumerator AreaAmbient()
    {
        while (true)
        {

            // yield return waitFor;
            yield return new WaitForSeconds(Random.Range(10f, 20f));


            print("in area: "+ inAera);

            if (Random.Range(0f, 101f) < chance && inAera)
            {

                print("Sound On");
                currentAudioSource.PlayOneShot(currentClips[Random.Range(0, currentClips.Count)]);

            }

        }
    }

    public void TentAreaParameter()
    {
        inAera = true;
        chance = 50;
        currentClips = tentAreaClips;
        currentAudioSource = tentAreaAudio;

    }

    public void CamelAreaParameter()
    {
        inAera = true;

        chance = 50;
        currentClips = camelAreaClips;
        currentAudioSource = camelAreaAudio;

    }
    public void RuinsAreaParameter()
    {
        inAera = true;

        chance = 50;
        currentClips = ruinsAreaClips;
        currentAudioSource = ruinsAreaAudio;

    }



    public void InAreaOff()
    {
        inAera = false;
    }

}

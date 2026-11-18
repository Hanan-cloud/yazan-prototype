
using UnityEngine;
using System.Collections.Generic;

public class RandomAudioLocation : MonoBehaviour
{
    [Header("Available Locations")]
    [SerializeField] private List<Transform> locations = new List<Transform>();

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    public void PlayAtRandomLocation()
    
    {
        

        Transform randomLocation = locations[
            Random.Range(0, locations.Count)
        ];

        audioSource.gameObject.transform.position = randomLocation.position;


            audioSource.Play();

    }
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class AreaAmbientTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> ambientClips;

    [Header("Random Time")]
    [SerializeField] private float minRandomTime = 10f;
    [SerializeField] private float maxRandomTime = 20f;

    [Header("Play Chance")]
    [Range(0f, 100f)]
    [SerializeField] private float chance = 50f;

    private Coroutine ambientCoroutine;
    private bool playerInside;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (ambientCoroutine == null)
        {
            ambientCoroutine = StartCoroutine(PlayAmbient());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (ambientCoroutine != null)
        {
            StopCoroutine(ambientCoroutine);
            ambientCoroutine = null;
        }
    }

    private IEnumerator PlayAmbient()
    {
        while (playerInside)
        {
            float randomTime = Random.Range(
                minRandomTime,
                maxRandomTime
            );

            yield return new WaitForSeconds(randomTime);

            if (!playerInside)
                break;

            if (Random.Range(0f, 100f) <= chance)
            {
                if (audioSource != null &&
                    ambientClips != null &&
                    ambientClips.Count > 0)
                {
                    AudioClip clip = ambientClips[
                        Random.Range(0, ambientClips.Count)
                    ];

                    audioSource.PlayOneShot(clip);
                }
            }
        }

        ambientCoroutine = null;
    }
}

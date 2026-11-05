using UnityEngine;
using System.Collections.Generic;

public class PlayerEffects : MonoBehaviour
{
    AudioSource audioSource;

    [Header("Step Sounds")]
    [SerializeField] List<AudioClip> soilClips;
    [SerializeField] List<AudioClip> concreteClips;

    [Header("Ground Check")]
    [SerializeField] LayerMask soilLayer;
    [SerializeField] LayerMask concreteLayer;
    [SerializeField] float rayLength = 1f;

    [Header("Effects")]
    [SerializeField] ParticleSystem stepEffect;

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
        if (audioSource.isPlaying) return;

        List<AudioClip> clips = GetGroundClips();
        if (clips == null || clips.Count == 0) return;

        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Count)]);
    }

    // Raycast down and return the clip list that matches the ground layer
    List<AudioClip> GetGroundClips()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            rayLength,
            soilLayer.value | concreteLayer.value
        );

        if (!hit) return null;

        int layerBit = 1 << hit.collider.gameObject.layer;

        if ((soilLayer.value & layerBit) != 0) return soilClips;
        if ((concreteLayer.value & layerBit) != 0) return concreteClips;

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    }
}
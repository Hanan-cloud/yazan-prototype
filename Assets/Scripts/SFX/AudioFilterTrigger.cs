using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class AudioFilterTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private EnviromentSounds environmentSounds;

    [Header("Frequency")]
    [Tooltip("Cutoff frequency (Hz) when the player is at the exact center of the trigger.")]
    [SerializeField] private float minFrequency = 3000f;
    [Tooltip("Optional center point. Defaults to this trigger object's position.")]
    [SerializeField] private Transform center;

    private Collider2D triggerCollider;
    private Transform player;
    private readonly HashSet<Collider2D> playerColliders = new HashSet<Collider2D>();

    private void Awake()
    {
        triggerCollider = GetComponent<Collider2D>();
        if (environmentSounds == null)
        {
            Debug.LogWarning("Assign an EnviromentSounds component to AudioFilterTrigger.", this);
        }

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("AudioFilterTrigger requires its Collider2D to have Is Trigger enabled.", this);
        }
    }

    private void Update()
    {
        if (playerColliders.Count == 0 ||
            player == null ||
            environmentSounds == null ||
            triggerCollider == null)
            return;

        float maxFreq = Mathf.Max(environmentSounds.DefaultFrequency, 10f);
        float minFreq = Mathf.Clamp(minFrequency, 10f, maxFreq);
        float targetFrequency = Mathf.Lerp(
            minFreq,
            maxFreq,
            GetNormalizedDistance()
        );

        environmentSounds.SetFilterFrequency(targetFrequency);
    }

    private float GetNormalizedDistance()
    {
        Bounds bounds = triggerCollider.bounds;
        Vector2 triggerCenter = center != null ? center.position : transform.position;
        Vector2 min = bounds.min;
        Vector2 max = bounds.max;
        float maxDistance = Mathf.Max(
            Vector2.Distance(triggerCenter, new Vector2(min.x, min.y)),
            Vector2.Distance(triggerCenter, new Vector2(min.x, max.y)),
            Vector2.Distance(triggerCenter, new Vector2(max.x, min.y)),
            Vector2.Distance(triggerCenter, new Vector2(max.x, max.y))
        );
        float distance = Vector2.Distance(player.position, triggerCenter);

        return Mathf.Clamp01(distance / Mathf.Max(maxDistance, Mathf.Epsilon));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform playerTransform = GetPlayerTransform(collision);
        if (playerTransform == null)
            return;

        playerColliders.Add(collision);
        player = playerTransform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!playerColliders.Remove(collision))
            return;

        if (playerColliders.Count > 0)
        {
            foreach (Collider2D remainingCollider in playerColliders)
            {
                player = GetPlayerTransform(remainingCollider);
                if (player != null)
                    return;
            }

            playerColliders.Clear();
        }

        player = null;

        if (environmentSounds != null)
        {
            environmentSounds.ResetFilterFrequency();
        }
    }

    private static Transform GetPlayerTransform(Collider2D collider)
    {
        for (Transform current = collider.transform; current != null; current = current.parent)
        {
            if (current.CompareTag("Player"))
                return current;
        }

        return null;
    }

    private void OnDisable()
    {
        if (playerColliders.Count == 0)
            return;

        playerColliders.Clear();
        player = null;

        if (environmentSounds != null)
        {
            environmentSounds.ResetFilterFrequency();
        }
    }
}
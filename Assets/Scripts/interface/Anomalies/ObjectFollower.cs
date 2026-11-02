using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
        [Header("References")]
    [SerializeField] private Transform follower;
    [SerializeField] private string playerTag = "Player";

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0.8f;
    [SerializeField] private float smoothTime = 1.5f;

    private Transform player;

    private Vector3 originalPosition;

    private float velocityX;

    private bool isFollowing;

    private void Awake()
    {
        if (follower != null)
        {
            originalPosition = follower.localPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        player = other.transform;
        isFollowing = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        player = other.transform;
        isFollowing = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        isFollowing = false;
        player = null;
        velocityX = 0f;
    }

    private void Update()
    {
        if (!isFollowing || player == null || follower == null)
            return;

        FollowPlayer();
    }

    private void FollowPlayer()
    {
        float targetX = player.position.x;

        float newX = Mathf.SmoothDamp(
            follower.position.x,
            targetX,
            ref velocityX,
            smoothTime,
            moveSpeed
        );

        follower.position = new Vector3(
            newX,
            follower.position.y,
            follower.position.z
        );
    }

    private void OnDisable()
    {
        ResetState();
    }

    public void ResetState()
    {
        if (follower != null)
        {
            follower.localPosition = originalPosition;
        }

        player = null;
        isFollowing = false;
        velocityX = 0f;
    }
}

using UnityEngine;

public class ReflectionMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 followOffset;

    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning($"{nameof(ReflectionMovement)} on {name} has no player assigned.", this);
        }
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        float facingDirection = player.localScale.x < 0f ? -1f : 1f;
        Vector3 playerPosition = player.position;
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(
            playerPosition.x + followOffset.x ,
            playerPosition.y + followOffset.y,
            currentPosition.z);


        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Abs(localScale.x) * facingDirection;
        transform.localScale = localScale;
    }
}

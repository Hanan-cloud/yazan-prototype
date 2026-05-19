using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runMultiplier = 2f;

    private Vector2 moveInput;
    private bool isRunning;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Input System events
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    private void FixedUpdate()
    {
        float speed = moveSpeed;

        if (isRunning)
            speed *= runMultiplier;

        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }
}
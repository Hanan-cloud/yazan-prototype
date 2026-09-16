using System.Collections;

using UnityEngine;

public class oppositePlayer : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float acceleration = 15f;
    public float deceleration = 20f;

    private Rigidbody2D rb;
    private float currentSpeed;
    private float moveInput;
    private bool isRuning;


    string walkState = "walk";
    string runState = "run";
    string idleState = "idle";

    private string currentAnimState = "";

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer sprite;

    Directions playerCurrentDir;


    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
    WaitForSeconds wait1s = new WaitForSeconds(1);

    void Awake()
    {



        rb = GetComponent<Rigidbody2D>();


    }

    private void OnEnable()
    {
        sprite.enabled = false;
        StartCoroutine(Delay());

    }

    private void OnDisable()
    {
        sprite.enabled = false;

    }

  
    IEnumerator Delay()
    {

        yield return wait1s;
        Vector3 temp = transform.position;
        // if player moves to the right the shadow clone appears from the left
        if (PlayerController.Instance.PlayerCurrentDir == Directions.Right)
        {
            temp.x = leftPos.position.x;
            transform.position = temp;
        }
        else // player moving to the left
        {
            temp.x = rightPos.position.x;
            transform.position = temp;


        }

        sprite.enabled = true;

    }





    void Update()
    {
        if (PauseMenu.instance.IsStopped) return;

        moveInput = InputManager.Instance.Dir.x;
        isRuning = InputManager.Instance.IsRunning;

        float maxSpeed = isRuning ? runSpeed : walkSpeed;
        float targetSpeed = moveInput * maxSpeed;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelRate * Time.deltaTime);

        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveInput > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;

         SetAnimState(isRuning ? runState : walkState);
        }
        else
        {
            SetAnimState(idleState);
        }
    }

    private void SetAnimState(string state)
    {
        if (state == currentAnimState) return;
        currentAnimState = state;
        animator.Play(state);
    }

    void FixedUpdate()
    {
        if (PauseMenu.instance.IsStopped) return;

        rb.linearVelocity = new Vector2(-currentSpeed, rb.linearVelocity.y);



        if (transform.localScale.x > 0)
        {
            playerCurrentDir = Directions.Right;
        }
        if (transform.localScale.x < 0)
        {
            playerCurrentDir = Directions.Left;
        }

    }
}

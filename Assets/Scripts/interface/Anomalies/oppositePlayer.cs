using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class oppositePlayer : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 15f;
    public float deceleration = 20f;

    private Rigidbody2D rb;
    private float currentSpeed;
    private float moveInput;
    private bool isRuning;


    string isRuningSt = "IsRuning";
    string isWalkingSt = "IsWalking";


    bool isPaused = false;

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

    private void Start()
    {
        PauseMenu.OnPauseChanged += OnPause;
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




    void OnPause(bool b)
    {

        isPaused = b;
    }

    void Update()
    {
        if (isPaused == true) return;

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

            if (isRuning)
            {
              
     
                    animator.SetBool(isRuningSt, true);
                    animator.SetBool(isWalkingSt, false);

                


            }
            else
            {

       

                    animator.SetBool(isRuningSt, false);
                    animator.SetBool(isWalkingSt, true);

                

            }
        }
        else
        {


                animator.SetBool(isRuningSt, false);
                animator.SetBool(isWalkingSt, false);

            

        }
    }

    void FixedUpdate()
    {
        if (isPaused == true) return;

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

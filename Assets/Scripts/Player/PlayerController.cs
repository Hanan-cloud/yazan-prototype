using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public static PlayerController Instance;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 15f;
    public float deceleration = 20f;

    private Rigidbody2D rb;
    private float currentSpeed;
    private float moveInput;
    private bool isRuning;

    [SerializeField] Animator animator;
    [SerializeField] Animator yazanShadowAnimator;
    string isRuningSt = "IsRuning";
    string isWalkingSt = "IsWalking";


    [SerializeField] List<Animator> animators;

    Directions playerCurrentDir ;

    public Directions PlayerCurrentDir { get => playerCurrentDir; set => playerCurrentDir = value; }

    void Awake()
    {

        Instance = this;

        rb = GetComponent<Rigidbody2D>();


    }


    void Update()
    {
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
                for (int i = 0; i<animators.Count; i++)
                {
                    animators[i].SetBool(isRuningSt, true);
                    animators[i].SetBool(isWalkingSt, false);

                }

       
            }
            else
            {

                for (int i = 0; i < animators.Count; i++)
                {
                    animators[i].SetBool(isRuningSt, false);
                    animators[i].SetBool(isWalkingSt, true);

                }
     
            }
        }
        else
        {

            for (int i = 0; i < animators.Count; i++)
            {
                animators[i].SetBool(isRuningSt, false);
                animators[i].SetBool(isWalkingSt, false);

            }

        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    
    
    
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
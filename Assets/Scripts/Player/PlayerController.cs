using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 15f;
    public float deceleration = 20f;

    [SerializeField] private AudioSource breathing;
    [SerializeField] private float breathFadeDuration = 0.3f;
    [SerializeField] private float breathMaxVolume = 1f;

    [SerializeField] private float inputGraceDuration = 0.08f;

    [SerializeField] private List<Animator> animators;

    private Rigidbody2D rb;

    private float currentSpeed;
    private float moveInput;
    private bool isRuning;

    private float lastNonZeroMoveInput = 0f;
    private float inputGraceTimer = 0f;

    private bool wasActuallyRunning;

    bool isEffectivelyMoving;
    float effectiveDirSign;

    float maxSpeed;
    float targetSpeed;

    float accelRate;
    bool isActuallyRunning;

    const string Walk = "walk";
    const string Run = "run";
    const string Idle = "idle";

    private string currentAnimState = "";

    private Directions playerCurrentDir;
    public Directions PlayerCurrentDir { get => playerCurrentDir; set => playerCurrentDir = value; }
    bool isPaused;
    private bool wasPaused;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (breathing != null)
        {
            breathing.volume = 0f;
            breathing.loop = true;
            breathing.Play();
        }
    }

    void Update()
    {
        bool isPaused = PauseMenu.instance.IsStopped;

        if (isPaused != wasPaused)
        {
            wasPaused = isPaused;
            if (breathing != null)
            {
                if (isPaused)
                {
                    breathing.DOKill();
                    breathing.Pause();
                }
                else
                {
                    breathing.UnPause();
                    breathing.DOFade(isActuallyRunning ? breathMaxVolume : 0f, breathFadeDuration);
                }
            }
        }

        if (isPaused) return;

        moveInput = InputManager.Instance.Dir.x;
        isRuning = InputManager.Instance.IsRunning;

        if (moveInput != 0f)
        {
            lastNonZeroMoveInput = moveInput;
            inputGraceTimer = inputGraceDuration;
        }
        else if (inputGraceTimer > 0f)
        {
            inputGraceTimer -= Time.deltaTime;
        }

        isEffectivelyMoving = moveInput != 0f || inputGraceTimer > 0f;
        effectiveDirSign = moveInput != 0f ? moveInput : lastNonZeroMoveInput;

        maxSpeed = isRuning ? runSpeed : walkSpeed;
        targetSpeed = moveInput * maxSpeed;

        if (moveInput != 0f && currentSpeed != 0f && Mathf.Sign(moveInput) != Mathf.Sign(currentSpeed))
        {
            currentSpeed = 0f;
        }

        accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelRate * Time.deltaTime);

        isActuallyRunning = isRuning && isEffectivelyMoving;
        if (isActuallyRunning != wasActuallyRunning)
        {
            wasActuallyRunning = isActuallyRunning;
            if (breathing != null)
            {
                breathing.DOKill();
                breathing.DOFade(isActuallyRunning ? breathMaxVolume : 0f, breathFadeDuration);
            }
        }

        if (isEffectivelyMoving)
        {
            Vector3 scale = transform.localScale;
            scale.x = effectiveDirSign > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;

            SetAnimatorsState(isRuning ? Run : Walk);
        }
        else
        {
            SetAnimatorsState(Idle);
        }
    }

    private void SetAnimatorsState(string animName)
    {
        if (animName == currentAnimState) return;
        currentAnimState = animName;

        for (int i = 0; i < animators.Count; i++)
        {
            Animator anim = animators[i];
            if (anim == null || !anim.gameObject.activeSelf) continue;

            anim.Play(animName);
        }
    }

    void FixedUpdate()
    {
        if (PauseMenu.instance.IsStopped) return;

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);

        if (transform.localScale.x > 0)
        {
            playerCurrentDir = Directions.Right;
        }
        else if (transform.localScale.x < 0)
        {
            playerCurrentDir = Directions.Left;
        }
    }
}
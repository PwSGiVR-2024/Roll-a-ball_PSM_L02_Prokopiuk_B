using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    protected Vector3 currentMovement;
    Vector2 currentMovementInput;
    Rigidbody characterController;
    protected Animator playerAnimator;
    protected AudioSource audioSource;
    // Removed ParticleSystem dust;

    [SerializeField] float acceleration = 10f;
    [SerializeField] float maxSpeed = 5f;
    bool isSafe = false;
    bool gameStarted = false;

    void Awake()
    {
        characterController = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        audioSource = transform.Find("GunShot").GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Start()
    {
        // Removed dust initialization
        // dust = transform.Find("Dust").GetComponent<ParticleSystem>();
    }

    protected virtual void OnEnable()
    {
        EventManager.Instance.Subscribe("CountdownFinished", StartGame);
    }

    void StartGame(object message)
    {
        gameStarted = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "RedLine")
        {
            isSafe = true;

            if (CompareTag("Player"))
            {
                EventManager.Instance.TriggerEvent("GameWon", null);
            }
        }
    }

    public bool IsMoving => playerAnimator.GetBool("running") || playerAnimator.GetBool("stopping");
    public bool IsSafe => isSafe;

    public void KillPlayer()
    {
        if (PlayerIsDead() || isSafe) return;
        playerAnimator.SetBool("die", true);
        audioSource.Play();
        GetComponent<Collider>().enabled = false;
        if (CompareTag("Player"))
        {
            EventManager.Instance.TriggerEvent("GameLost", null);
        }
    }

    public bool PlayerIsDead()
    {
        return playerAnimator.GetBool("die");
    }

    void PlayerMovement()
    {
        if (CompareTag("Player") && !PlayerIsDead())
        {
            float relativeSpeed = characterController.linearVelocity.magnitude / maxSpeed;
            EventManager.Instance.TriggerEvent("PlayerSpeed", IsMagnitudeLowerThan() ? 0 : relativeSpeed);
            EventManager.Instance.TriggerEvent("PlayerStatus", IsMoving ? playerAnimator.GetBool("running") ? "Running" : "Stopping" : "Stopped");
        }

        if (currentMovement != Vector3.zero && !PlayerIsDead())
        {
            characterController.AddForce(currentMovement.normalized * acceleration, ForceMode.Acceleration);
        }
        else
        {
            characterController.linearVelocity = Vector3.zero;
            playerAnimator.SetBool("stopping", true);
        }

        if (characterController.linearVelocity.magnitude > maxSpeed)
        {
            characterController.linearVelocity = characterController.linearVelocity.normalized * maxSpeed;
        }

        if (IsMagnitudeLowerThan())
        {
            playerAnimator.SetBool("stopping", false);
        }
    }

    // Removed PlayDustAnimation() function

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void Update()
    {
        // Removed call to PlayDustAnimation();
    }

    public void OnMove(InputValue value)
    {
        if (!CompareTag("Player") || playerAnimator.GetBool("die") || !gameStarted) return;
        currentMovementInput = value.Get<Vector2>();

        if (currentMovementInput != Vector2.zero)
        {
            playerAnimator.SetBool("running", true);
        }
        else
        {
            playerAnimator.SetBool("running", false);
        }

        currentMovement.x = currentMovementInput.x;
        currentMovement.z = Math.Abs(currentMovementInput.y);
    }

    bool IsMagnitudeLowerThan(float minMagnitude = 0.1f)
    {
        return characterController.linearVelocity.magnitude < minMagnitude;
    }
}
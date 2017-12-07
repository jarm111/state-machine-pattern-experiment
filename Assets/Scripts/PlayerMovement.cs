using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public enum PlayerStates
    {
        Walking,
        Sprinting,
        Crouched,
        Prone,
        Jumping
    }

    [SerializeField]
    private float walkingSpeed = 10;
    [SerializeField]
    private float jumpForce = 300;
    [SerializeField]
    private float sprintSpeedMultiplier = 1.3f;
    [SerializeField]
    private float crouchingSpeedMultiplier = 0.7f;
    [SerializeField]
    private float proneSpeedMultiplier = 0.4f;
    [SerializeField]
    private Vector2 sizeWhenStanding = new Vector2(2, 2);
    [SerializeField]
    private float crouchedSizeMultiplier = 0.5f;
    [SerializeField]
    private float proneSizeMultiplier = 0.25f;

    private float currentMovementSpeed;
    private PlayerStates playerState;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;

    public PlayerStates PlayerState
    {
        get
        {
            return playerState;
        }
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerState = PlayerStates.Walking;
        currentMovementSpeed = walkingSpeed;
    }

    private void Update()
    {
        UpdatePlayerState();
    }

    private void FixedUpdate()
    {
        Move(currentMovementSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeStateWhenLandingFromJump();
    }

    private void UpdatePlayerState()
    {
        switch (playerState)
        {
            case PlayerStates.Walking:

                if (Input.GetButtonDown("Jump"))
                {
                    playerState = PlayerStates.Jumping;
                    Jump();
                }

                if (Input.GetButtonDown("Duck"))
                {
                    playerState = PlayerStates.Crouched;
                    Crouch();
                }

                if (Input.GetButtonDown("Sprint"))
                {
                    playerState = PlayerStates.Sprinting;
                    Sprint();
                }

                break;

            case PlayerStates.Sprinting:

                if (Input.GetButtonUp("Sprint"))
                {
                    playerState = PlayerStates.Walking;
                    StopSprint();
                }

                if (Input.GetButtonDown("Jump"))
                {
                    playerState = PlayerStates.Jumping;
                    StopSprint();
                    Jump();
                }

                if (Input.GetButtonDown("Duck"))
                {
                    playerState = PlayerStates.Crouched;
                    StopSprint();
                    Crouch();
                }

                break;

            case PlayerStates.Crouched:

                if (Input.GetButtonDown("Jump"))
                {
                    playerState = PlayerStates.Walking;
                    StandUp();
                }

                if (Input.GetButtonDown("Duck"))
                {
                    playerState = PlayerStates.Prone;
                    Prone();
                }

                break;

            case PlayerStates.Prone:

                if (Input.GetButtonDown("Jump"))
                {
                    playerState = PlayerStates.Crouched;
                    Crouch();
                }

                break;

            case PlayerStates.Jumping:

                break;
        }
    }

    private void ChangeStateWhenLandingFromJump()
    {
        if (CheckIsGrounded() && playerState == PlayerStates.Jumping)
        {
            playerState = PlayerStates.Walking;
        }
    }

    private bool CheckIsGrounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x,
            transform.position.y - capsuleCollider.size.y - .1f),
            Vector2.down, .1f,
            LayerMask.GetMask("Ground"));
    }

    private void Move(float speed)
    {
        rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y));
    }

    private void Jump()
    {
        rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpForce));
    }

    private void Crouch()
    {
        gameObject.transform.localScale = new Vector2 (sizeWhenStanding.x, sizeWhenStanding.y * crouchedSizeMultiplier);
        currentMovementSpeed = walkingSpeed * crouchingSpeedMultiplier;
    }

    private void Prone()
    {
        gameObject.transform.localScale = new Vector2(sizeWhenStanding.x, sizeWhenStanding.y * proneSizeMultiplier); ;
        currentMovementSpeed = walkingSpeed * proneSpeedMultiplier;
    }

    private void StandUp()
    {
        gameObject.transform.localScale = sizeWhenStanding;
        currentMovementSpeed = walkingSpeed;
    }

    private void Sprint()
    {
        currentMovementSpeed = walkingSpeed * sprintSpeedMultiplier;
    }

    private void StopSprint()
    {
        currentMovementSpeed = walkingSpeed;
    }
}

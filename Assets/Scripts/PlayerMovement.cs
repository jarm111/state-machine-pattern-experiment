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
        gameObject.transform.localScale = new Vector3(2, 1, 1);
        currentMovementSpeed = walkingSpeed * crouchingSpeedMultiplier;
    }

    private void StandUp()
    {
        gameObject.transform.localScale = new Vector3(2, 2, 1);
        currentMovementSpeed = walkingSpeed;
    }

    private bool CheckIsGrounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, 
            transform.position.y - capsuleCollider.size.y - .1f), 
            Vector2.down, .1f, 
            LayerMask.GetMask("Ground"));
    }

    private void Prone()
    {
        gameObject.transform.localScale = new Vector3(2, 0.5f, 1);
        currentMovementSpeed = walkingSpeed * proneSpeedMultiplier;
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

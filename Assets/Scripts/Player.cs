using UnityEngine;

public class Player : MonoBehaviour
{

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
    private bool isSprinting;
    private bool isCrouching;
    private bool isProne;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        currentMovementSpeed = walkingSpeed;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isCrouching)
            {
                StandUp();
            }
            else if (isProne)
            {
                Crouch();
            }
            else if (CheckIsGrounded())
            {
                StopSprint();
                Jump();
            }
        }

        if (Input.GetButtonDown("Duck") && CheckIsGrounded())
        {
            if (!isCrouching && !isProne)
            {
                StopSprint();
                Crouch();
            }
            else if (!isProne)
            {
                Prone();
            } 
        }

        if (Input.GetButtonDown("Sprint") && !isSprinting && CheckIsGrounded() && !isCrouching && !isProne)
        {
            Sprint();
        }

        if (Input.GetButtonUp("Sprint") && isSprinting)
        {
            StopSprint();
        }
    }

    private void FixedUpdate()
    {
        Move(currentMovementSpeed);
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
        isCrouching = true;
        isProne = false;
    }

    private void StandUp()
    {
        gameObject.transform.localScale = new Vector3(2, 2, 1);
        currentMovementSpeed = walkingSpeed;
        isCrouching = false;
    }

    private bool CheckIsGrounded()
    {
        bool isGrounded = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - capsuleCollider.size.y - .1f), Vector2.down, .1f, LayerMask.GetMask("Ground"));
        Debug.Log(isGrounded);
        return isGrounded;
    }

    private void Prone()
    {
        gameObject.transform.localScale = new Vector3(2, 0.5f, 1);
        currentMovementSpeed = walkingSpeed * proneSpeedMultiplier;
        isProne = true;
        isCrouching = false;
    }

    private void Sprint()
    {
        currentMovementSpeed = walkingSpeed * sprintSpeedMultiplier;
        isSprinting = true;
    }

    private void StopSprint()
    {
        currentMovementSpeed = walkingSpeed;
        isSprinting = false;
    }
}

using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float walkingSpeed = 10;
    [SerializeField]
    private float jumpForce = 300;
    [SerializeField]
    private float crouchingSpeedMultiplier = 0.8f;
    [SerializeField]
    private float proneSpeedMultiplier = 0.4f;

    private float currentMovementSpeed;
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
                Jump();
            }
        }

        if (Input.GetButtonDown("Duck") && CheckIsGrounded())
        {
            if (!isCrouching && !isProne)
            {
                Crouch();
            }
            else if (!isProne)
            {
                Prone();
            } 
        }

        //Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - capsuleCollider.size.y - .01f), Vector2.down * .1f, Color.green, 0.5f, false);
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
        //capsuleCollider.size = new Vector2(1, .5f);
        isCrouching = true;
        isProne = false;
    }

    private void StandUp()
    {
        gameObject.transform.localScale = new Vector3(2, 2, 1);
        currentMovementSpeed = walkingSpeed;
        //capsuleCollider.size = new Vector2(1, 1);
        isCrouching = false;
    }

    private bool CheckIsGrounded()
    {
        // LayerMask.NameToLayer("Ground")
        bool isGrounded = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - capsuleCollider.size.y - .1f), Vector2.down, .1f, LayerMask.GetMask("Ground"));
        //bool isGrounded = (Mathf.Abs(rb2d.velocity.y) < 0.1);
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
}

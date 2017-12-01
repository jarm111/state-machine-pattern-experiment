using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private float jumpForce = 1;

    private bool isCrouching;
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            if (isCrouching)
            {
                gameObject.transform.localScale = new Vector3(2, 2, 1);
                capsuleCollider.size = new Vector2(1, 1);
                isCrouching = false;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(2, 1, 1);
                capsuleCollider.size = new Vector2(1, .5f);
                isCrouching = true;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb2d.velocity.y));
    }

    private void Jump()
    {
        rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpForce));
    }

    //private void Crouch()
    //{

    //}
}

using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float movementSpeed = 1;
    [SerializeField]
    private float jumpForce = 1;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rb2d.AddForce(new Vector2(rb2d.velocity.x, jumpForce));
        }
    }

    private void FixedUpdate()
    {
        rb2d.AddForce(new Vector2(Input.GetAxis("Horizontal") * movementSpeed, rb2d.velocity.y));
    }
}

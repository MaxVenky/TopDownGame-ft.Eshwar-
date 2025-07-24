using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float movementX;
    float movementZ;
    [SerializeField]
    float speed = 10;
    Vector3 movement;
    bool isGrounded;
    bool jumped = false;
    Rigidbody rb;

    [SerializeField]
    float jumpForce;



    void Start()
    {
        isGrounded = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(movementX, 0f, movementZ);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumped = true;
                jump();
            }
            else if (jumped)
            {
                jump();
                jumped = false;
            }
        }
    }

    void jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}

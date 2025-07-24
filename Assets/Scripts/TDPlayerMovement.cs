using UnityEngine;
using System.Collections;

public class TDPlayerMovement : MonoBehaviour
{
    float movementX;
    float movementZ;
    [SerializeField]
    float speed = 10;
    Vector3 movement;
    bool isDashing = false;
    [SerializeField]
    float dashingForce;
    [SerializeField]
    float dashingTime;
    [SerializeField]
    float dashCooldown;
    bool canDash = true;
    TrailRenderer trailRenderer;
    [SerializeField]
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(movementX, 0f, movementZ);
        movement.Normalize();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(dash());
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    IEnumerator dash()
    {
        canDash = false;
        Invoke("waitToDash", dashCooldown);
        isDashing = true;
        rb.AddForce(movement * dashingForce, ForceMode.Impulse);
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        rb.linearVelocity = Vector3.zero;
        trailRenderer.emitting = false;
        isDashing = false;
    }

    void waitToDash()
    {
        canDash = true;
    }
}

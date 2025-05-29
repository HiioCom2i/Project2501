using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    private readonly float moveSpeed = 100.0f;
    private readonly float jumpForce = 5.0f;
    private readonly bool airControl = true;

    public float maxHealth = 3.0f;
    public float attackDamage = 1.0f;

    private new Rigidbody rigidbody;

    const float groundedRadius = 0.05f;
    public LayerMask groundLayer;
    private bool grounded;
    public Transform groundCheck;

    private bool facingRight = true;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundedRadius, groundLayer);
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public void Move(float moveX, float moveZ, bool jump)
    {
        if (grounded || airControl)
        {
            if ((moveX > 0 && !facingRight) || (moveX < 0 && facingRight))
            {
                Flip();
            }

            Vector3 targetVelocity = new(moveSpeed * moveX, rigidbody.linearVelocity.y, moveSpeed * moveZ);
            rigidbody.linearVelocity = targetVelocity;
        }

        if (grounded && jump)
        {
            Vector3 targetVelocity = new(rigidbody.linearVelocity.z, jumpForce, rigidbody.linearVelocity.z);
            rigidbody.linearVelocity = targetVelocity;
            grounded = false;
        }
    }

    public void ReceiveDamage(float damage)
    {
        GetComponent<SpriteRenderer>().color = new Color(1, GetComponent<SpriteRenderer>().color.g - 0.1f, GetComponent<SpriteRenderer>().color.b - 0.1f, 1);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}

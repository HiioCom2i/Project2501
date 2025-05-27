using UnityEngine;
using UnityEngine.Events;

public class GameCharacterController : MonoBehaviour
{
    private readonly float moveSpeed = 100.0f;
    private readonly float jumpForce = 150.0f;
    private readonly bool airControl = true;

    private new Rigidbody rigidbody;

    const float groundedRadius = 0.05f;
    public LayerMask groundLayer;
    private bool grounded;
    public Transform groundCheck;

    private bool facingRight = true;

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        this.grounded = Physics.CheckSphere(groundCheck.position, groundedRadius, groundLayer);
    }

    public bool IsGrounded()
    {
        return this.grounded;
    }

    public void Move(float moveX, float moveZ, bool jump)
    {
        if (grounded || airControl)
        {
            if (moveX > 0 && !facingRight || moveX < 0 && facingRight)
            {
                Flip();
            }

            Vector3 targetVelocity = new(moveSpeed * moveX, rigidbody.linearVelocity.y, moveSpeed * moveZ);
            rigidbody.linearVelocity = targetVelocity;
        }

        if (grounded && jump)
        {
            grounded = false;
            rigidbody.AddForce(new Vector3(0, jumpForce, 0));
        }
    }

    public void Punch()
    {
        // Colocar hitbox de soco
        // Fazer um callback de remover pra quando acabar
    }

    private void Flip()
    {
        facingRight = !facingRight;
        GetComponent<SpriteRenderer>().flipX = !facingRight;
    }
}

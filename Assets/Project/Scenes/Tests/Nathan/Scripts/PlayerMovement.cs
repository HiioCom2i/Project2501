using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameCharacterController controller;
    public Animator animator;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;

    bool attack = false;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (jumpAction.IsPressed())
        {
            animator.SetBool("IsJumping", true);
        }

        if (attackAction.IsPressed())
        {
            attack = true;
            animator.SetBool("IsPunch", true);
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x) + Mathf.Abs(rb.linearVelocity.z));
        animator.SetBool("IsJumping", !controller.IsGrounded() && rb.linearVelocity.y > 0.1f);
        animator.SetBool("IsFalling", !controller.IsGrounded() && rb.linearVelocity.y < -0.1f);
    }

    public void EndPunch()
    {
        animator.SetBool("IsPunch", false);
    }

    void FixedUpdate()
    {
        if (attack)
        {
            controller.Punch();
        }
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        controller.Move(moveValue.x * Time.fixedDeltaTime, moveValue.y * Time.fixedDeltaTime, jumpAction.IsPressed());
        attack = false;
    }
}
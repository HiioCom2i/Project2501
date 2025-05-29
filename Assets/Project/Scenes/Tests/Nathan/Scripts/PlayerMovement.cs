using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public GameCharacterController controller;
    public Animator animator;
    
    public Transform punchPoint;
    public float punchRange = 0.5f;
    public LayerMask enemyLayers;

    public bool isPunching;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

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
            if (!attackAction.IsPressed())
            {
                animator.SetBool("IsPunch", false);
            }
        }
        if (attackAction.IsPressed())
        {
            animator.SetBool("IsPunch", true);
        }

        if (isPunching)
        {
            RaycastHit[] hits = Physics.SphereCastAll(punchPoint.position, punchRange, transform.right, 0f, enemyLayers);
            print(hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                GameCharacterController character = hits[i].collider.gameObject.GetComponent<GameCharacterController>();
                character.ReceiveDamage(controller.attackDamage);
            }
        }

        Camera.main.transform.position = new(transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

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
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        controller.Move(moveValue.x * Time.fixedDeltaTime, moveValue.y * Time.fixedDeltaTime, jumpAction.IsPressed());
    }
}
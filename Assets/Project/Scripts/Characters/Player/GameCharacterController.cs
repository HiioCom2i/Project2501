using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using static EnemyAI;

public class GameCharacterController : MonoBehaviour
{
    public float moveSpeed = 100.0f;
    private const float jumpForce = 5.0f;

    public float attackDamage = 1.0f;

    private new Rigidbody rigidbody;

    private const float groundedRadius = 0.05f;
    public LayerMask groundLayer;
    private bool grounded;
    public Transform groundCheck;

    [Header("Status")]
    public float vidaMax = 3f;
    private float vida;

    private SpriteRenderer[] spriteRenderers;

    private bool facingRight = true;

    public event OnDeathDelegate OnDeath;

    private void Start()
    {
        vida = vidaMax;
    }
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
        if ((moveX > 0 && !facingRight) || (moveX < 0 && facingRight))
        {
            Flip();
        }

        Vector3 targetVelocity = new(moveSpeed * moveX, rigidbody.linearVelocity.y, moveSpeed * moveZ);
        rigidbody.linearVelocity = targetVelocity;

        if (grounded && jump)
        {
            Vector3 jumpVelocity = new(rigidbody.linearVelocity.z, jumpForce, rigidbody.linearVelocity.z);
            rigidbody.linearVelocity = jumpVelocity;
            grounded = false;
        }
    }

    public void ReceiveDamage(float damage)
    {
        vida -= damage;

        Debug.Log($"Player took {damage} damage. Current health: {vida}");

        if (vida <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    void Die()
    {

        //GetComponent<Collider>().enabled = false;

        //OnDeath?.Invoke();
        //StartCoroutine(DeathAnimation());

        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator FlashRed()
    {
        foreach (var sr in spriteRenderers)
        {
            sr.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var sr in spriteRenderers)
        {
            sr.color = Color.white;
        }
    }

    //System.Collections.IEnumerator DeathAnimation()
    //{
    //    float timer = 0;
    //    while (timer < 1f)
    //    {
    //        transform.position += 2f * Time.deltaTime * Vector3.down;
    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    Destroy(gameObject);
    //}

    private void Flip()
    {
        facingRight = !facingRight;
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}

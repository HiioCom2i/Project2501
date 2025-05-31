using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyAI : MonoBehaviour
{
    private enum State { IDLE, CHASE, ATTACK, COOLDOWN, HIT, DEAD }
    private State currentState = State.IDLE;
    public Animator animator;

    public GameCharacterController controller;

    [Header("AI Settings")]
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    [Header("Combat Settings")]
    public float attackCooldown = 1.5f;
    public float stunDuration = 0.5f;

    [Header("Status")]
    public float maxHealth = 3f;
    private float currentHealth;

    private Transform player;
    private Rigidbody rb;
    private SpriteRenderer[] spriteRenderers;

    private float cooldownTimer;
    private float stunTimer;

    // Delegate para eventos de morte
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        currentHealth = maxHealth;

        // Encontrar o player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player não encontrado! Certifique-se de que o Player tem a tag 'Player'");
        }
    }

    private void FixedUpdate()
    {
        if (currentState == State.CHASE)
        {
            ChasePlayer();
        }
    }

    void Update()
    {
        if (player == null || currentState == State.DEAD) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // State Machine
        switch (currentState)
        {
            case State.IDLE:
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = State.CHASE;
                }
                break;

            case State.CHASE:
                if (distanceToPlayer <= attackRange)
                {
                    currentState = State.ATTACK;
                }
                else if (distanceToPlayer > detectionRange * 1.5f)
                {
                    currentState = State.IDLE;
                }
                break;

            case State.ATTACK:
                PerformAttack();
                currentState = State.COOLDOWN;
                cooldownTimer = attackCooldown;
                break;

            case State.COOLDOWN:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    currentState = State.CHASE;
                }
                break;

            case State.HIT:
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    currentState = State.CHASE;
                }
                break;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = player.position - transform.position;
        controller.Move(direction.x, direction.z, false);
    }

    void PerformAttack()
    {
        animator.SetBool("IsPunch", true);

        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange)
            {
                GameCharacterController playerController = player.GetComponent<GameCharacterController>();
                if (playerController != null)
                {
                    playerController.ReceiveDamage(controller.attackDamage);
                }
            }
        }

        animator.SetBool("IsPunch", false);
    }

    public void TakeDamage(float damage)
    {
        if (currentState == State.DEAD) return;

        currentHealth -= damage;

        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            currentState = State.HIT;
            stunTimer = stunDuration;
            StartCoroutine(FlashRed());
        }
    }

    void Die()
    {
        currentState = State.DEAD;

        GetComponent<Collider>().enabled = false;

        OnDeath?.Invoke();
        StartCoroutine(DeathAnimation());
    }

    System.Collections.IEnumerator FlashRed()
    {
        foreach (var sr in spriteRenderers)
        {
            sr.color = Color.white;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var sr in spriteRenderers)
        {
            sr.color = Color.red;
        }
    }

    System.Collections.IEnumerator DeathAnimation()
    {
        // Cair
        float timer = 0;
        while (timer < 1f)
        {
            transform.position += 2f * Time.deltaTime * Vector3.down;
            timer += Time.deltaTime;
            yield return null;
        }

        // Destruir
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar ranges no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
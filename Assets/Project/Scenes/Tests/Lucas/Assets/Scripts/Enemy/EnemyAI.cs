using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State { IDLE, CHASE, ATTACK, COOLDOWN, HIT, DEAD }
    private State currentState = State.IDLE;

    [Header("AI Settings")]
    [SerializeField] public float detectionRange = 5f;
    [SerializeField] public float attackRange = 1.5f;
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public float verticalSpeed = 1f;

    [Header("Combat Settings")]
    [SerializeField] public float health = 3f;
    [SerializeField] public float attackDamage = 1f;
    [SerializeField] public float attackCooldown = 1.5f;
    [SerializeField] public float stunDuration = 0.5f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;

    private float cooldownTimer;
    private float stunTimer;

    // Para movimento vertical limitado
    private float minY = -2.5f;
    private float maxY = -0.5f;

    // Delegate para eventos de morte
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate onDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

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
                ChasePlayer();
                if (distanceToPlayer <= attackRange)
                {
                    currentState = State.ATTACK;
                }
                else if (distanceToPlayer > detectionRange * 1.5f) // Margem para não ficar entrando e saindo
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
        Vector2 direction = (player.position - transform.position).normalized;

        // Movimento com velocidades diferentes para X e Y
        Vector2 moveVelocity = new Vector2(
            direction.x * moveSpeed,
            direction.y * verticalSpeed
        );

        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        // Limitar movimento vertical
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;

        // Flip sprite baseado na direção
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }

    void PerformAttack()
    {
        Debug.Log("Enemy Attack!");

        // Aqui você pode adicionar dano ao player
        // Por enquanto, apenas um log
    }

    public void TakeDamage(float damage)
    {
        if (currentState == State.DEAD) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Entrar em estado de stun
            currentState = State.HIT;
            stunTimer = stunDuration;

            // Feedback visual
            StartCoroutine(FlashRed());
        }
    }

    void Die()
    {
        currentState = State.DEAD;

        // Desabilitar colisão
        GetComponent<Collider2D>().enabled = false;

        onDeath?.Invoke();
        // Feedback visual de morte (cair e desaparecer)
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
            transform.position += Vector3.down * 2f * Time.deltaTime;
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
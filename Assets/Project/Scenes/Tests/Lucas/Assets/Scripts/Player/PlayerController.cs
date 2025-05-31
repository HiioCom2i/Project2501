using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float verticalSpeed = 2.5f;

    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public float attackDamage = 1f;
    public float attackCooldown = 0.5f;

    private Rigidbody rb;
    private float nextAttackTime = 0f;
    private Vector3 movement;

    // Limites do cenário
    private readonly float minY = -2.5f;
    private readonly float maxY = -0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.tag = "Player"; // Importante para a IA encontrar o player
    }

    void Update()
    {
        // Input de movimento
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Flip do sprite
        if (movement.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(movement.x), 1, 1);
        }

        // Ataque
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void FixedUpdate()
    {
        // Movimento com velocidades diferentes para X e Y
        Vector3 moveVelocity = new(movement.x * moveSpeed, movement.y * verticalSpeed, movement.z);
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        // Limitar movimento vertical
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    void Attack()
    {
        // Visual feedback temporário
        Debug.Log("=== INICIANDO ATAQUE ===");
        Debug.Log($"Posição do Player: {transform.position}");
        Debug.Log($"Direção (transform.right): {transform.right}");
        Debug.Log($"Attack Range: {attackRange}");
        Debug.Log("Player Attack!");

        StartCoroutine(AttackFlash());

        // Centro do círculo de ataque
        Vector3 attackCenter = transform.position + transform.right * 0.5f;
        Debug.Log($"Centro do ataque: {attackCenter}");

        // Detectar inimigos na área de ataque
        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter, attackRange);
        Debug.Log($"Colliders detectados: {hitEnemies.Length}");

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log($"Colidiu com: {enemy.name}, Tag: {enemy.tag}");
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log($"É um inimigo! Aplicando {attackDamage} de dano");
                enemy.GetComponent<EnemyAI>()?.TakeDamage(attackDamage);
            }
        }
        Debug.Log("=== ATAQUE FINALIZADO ===");
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Visualizar área de ataque no editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.right * 0.5f, attackRange);
        }
    }

    IEnumerator AttackFlash()
    {
        // Pegar o SpriteRenderer do filho "Visual"
        SpriteRenderer sprite = transform.Find("Visual").GetComponent<SpriteRenderer>();

        if (sprite == null)
        {
            Debug.LogError("Não encontrou SpriteRenderer no Visual!");
            yield break;
        }

        Color originalColor = sprite.color;
        sprite.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        sprite.color = originalColor;
    }
}
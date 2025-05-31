using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public float spawnInterval = 3f;
    [SerializeField] public int maxEnemies = 5;
    [SerializeField] public float spawnRadius = 2f;

    [Header("Spawn Points")]
    [SerializeField] public bool useMultipleSpawnPoints = false;
    [SerializeField] public Transform[] spawnPoints;

    private int currentEnemies = 0;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab não foi atribuído no EnemySpawner!");
            return;
        }

        // Começar a spawnar inimigos
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1f); // Delay inicial

        while (true)
        {
            if (currentEnemies < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;

        if (useMultipleSpawnPoints && spawnPoints.Length > 0)
        {
            // Usar pontos de spawn específicos
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPosition = randomPoint.position;
        }
        else
        {
            // Spawn em posição aleatória ao redor do spawner
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y * 0.5f, 0);

            // Garantir que está dentro dos limites verticais
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -2.5f, -0.5f);
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemies++;

        // Registrar quando o inimigo morrer
        enemy.GetComponent<EnemyAI>().onDeath += OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        currentEnemies--;
    }

    void OnDrawGizmosSelected()
    {
        // Visualizar área de spawn
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}

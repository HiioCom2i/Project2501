using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour
{
    public EnemySpawner[] enemySpawners;
    public Transform enemySpawnersTriggerLimit;

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        // TODO: Bloquear o caminho (limitar o X máximo da câmera e do personagem)

        SpawnEnemies();

        // TODO: Desbloquear o caminho (desbloquear o X máximo câmera e do personagem)
        // Talvez colocar um indicativo que já pode andar pra frente
    }

    public void SpawnEnemies()
    {
        print("Spawnando inimigos:");
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            EnemySpawner enemySpawner = enemySpawners[i];

            print("Onda de inimigos - n° " + i);
            GameObject[] entities = enemySpawner.SpawnEntities();

            bool allEntitiesDead = false;
            while (!allEntitiesDead)
            {
                foreach (GameObject entity in entities)
                {
                    if (entity.activeSelf) break;
                    allEntitiesDead = true;
                }
            }
        }
    }
}

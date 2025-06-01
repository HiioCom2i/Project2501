using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnObject;

    public GameObject[] SpawnEntities()
    {
        GameObject[] entities = { };

        Transform[] spawnTransforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform spawnTransform in spawnTransforms)
        {
            GameObject entity = Instantiate(spawnObject, spawnTransform.position, Quaternion.identity);
            entities.Append(entity);
        }
        Destroy(gameObject);

        return entities;
    }
}

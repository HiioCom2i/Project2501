using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnObject;

    public void SpawnEntities()
    {
        Transform[] spawnTransforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform spawnTransform in spawnTransforms)
        {
            _ = Instantiate(spawnObject, spawnTransform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

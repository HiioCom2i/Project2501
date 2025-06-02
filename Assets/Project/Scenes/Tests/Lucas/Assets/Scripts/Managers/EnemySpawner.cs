using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnObject;
    public float detectionRange = 5f;
    private Transform player;
    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("Player não encontrado! Certifique-se de que o Player tem a tag 'Player'");
            return;
        }

        player = playerObj.transform;
    }


    void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
            {
            SpawnEntities();
        }

    }
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

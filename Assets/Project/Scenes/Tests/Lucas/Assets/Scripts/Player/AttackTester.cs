using UnityEngine;

public class AttackTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("=== TESTE DE DETECÇÃO ===");

            Collider2D[] allColliders = Physics2D.OverlapCircleAll(transform.position, 5f);
            Debug.Log($"Total de colliders em raio de 5: {allColliders.Length}");

            foreach (var col in allColliders)
            {
                Debug.Log($"- {col.name} (Tag: {col.tag})");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
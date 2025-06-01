using UnityEngine;

public class ShadowController : MonoBehaviour
{
    public float floorHeight = 0f;

    void Update()
    {
        Vector3 newPosition = new(transform.position.x, floorHeight, transform.position.z);
        transform.position = newPosition;
    }
}

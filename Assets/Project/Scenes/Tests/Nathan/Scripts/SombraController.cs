using UnityEngine;

public class SombraController : MonoBehaviour
{
    public Transform personagem;
    public float alturaChao = 0f;
    public float escalaMinima = 0.1f;
    public float escalaMaxima = 0.25f;
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 novaPosicao = new Vector3 (personagem.position.x, alturaChao, personagem.position.z);
        transform.position = novaPosicao;
    }
}

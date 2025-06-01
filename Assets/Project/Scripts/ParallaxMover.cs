using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
    [Header("Configurações de Parallax")]
    public float velocidade = 1.0f;
    public float limiteEsquerdo = -1000f;
    public float limiteDireito = 1000f;
    public bool moverAutomatico = true;
    
    private Transform[] objetosFilhos;
    private Vector3[] posicoesIniciais;
    
    void Start()
    {
        // Armazena todos os objetos filhos e suas posições iniciais
        int quantidadeFilhos = transform.childCount;
        objetosFilhos = new Transform[quantidadeFilhos];
        posicoesIniciais = new Vector3[quantidadeFilhos];
        
        for (int i = 0; i < quantidadeFilhos; i++)
        {
            objetosFilhos[i] = transform.GetChild(i);
            posicoesIniciais[i] = objetosFilhos[i].transform.position;
        }
    }
    
    void Update()
    {
        if (moverAutomatico)
        {
            MoverCamada();
        }
    }
    
    void MoverCamada()
    {
        // Move todos os objetos filhos
        for (int i = 0; i < objetosFilhos.Length; i++)
        {
            if (objetosFilhos[i] != null)
            {
                // Move para a esquerda
                objetosFilhos[i].transform.Translate(Vector3.left * velocidade * Time.deltaTime);
                
                // Se saiu da tela à esquerda, reposiciona à direita
                if (objetosFilhos[i].transform.position.x < limiteEsquerdo)
                {
                    Vector3 novaPosicao = objetosFilhos[i].transform.position;
                    novaPosicao.x = limiteDireito;
                    objetosFilhos[i].transform.position = novaPosicao;
                }
            }
        }
    }
    
    // Para reiniciar as posições se necessário
    public void ReiniciarPosicoes()
    {
        for (int i = 0; i < objetosFilhos.Length && i < posicoesIniciais.Length; i++)
        {
            if (objetosFilhos[i] != null)
            {
                objetosFilhos[i].transform.position = posicoesIniciais[i];
            }
        }
    }
}
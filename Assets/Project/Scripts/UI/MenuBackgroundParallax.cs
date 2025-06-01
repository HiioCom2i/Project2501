using UnityEngine;

public class MenuBackgroundParallax : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidadeBase = 30f; // Velocidade base em pixels por segundo
    
    [Header("Multiplicadores de Velocidade")]
    [SerializeField] private float multiplicadorNuvens = 0.5f; // Nuvens se movem mais devagar
    [SerializeField] private float multiplicadorPredios = 1.5f; // Prédios se movem mais rápido
    
    [Header("Referências das Layers")]
    [SerializeField] private Transform layerNuvens;
    [SerializeField] private Transform layerPredios;
    
    [Header("Limites de Loop")]
    [SerializeField] private float limiteHorizontal = 1000f; // Quando resetar a posição
    
    private Vector3 posicaoInicialNuvens;
    private Vector3 posicaoInicialPredios;
    
    void Start()
    {
        // Encontra as layers automaticamente se não estiverem atribuídas
        if (layerNuvens == null)
            layerNuvens = transform.Find("Layer1_Ceu");
            
        if (layerPredios == null)
            layerPredios = transform.Find("Layer2_Predios");
            
        // Salva as posições iniciais
        if (layerNuvens != null)
            posicaoInicialNuvens = layerNuvens.position;
            
        if (layerPredios != null)
            posicaoInicialPredios = layerPredios.position;
    }
    
    void Update()
    {
        // Move as nuvens (mais devagar, fundo distante)
        if (layerNuvens != null)
        {
            layerNuvens.position += Vector3.left * (velocidadeBase * multiplicadorNuvens * Time.deltaTime);
            
            // Reset quando sair muito da tela
            if (layerNuvens.position.x < posicaoInicialNuvens.x - limiteHorizontal)
            {
                layerNuvens.position = posicaoInicialNuvens;
            }
        }
        
        // Move os prédios (mais rápido, mais próximo)
        if (layerPredios != null)
        {
            layerPredios.position += Vector3.left * (velocidadeBase * multiplicadorPredios * Time.deltaTime);
            
            // Reset quando sair muito da tela
            if (layerPredios.position.x < posicaoInicialPredios.x - limiteHorizontal)
            {
                layerPredios.position = posicaoInicialPredios;
            }
        }
    }
    
    // Método para parar/continuar a animação se necessário
    public void SetAnimacaoAtiva(bool ativa)
    {
        enabled = ativa;
    }
}
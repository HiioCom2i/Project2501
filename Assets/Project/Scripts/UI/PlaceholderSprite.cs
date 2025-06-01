using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlaceholderSprite : MonoBehaviour
{
    [Header("Tipo de Forma")]
    public TipoForma tipoForma = TipoForma.Retangulo;
    
    [Header("Configurações Visuais")]
    public Color corPrincipal = Color.white;
    public bool usarGradiente = false;
    public Color corGradienteTopo = Color.white;
    public Color corGradienteBase = Color.gray;
    
    public enum TipoForma
    {
        Retangulo,
        Elipse,
        Nuvem,
        Predio
    }
    
    private SpriteRenderer spriteRenderer;
    private Texture2D texturaGerada;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GerarSpritePlaceholder();
    }
    
    void GerarSpritePlaceholder()
    {
        // Tamanho padrão da textura
        int largura = 64;
        int altura = 64;
        
        texturaGerada = new Texture2D(largura, altura);
        
        switch(tipoForma)
        {
            case TipoForma.Retangulo:
                GerarRetangulo(largura, altura);
                break;
            case TipoForma.Elipse:
                GerarElipse(largura, altura);
                break;
            case TipoForma.Nuvem:
                GerarNuvem(largura, altura);
                break;
            case TipoForma.Predio:
                GerarPredio(largura, altura);
                break;
        }
        
        texturaGerada.Apply();
        
        // Criar sprite a partir da textura
        Sprite novoSprite = Sprite.Create(
            texturaGerada, 
            new Rect(0, 0, largura, altura),
            new Vector2(0.5f, 0.5f),
            100f // pixels por unidade
        );
        
        spriteRenderer.sprite = novoSprite;
    }
    
    void GerarRetangulo(int largura, int altura)
    {
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                Color cor = corPrincipal;
                
                if (usarGradiente)
                {
                    float t = (float)y / altura;
                    cor = Color.Lerp(corGradienteBase, corGradienteTopo, t);
                }
                
                texturaGerada.SetPixel(x, y, cor);
            }
        }
    }
    
    void GerarElipse(int largura, int altura)
    {
        Vector2 centro = new Vector2(largura / 2f, altura / 2f);
        float raioX = largura / 2f;
        float raioY = altura / 2f;
        
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                float distX = (x - centro.x) / raioX;
                float distY = (y - centro.y) / raioY;
                float distancia = distX * distX + distY * distY;
                
                if (distancia <= 1f)
                {
                    Color cor = corPrincipal;
                    
                    if (usarGradiente)
                    {
                        float t = (float)y / altura;
                        cor = Color.Lerp(corGradienteBase, corGradienteTopo, t);
                    }
                    
                    // Suavizar bordas
                    float alpha = Mathf.Clamp01(1f - (distancia - 0.8f) * 5f);
                    cor.a *= alpha;
                    
                    texturaGerada.SetPixel(x, y, cor);
                }
                else
                {
                    texturaGerada.SetPixel(x, y, Color.clear);
                }
            }
        }
    }
    
    void GerarNuvem(int largura, int altura)
    {
        // Criar uma nuvem simples com círculos sobrepostos
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                texturaGerada.SetPixel(x, y, Color.clear);
            }
        }
        
        // Adicionar "bolhas" da nuvem
        AdicionarCirculo(largura * 0.3f, altura * 0.5f, largura * 0.25f);
        AdicionarCirculo(largura * 0.5f, altura * 0.6f, largura * 0.3f);
        AdicionarCirculo(largura * 0.7f, altura * 0.5f, largura * 0.25f);
        AdicionarCirculo(largura * 0.5f, altura * 0.4f, largura * 0.2f);
    }
    
    void GerarPredio(int largura, int altura)
    {
        // Base do prédio
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                Color cor = corPrincipal;
                
                if (usarGradiente)
                {
                    float t = (float)y / altura;
                    cor = Color.Lerp(corGradienteBase, corGradienteTopo, t);
                }
                
                // Adicionar "janelas" simples
                if (x % 16 >= 4 && x % 16 <= 12 && y % 16 >= 4 && y % 16 <= 12)
                {
                    cor *= 0.7f; // Janelas mais escuras
                }
                
                texturaGerada.SetPixel(x, y, cor);
            }
        }
    }
    
    void AdicionarCirculo(float centroX, float centroY, float raio)
    {
        int largura = texturaGerada.width;
        int altura = texturaGerada.height;
        
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                float distancia = Vector2.Distance(new Vector2(x, y), new Vector2(centroX, centroY));
                
                if (distancia <= raio)
                {
                    Color cor = corPrincipal;
                    float alpha = Mathf.Clamp01(1f - (distancia / raio - 0.8f) * 5f);
                    cor.a *= alpha;
                    
                    Color pixelAtual = texturaGerada.GetPixel(x, y);
                    Color novoPixel = Color.Lerp(pixelAtual, cor, cor.a);
                    texturaGerada.SetPixel(x, y, novoPixel);
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject menuGameOver;

    public void RecomecarJogo()
    {
        SceneManager.LoadScene("CenaDoJogo");
    }
    public void Sair()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    //Botões menu opções
    public void VoltarParaMenu()
    {
        SceneManager.LoadScene("CenaDoMenu");
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseController : MonoBehaviour
{
    public GameObject menuPause;
    public GameObject menuPauseOpcoes;

    void Start(){}
    void Update(){}

    //Botões menu principal
    public void ResumirJogo()
    {
        // menuPause.SetActive(false);
        // menuPauseOpcoes.SetActive(false);
        Time.timeScale = 1; // volta o tempo ao normal
    }

    public void Opcoes()
    {
        menuPause.SetActive(false);   
        menuPauseOpcoes.SetActive(true); 
    }


    public void VoltarMenuPrincipal()
    {
        // TODO - botar o nome da cena do menu do jogo aq
        // SceneManager.LoadScene();

    }

    //Botões menu pause opções
    public void VoltarMenuPause()
    {
        menuPauseOpcoes.SetActive(false);
        menuPause.SetActive(true);   
    }

    // TODO - função para controlar o volume
    // public void ControlarVolume(){}

}

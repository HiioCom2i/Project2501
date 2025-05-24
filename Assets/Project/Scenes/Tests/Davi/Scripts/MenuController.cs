using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject menuPrincipal;
    public GameObject menuOpcoes;

    void Start(){}
    void Update(){}


    //Botões menu principal
    public void ComecarJogo()
    {
        SceneManager.LoadScene("CenaSeiLa");
    }

    public void Opcoes()
    {
        menuOpcoes.SetActive(true); 
        menuPrincipal.SetActive(false); 
    }


    public void Sair()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    //Botões menu opções
    public void VoltarParaMenu()
    {
        menuOpcoes.SetActive(false);
        menuPrincipal.SetActive(true);
    }
}

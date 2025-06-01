using UnityEngine;

public class MusicaController : MonoBehaviour
{
    public AudioSource musica;
    public AudioClip intro;
    public AudioClip loop;

    void Start()
    {
        if (musica != null && intro != null && loop != null)
        {
            musica.loop = false;
            musica.clip = intro;
            musica.Play();
            Invoke(nameof(TocarLoop), intro.length);
        }
    }

    void TocarLoop()
    {
        musica.clip = loop;
        musica.loop = true;
        musica.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

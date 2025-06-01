using UnityEngine;

public class OpcoesController : MonoBehaviour
{
    public AudioSource musica;
    public AudioClip introClip;
    public AudioClip loopClip;

    void Start()
    {
        if (musica != null && introClip != null && loopClip != null)
        {
            musica.loop = false;
            musica.clip = introClip;
            musica.Play();
            Invoke(nameof(TocarLoop), introClip.length);
        }
    }

    void TocarLoop()
    {
        musica.clip = loopClip;
        musica.loop = true;
        musica.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

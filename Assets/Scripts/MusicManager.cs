using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip menuClip;
    public AudioClip gameClip;
    //tiempo de fundido entre pistas de audio
    [Range(1, 3)]
    public float fadeTime = 2f;
    private Coroutine changeClipCoroutine;
    
    public static MusicManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // en caso de que ya exista una instancia, para evitar solapamientos, autodestruiremos la nueva instancia
            Destroy(gameObject);
        }
        
        //esto hara que la instancia no sea destruida entre escenas
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        
        
    }

    /// <summary>
    /// Reproduce el audio de menu
    /// </summary>
    public void PlayMainMenu()
    {
        if (audioSource.clip == menuClip) return;
        if (changeClipCoroutine != null) StopCoroutine(changeClipCoroutine);
        changeClipCoroutine = StartCoroutine(ChangeClip(menuClip));
    }

    /// <summary>
    /// Reproduce el audio de game
    /// </summary>
    public void PlayGame()
    {
        if(audioSource.clip == gameClip) return;
        if (changeClipCoroutine != null) StopCoroutine(changeClipCoroutine);
        changeClipCoroutine = StartCoroutine(ChangeClip(gameClip));
    }


    /// <summary>
    /// corrutina que cambia de clip al cambiar la escena
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public IEnumerator ChangeClip(AudioClip clip)
    {

       

        ////relizamos el cambio de clip al que recibimos como parametro
        audioSource.clip = clip;
        //Iniciamos la reproduccion ya que el cambio de clip la detiene
        audioSource.Play();
        yield return null;

        
    }
    
}

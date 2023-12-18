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



    public IEnumerator ChangeClip(AudioClip clip)
    {

        ////usaremos el contador con la mitad del tiempo, ya que deberemos hacer el fundido de salida y de entrada.
        //float counter = fadeTime / 2;
        //while (counter > 0)
        //{
        //    Debug.Log(OptionsManager.instance.musicSlider.value);

        //    //vamos reduciendo el volumen
        //    audioSource.volume = counter / OptionsManager.instance.musicSlider.value;
        //    Debug.Log(audioSource.volume);
        //    //reducimos el contador
        //    counter -= Time.deltaTime;
        //    yield return null;
        //}

        ////relizamos el cambio de clip al que recibimos como parametro
        audioSource.clip = clip;
        //Iniciamos la reproduccion ya que el cambio de clip la detiene
        audioSource.Play();
        yield return null;

        //while (counter < fadeTime / 2)
        //{

        //    audioSource.volume = counter / OptionsManager.instance.musicSlider.value;
            
        //    counter += Time.deltaTime;

        //    yield return null;
        //}
    }
    
}

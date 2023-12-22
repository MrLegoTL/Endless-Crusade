using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
  
    public Slider musicSlider;
    public Toggle fullScreenToggle;
    public static OptionsManager instance;

    private void Start()
    {
        musicSlider.value = MusicManager.instance.audioSource.volume;
        if (Screen.fullScreen)
        {
            fullScreenToggle.isOn = true;
        }
        else
        {
            fullScreenToggle.isOn=false;
        }
        
    }
    /// <summary>
    /// Metodo que cambia el volumen con el slider
    /// </summary>
    public void ChangeVolumen()
    {
        float newVolumen = musicSlider.value;
        SetVolumen(newVolumen);

    }
    /// <summary>
    /// metodo para obteenr el volumen
    /// </summary>
    /// <param name="volumen"></param>
    private void SetVolumen(float volumen)
    {
       MusicManager.instance.audioSource.volume = volumen;
    }

    /// <summary>
    /// Metdodo para activa la opcion de pantalla completa
    /// </summary>
    /// <param name="fullScreen"></param>
    public void ActiveFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }
 
}

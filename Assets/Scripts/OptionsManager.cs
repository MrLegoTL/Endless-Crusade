using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
  
    public Slider musicSlider;
    public static OptionsManager instance;
    private void Start()
    {
        musicSlider.value = MusicManager.instance.audioSource.volume;
        
    }
    public void ChangeVolumen()
    {
        float newVolumen = musicSlider.value;
        SetVolumen(newVolumen);

    }
    private void SetVolumen(float volumen)
    {
       MusicManager.instance.audioSource.volume = volumen;
    }

}

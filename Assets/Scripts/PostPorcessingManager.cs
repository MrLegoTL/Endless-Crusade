using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PostPorcessingManager : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume _postProcessVolume;
    private ChromaticAberration _chromaticAberration;
    private Color _bloomColor = new Color(49f, 0f, 255f);
    private Bloom _bloom;
    public float timePostProcessing;
  

    private void Start()
    {
        _postProcessVolume =  GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
        _postProcessVolume.profile.TryGetSettings(out _bloom);

    }
  
    public void ActivePowerPostProcess()
    {
        //Chromatic Aberration
        _chromaticAberration.active = true;        
        _chromaticAberration.intensity.value = 0.5f;
        //Bloom
        _bloom.active = true;
        _bloom.intensity.value = 40;
        _bloom.threshold.value = 0.8f;
        _bloom.diffusion.value = 4.5f;
        _bloom.anamorphicRatio.value = 1f;
        _bloom.color.value = new Color(0.19f,0,1,1);


    }

    public void DesactivePowerPostProcess()
    {
        _chromaticAberration.active = false;
        _chromaticAberration.intensity.value = 0;
        _bloom.active = false;
    }
    
    public void ActiveImmunityPostProcess()
    {
        //Chromatic Aberration
        _chromaticAberration.active = true;
        _chromaticAberration.intensity.value = 0.5f;
        //Bloom
        _bloom.active = true;
        _bloom.intensity.value = 40;
        _bloom.threshold.value = 0.8f;
        _bloom.diffusion.value = 4.5f;
        _bloom.anamorphicRatio.value = 1f;
        _bloom.color.value = new Color(0, 1, 1, 1);
    }

    public void DesactiveImmunityPostProcess()
    {
        _chromaticAberration.active = false;
        _chromaticAberration.intensity.value = 0;
        _bloom.active = false;
    }
    
}

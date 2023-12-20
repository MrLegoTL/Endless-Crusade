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
    public float timePostProcessing;
  

    private void Start()
    {
        _postProcessVolume =  GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _chromaticAberration);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _chromaticAberration.active= false;
        }
    }
    public void ActivePowerPostProcess()
    {
     
            _chromaticAberration.active =true;
            //_chromaticAberration.intensity.value = 0.5f;
        
       
        
        
    }
    public void DesactivePowerPostProcess()
    {
        _chromaticAberration.active = false;
        _chromaticAberration.intensity.value = 0;
    }
    
    
}

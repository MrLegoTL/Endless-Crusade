using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostPorcessingManager : MonoBehaviour
{
    private PostProcessVolume _postProcessVolume;
    private AmbientOcclusion _ambientOcclusion;

    private float myValue;

    private void Start()
    {
        _postProcessVolume =  GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _ambientOcclusion);
    }

    public void AmbientOcclusionOnOff(bool on)
    {
        if (on)
        {
            _ambientOcclusion.active = true;
        }
        else
        {
            _ambientOcclusion.active = false;
        }
    }

    public void GeneralSettings()
    {
        _ambientOcclusion.mode.value = AmbientOcclusionMode.MultiScaleVolumetricObscurance;
        _ambientOcclusion.intensity.value = myValue;
        _ambientOcclusion.radius.value = myValue;
        _ambientOcclusion.quality.value = AmbientOcclusionQuality.Medium;
        _ambientOcclusion.color.value = Color.blue;
        _ambientOcclusion.ambientOnly.value = true;
    }
}

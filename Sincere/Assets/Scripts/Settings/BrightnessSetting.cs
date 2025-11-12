using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessSetting : MonoBehaviour
{
    public Slider brightnessSlider;
    public Volume postProcessingVolume;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
            brightnessSlider.value = savedBrightness;
            SetBrightness(savedBrightness);
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    void SetBrightness(float value)
    {
        BrightnessManager.instance?.ApplyBrightness(value);
    }
}
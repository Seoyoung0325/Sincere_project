using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/*public class BrightnessSetting : MonoBehaviour
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
        // Post Exposure는 -2 ~ 2 정도 범위 추천
        colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
        PlayerPrefs.SetFloat("Brightness", value);
    }
}*/
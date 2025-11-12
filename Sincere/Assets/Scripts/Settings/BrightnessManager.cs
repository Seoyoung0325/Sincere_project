using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager instance;

    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (postProcessingVolume != null)
            {
                DontDestroyOnLoad(postProcessingVolume.gameObject); 
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
            ApplyBrightness(savedBrightness);
        }
        else
        {
            Debug.LogWarning("ColorAdjustments를 찾을 수 없습니다.");
        }
    }

    public void ApplyBrightness(float value)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
            PlayerPrefs.SetFloat("Brightness", value);
        }
    }

    public float GetBrightness()
    {
        return PlayerPrefs.GetFloat("Brightness", 0.5f);
    }
}
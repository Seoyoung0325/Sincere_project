using UnityEngine;
using UnityEngine.UI;

public class EffectsSetting : MonoBehaviour
{
    public Slider effectsSlider;

    void Start()
    {
        // 저장된 값 불러오기 (없으면 기본값 0.5)
        float savedVolume = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);
        effectsSlider.value = savedVolume;

        // 초기 적용
        SetEffectsVolume(savedVolume);

        // 슬라이더 값 변경 시 적용
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
    }

    void SetEffectsVolume(float value)
    {
        // 싱글톤 매니저에 전달
        EffectSoundManager manager = FindObjectOfType<EffectSoundManager>();
        if (manager != null)
        {
            manager.SetVolume(value);
        }

        // 저장
        PlayerPrefs.SetFloat("EffectsVolume", value);
        Debug.Log("효과음 설정 완료: " + (int)(value * 100));
    }
}
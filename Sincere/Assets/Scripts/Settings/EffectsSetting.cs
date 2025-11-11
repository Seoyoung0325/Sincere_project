using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectsSetting : MonoBehaviour
{
    public Slider effectsSlider;             // 효과음 볼륨 조절 슬라이더
    //public AudioSource[] effectsSources;     // 효과음 오디오 소스 배열 - 사운드가 없으므로 관련 코드 주석 처리

    void Start()
    {
        // 슬라이더 값이 변경될 때마다 SetEffectsVolume 함수 호출
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
    }

    void SetEffectsVolume(float value)
    {
        // 모든 효과음 오디오 소스의 볼륨을 슬라이더 값으로 설정
        /*foreach (AudioSource effects in effectsSources)
        {
            effects.volume = value;
        }*/
        float result = value * 100;
        Debug.Log("효과음 설정 완료: " + (int)result);
    }

}

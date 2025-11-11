using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmSetting : MonoBehaviour
{
    public Slider bgmSlider;        // 볼륨 조절 슬라이더
    //public AudioSource bgmSource;   // BGM 오디오 - 사운드가 없으므로 관련 코드 주석 처리

    void Start()
    {
        // 슬라이더가 변경될 때마다 SetBGMVolume 함수 호출
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    void SetBGMVolume(float value)
    {
        //bgmSource.volume = value; // value는 0~1 범위
        float result = value * 100;
        Debug.Log("배경음 설정 완료: " + (int)result);
    }
}


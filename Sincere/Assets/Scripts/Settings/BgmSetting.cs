using UnityEngine;
using UnityEngine.UI;

public class BgmSetting : MonoBehaviour
{
    public Slider bgmSlider; // BGM 볼륨 조절 슬라이더

    void Start()
    {
        // 저장된 BGM 볼륨 값을 불러와서 슬라이더에 적용 (기본값 0.5)
        float savedVolume = PlayerPrefs.GetFloat("BgmVolume", 0.5f);
        bgmSlider.value = savedVolume;

        // 초기 볼륨 적용
        SetBGMVolume(savedVolume);

        // 슬라이더 값이 변경될 때마다 SetBGMVolume 함수 호출
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    void SetBGMVolume(float value)
    {
        // BgmManager 싱글톤을 찾아서 볼륨 설정 전달
        BgmManager manager = FindObjectOfType<BgmManager>();
        if (manager != null)
        {
            manager.SetVolume(value);
        }

        // 설정값 저장
        PlayerPrefs.SetFloat("BgmVolume", value);

        // 디버그 출력
        Debug.Log("배경음 설정 완료: " + (int)(value * 100));
    }
}
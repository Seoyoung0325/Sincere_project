using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BgmManager : MonoBehaviour
{
    public Slider bgmSlider;          // BGM 조절용 슬라이더
    public TextMeshProUGUI volumeText; // 현재 볼륨 표시
    //public AudioSource bgmSource;      // BGM 오디오 소스

    private void Awake()
    {
        // 게임 내내 유지
        DontDestroyOnLoad(gameObject);

        // 슬라이더 기본 설정
        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 10;
        bgmSlider.wholeNumbers = true; // 정수만 선택 가능

        // 저장된 값 불러오기
        int savedValue = PlayerPrefs.GetInt("BGMVolume", 5);
        bgmSlider.value = savedValue;

        // 값 변경 이벤트 연결
        bgmSlider.onValueChanged.AddListener(OnSliderChanged);

        // 초기 텍스트 갱신
        UpdateVolumeText(savedValue);
    }

    public void OnSliderChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);

        // 로그 출력
        Debug.Log($"BGM 볼륨 변경: {intValue}");

        // 적용 (아직 오디오가 없으므로 주석 처리)
     /* AudioSource bgmSource = GetComponent<AudioSource>();
        if (bgmSource != null)
        {
            bgmSource.volume = intValue / 10f; 
        }*/

        // 값 저장
        PlayerPrefs.SetInt("BGMVolume", intValue);
        PlayerPrefs.Save();

        // 텍스트 갱신
        UpdateVolumeText(intValue);
    }

    private void UpdateVolumeText(int value)
    {
        if (volumeText != null)
        {
            volumeText.text = $"{value}";
        }
    }

    public int GetCurrentVolume()
    {
        return Mathf.RoundToInt(bgmSlider.value);
    }
}
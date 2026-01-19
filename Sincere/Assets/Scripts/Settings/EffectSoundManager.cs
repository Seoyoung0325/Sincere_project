using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectSoundManager : MonoBehaviour
{
    // public AudioSource effectSource;  // 오디오 소스가 아직 없으므로 주석 처리
    public Slider effectSlider;          // 효과음 볼륨 조절 슬라이더
    public TextMeshProUGUI volumeText;   // 과음 볼륨 표시

    private static EffectSoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 슬라이더 기본 설정
            effectSlider.minValue = 0;
            effectSlider.maxValue = 10;
            effectSlider.wholeNumbers = true; // 정수 단계만 선택 가능

            // 저장된 값 불러오기 (없으면 기본값 5)
            int savedValue = PlayerPrefs.GetInt("EffectsVolume", 5);
            effectSlider.value = savedValue;

            // 슬라이더 이벤트 연결
            effectSlider.onValueChanged.AddListener(OnSliderChanged);

            // 초기 텍스트 갱신
            UpdateVolumeText(savedValue);

            // 적용 
            // if (effectSource != null)
            //     effectSource.volume = savedValue / 10f;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSliderChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);

        // 로그 출력
        Debug.Log($"효과음 볼륨 변경: {intValue}");

        // 적용 
        // if (effectSource != null)
        //     effectSource.volume = intValue / 10f;

        // 값 저장
        PlayerPrefs.SetInt("EffectsVolume", intValue);
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

    public void PlayEffect(AudioClip clip)
    {
        // 재생
        // if (effectSource != null)
        // {
        //     effectSource.PlayOneShot(clip);
        // }
    }

    public int GetCurrentVolume()
    {
        return Mathf.RoundToInt(effectSlider.value);
    }
}
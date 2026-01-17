using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI brightnessText;

    [Header("Post Processing")]
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    // 밝기 단계 (0~3)
    private readonly float[] brightnessLevels = { 0.0f, 0.25f, 0.5f, 0.75f };
    private int currentIndex = 0; // 기본값: 첫 번째 단계

    private void Awake()
    {
        // 싱글톤 보장
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (postProcessingVolume != null)
            {
                DontDestroyOnLoad(postProcessingVolume.gameObject);

                if (postProcessingVolume.profile.TryGet(out colorAdjustments))
                {
                    // 저장된 인덱스 불러오기 (없으면 0)
                    currentIndex = PlayerPrefs.GetInt("BrightnessIndex", 0);
                    ApplyBrightness();
                }
            }
            else
            {
                Debug.LogWarning("PostProcessing Volume이 연결되지 않았습니다.");
            }
        }
        else
        {
            Destroy(gameObject); // 중복 방지
        }
    }

    // Inspector에서 Button OnClick 이벤트로 직접 연결
    public void OnLeftClick()
    {
        currentIndex = (currentIndex - 1 + brightnessLevels.Length) % brightnessLevels.Length;
        ApplyBrightness();
    }

    public void OnRightClick()
    {
        currentIndex = (currentIndex + 1) % brightnessLevels.Length;
        ApplyBrightness();
    }

    private void ApplyBrightness()
    {
        if (colorAdjustments != null)
        {
            float value = brightnessLevels[currentIndex];
            // postExposure 값은 -2 ~ 2 범위에서 보정
            colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);

            // 인덱스로 저장
            PlayerPrefs.SetInt("BrightnessIndex", currentIndex);
            PlayerPrefs.Save();

            // 텍스트 표시
            if (brightnessText != null)
            {
                brightnessText.text = $"밝기 {currentIndex + 1}/{brightnessLevels.Length}";
                // 또는 퍼센트로 표시하고 싶으면:
                // brightnessText.text = $"{Mathf.RoundToInt(value * 100)}%";
            }

            Debug.Log($"현재 인덱스: {currentIndex}, 밝기값: {value}");
        }
    }
}
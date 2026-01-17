using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager instance;

    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    public Button leftButton;              // 왼쪽 버튼
    public Button rightButton;             // 오른쪽 버튼
    public TextMeshProUGUI brightnessText; // 현재 단계 표시용

    // 밝기 단계 (1~4)
    private readonly float[] brightnessLevels = { 0.0f, 0.33f, 0.66f, 0.75f };
    private int currentIndex = 1; // 기본값: 2단계

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (postProcessingVolume != null)
            {
                DontDestroyOnLoad(postProcessingVolume.gameObject);

                if (postProcessingVolume.profile.TryGet(out colorAdjustments))
                {
                    float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.33f);
                    currentIndex = FindClosestIndex(savedBrightness);
                    ApplyBrightness();
                }
            }
            else
            {
                Debug.LogWarning("PostProcessing Volume이 연결되지 않았습니다.");
            }

            // 버튼 이벤트 연결
            if (leftButton != null) leftButton.onClick.AddListener(OnLeftClick);
            if (rightButton != null) rightButton.onClick.AddListener(OnRightClick);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnLeftClick()
    {
        currentIndex = (currentIndex - 1 + brightnessLevels.Length) % brightnessLevels.Length;
        ApplyBrightness();
    }

    private void OnRightClick()
    {
        currentIndex = (currentIndex + 1) % brightnessLevels.Length;
        ApplyBrightness();
    }

    private void ApplyBrightness()
    {
        if (colorAdjustments != null)
        {
            float value = brightnessLevels[currentIndex];
            colorAdjustments.postExposure.value = Mathf.Lerp(-2f, 2f, value);
            PlayerPrefs.SetFloat("Brightness", value);

            if (brightnessText != null)
                brightnessText.text = $"{currentIndex + 1}";
        }
    }

    private int FindClosestIndex(float value)
    {
        int closest = 0;
        float minDiff = Mathf.Abs(brightnessLevels[0] - value);
        for (int i = 1; i < brightnessLevels.Length; i++)
        {
            float diff = Mathf.Abs(brightnessLevels[i] - value);
            if (diff < minDiff)
            {
                minDiff = diff;
                closest = i;
            }
        }
        return closest;
    }
}
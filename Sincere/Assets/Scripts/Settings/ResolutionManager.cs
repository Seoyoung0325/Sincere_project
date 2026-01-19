using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ResolutionManager : MonoBehaviour
{
    public TextMeshProUGUI resolutionText;
    public Button leftButton;
    public Button rightButton;

    private Resolution[] supportedResolutions;
    private int currentIndex = 0;

    private void Awake()
    {
        // 추후 일부 해상도만 추릴 예정
        supportedResolutions = Screen.resolutions
            .GroupBy(r => new { r.width, r.height }) // 중복 제거
            .Select(g => g.First())
            .ToArray();

        foreach (var r in supportedResolutions)
        {
            Debug.Log($"{r.width} x {r.height} @ {r.refreshRateRatio.value}Hz");
        }

        // 저장된 값 불러오기
        currentIndex = Mathf.Clamp(PlayerPrefs.GetInt("ResolutionIndex", 0), 0, supportedResolutions.Length - 1);

        // 버튼 이벤트 연결
        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);

        ApplyResolution();
        UpdateText();
    }

    public void OnLeftClick()
    {
        currentIndex = (currentIndex - 1 + supportedResolutions.Length) % supportedResolutions.Length;
        ApplyResolution();
        SaveSettings();
        UpdateText();
    }

    public void OnRightClick()
    {
        currentIndex = (currentIndex + 1) % supportedResolutions.Length;
        ApplyResolution();
        SaveSettings();
        UpdateText();
    }

    private void ApplyResolution()
    {
        var res = supportedResolutions[currentIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        Debug.Log($"해상도 변경: {res.width}x{res.height}");
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", currentIndex);
        PlayerPrefs.Save();
    }

    private void UpdateText()
    {
        if (resolutionText != null)
        {
            var res = supportedResolutions[currentIndex];
            resolutionText.text = $"{res.width} x {res.height}";
        }
    }
}
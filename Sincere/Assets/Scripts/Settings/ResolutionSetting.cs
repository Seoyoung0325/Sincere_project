using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionSetting : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private List<(Resolution, FullScreenMode)> resolutionOptions = new List<(Resolution, FullScreenMode)>();

    void Start()
    {
        resolutionDropdown.ClearOptions();
        Resolution[] resolutions = Screen.resolutions;
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        // 해상도 + 창모드 조합 생성
        foreach (Resolution res in resolutions)
        {
            AddOption(res, FullScreenMode.ExclusiveFullScreen, "전체 화면", options);
            AddOption(res, FullScreenMode.Windowed, "창 모드", options);
            AddOption(res, FullScreenMode.FullScreenWindow, "전체 화면 창", options);
        }

        resolutionDropdown.AddOptions(options);

        // 저장된 인덱스 불러오기
        int savedIndex = PlayerPrefs.GetInt("ResolutionComboIndex", 0);
        resolutionDropdown.value = savedIndex;
        ApplyResolution(savedIndex);

        resolutionDropdown.onValueChanged.AddListener(ApplyResolution);

    }

    void AddOption(Resolution res, FullScreenMode mode, string label, List<TMP_Dropdown.OptionData> options)
    {
        string optionText = $"{res.width}x{res.height} ({label})";
        options.Add(new TMP_Dropdown.OptionData(optionText));
        resolutionOptions.Add((res, mode));
    }

    void ApplyResolution(int index)
    {
        if (index >= 0 && index < resolutionOptions.Count)
        {
            var (res, mode) = resolutionOptions[index];
            Screen.SetResolution(res.width, res.height, mode);

            PlayerPrefs.SetInt("ResolutionComboIndex", index);
            Debug.Log($"해상도 적용: {res.width}x{res.height}, 모드: {mode}");
        }
    }
}
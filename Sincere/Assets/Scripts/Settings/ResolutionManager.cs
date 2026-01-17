using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionManager : MonoBehaviour
{
    public TextMeshProUGUI resolutionText;
    public Button leftButton;     // 왼쪽 버튼
    public Button rightButton;    // 오른쪽 버튼

    // 지원하는 해상도 목록
    private Vector2Int[] resolutionSizes = {
        new Vector2Int(1280, 720),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440)
    };

    // 현재 선택된 해상도 인덱스
    private int currentIndex = 2; // 기본값: 1920x1080

    private void Awake()
    {
        // 게임 내내 유지
        DontDestroyOnLoad(gameObject);

        // 저장된 설정 불러오기 (없으면 기본값 사용)
        currentIndex = PlayerPrefs.GetInt("ResolutionIndex", 2);

        // 버튼 이벤트 연결
        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);

        // 해상도 적용
        ApplyResolution();
        UpdateText();
    }

    private void OnLeftClick()
    {
        // 왼쪽 버튼 클릭 시 해상도 인덱스 감소
        currentIndex = (currentIndex - 1 + resolutionSizes.Length) % resolutionSizes.Length;
        ApplyResolution();
        SaveSettings();
        UpdateText();
    }

    private void OnRightClick()
    {
        // 오른쪽 버튼 클릭 시 해상도 인덱스 증가
        currentIndex = (currentIndex + 1) % resolutionSizes.Length;
        ApplyResolution();
        SaveSettings();
        UpdateText();
    }

    private void ApplyResolution()
    {
        // 해상도 적용 
        var res = resolutionSizes[currentIndex];
        Screen.SetResolution(res.x, res.y, Screen.fullScreen);

        Debug.Log($"해상도 변경: {res.x}x{res.y}");
    }

    private void SaveSettings()
    {
        // 현재 설정 저장
        PlayerPrefs.SetInt("ResolutionIndex", currentIndex);
        PlayerPrefs.Save();
    }

    private void UpdateText()
    {
        // UI 텍스트 업데이트
        if (resolutionText != null)
        {
            var res = resolutionSizes[currentIndex];
            resolutionText.text = res.x + "x" + res.y;
        }
    }
}
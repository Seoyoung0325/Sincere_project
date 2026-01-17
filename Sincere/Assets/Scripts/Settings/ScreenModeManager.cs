using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenModeManager : MonoBehaviour
{
    public Button windowedButton;     // 창 모드 버튼
    public Button fullscreenButton;   // 전체 화면 버튼

    private bool isFullScreen = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 유지

        // 버튼 이벤트 연결
        windowedButton.onClick.AddListener(SetWindowed);
        fullscreenButton.onClick.AddListener(SetFullscreen);

        // 이전 실행에서 저장된 모드 불러오기
        isFullScreen = PlayerPrefs.GetInt("ScreenMode", 0) == 1;
        ApplyScreenMode();
        UpdateButtonVisuals();
    }

    private void SetWindowed()
    {
        isFullScreen = false;
        ApplyScreenMode();
        PlayerPrefs.SetInt("ScreenMode", 0); // 저장
        PlayerPrefs.Save();
        UpdateButtonVisuals();
        Debug.Log("화면 모드 변경: 창 모드");
    }

    private void SetFullscreen()
    {
        isFullScreen = true;
        ApplyScreenMode();
        PlayerPrefs.SetInt("ScreenMode", 1); // 저장
        PlayerPrefs.Save();
        UpdateButtonVisuals();
        Debug.Log("화면 모드 변경: 전체 화면");
    }

    public void ApplyScreenMode()
    {
        Screen.fullScreen = isFullScreen;
    }

    public void UpdateButtonVisuals()
    {
        // 버튼 색상으로 현재 모드 표시
        Color selected = new Color(0.8f, 0.8f, 0.8f, 1f);
        Color normal = Color.white;

        if (windowedButton != null && fullscreenButton != null)
        {
            windowedButton.image.color = isFullScreen ? normal : selected;
            fullscreenButton.image.color = isFullScreen ? selected : normal;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private static ResolutionManager instance;

    // 현재 선택된 해상도 인덱스
    public int currentResolutionIndex;

    // 현재 선택된 화면 모드 (전체 화면, 창 모드 등)
    public FullScreenMode currentScreenMode;

    void Awake()
    {
        // 하나만 유지하고 중복 생성 방지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트 유지

            // 저장된 설정 불러오기 (기본값: 해상도 0번, 전체 화면 창)
            currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
            currentScreenMode = (FullScreenMode)PlayerPrefs.GetInt("ScreenMode", (int)FullScreenMode.FullScreenWindow);

            // 해상도 및 화면 모드 적용
            ApplyResolution(currentResolutionIndex, currentScreenMode);
        }
        else
        {
            Destroy(gameObject); // 중복 제거
        }
    }

    // 해상도 및 화면 모드 적용 함수
    public void ApplyResolution(int resolutionIndex, FullScreenMode screenMode)
    {
        Resolution[] resolutions = Screen.resolutions;

        // 유효한 인덱스인지 확인
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            Resolution res = resolutions[resolutionIndex];

            // 실제 해상도 및 화면 모드 적용
            Screen.SetResolution(res.width, res.height, screenMode);

            // 현재 설정 저장
            currentResolutionIndex = resolutionIndex;
            currentScreenMode = screenMode;

            // PlayerPrefs에 저장
            PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
            PlayerPrefs.SetInt("ScreenMode", (int)screenMode);
        }
    }
}
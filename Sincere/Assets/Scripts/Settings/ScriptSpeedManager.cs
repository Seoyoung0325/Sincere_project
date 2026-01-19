using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptSpeedManager : MonoBehaviour
{
    public TextMeshProUGUI speedText; // 현재 속도 표시용
    public Button leftButton;         // 왼쪽 버튼
    public Button rightButton;        // 오른쪽 버튼

    private string[] speedLevels = { "느림", "보통", "빠름" };
    private int currentIndex = 1; // 보통을 기본값으로 설정

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);

        // 저장된 값 불러오기
        currentIndex = PlayerPrefs.GetInt("SpeedIndex", 1);
        ApplySpeed();
    }

    public void OnLeftClick()
    {
        currentIndex = (currentIndex - 1 + speedLevels.Length) % speedLevels.Length;
        ApplySpeed();
    }

    public void OnRightClick()
    {
        currentIndex = (currentIndex + 1) % speedLevels.Length;
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        // 260118 기준 표시되는 것만 바뀌도록 설정(추후 실제 속도도 바뀌게 수정 예정)
        if (speedText != null)
            speedText.text = speedLevels[currentIndex];

        PlayerPrefs.SetInt("SpeedIndex", currentIndex);
        PlayerPrefs.Save();

        // 로그 출력
        Debug.Log($"속도 변경: {speedLevels[currentIndex]}");
    }

    public string GetCurrentSpeed()
    {
        return speedLevels[currentIndex];
    }
}
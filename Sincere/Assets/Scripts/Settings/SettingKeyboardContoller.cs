using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsKeyboardController : MonoBehaviour
{
    public TextMeshProUGUI[] menuLabels;

    public Button screenModeLeftButton;
    public Button screenModeRightButton;

    public Button resolutionLeftButton;
    public Button resolutionRightButton;

    public Button brightnessLeftButton;
    public Button brightnessRightButton;

    public Button scriptSpeedLeftButton;
    public Button scriptSpeedRightButton;

    public Slider bgmSlider;
    public Slider effectSlider;

    private int currentMenuIndex = 0;

    void Start()
    {
        currentMenuIndex = 0;
        HighlightCurrentMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMenuIndex = Mathf.Max(0, currentMenuIndex - 1);
            HighlightCurrentMenu();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMenuIndex = Mathf.Min(menuLabels.Length - 1, currentMenuIndex + 1);;
            HighlightCurrentMenu();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleLeftAction();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {;
            HandleRightAction();
        }
    }

    void HandleLeftAction()
    {
        switch (currentMenuIndex)
        {
            case 0: // 화면 모드
                screenModeLeftButton.onClick.Invoke();
                break;

            case 1: // 해상도
                resolutionLeftButton.onClick.Invoke();
                break;

            case 2: // 밝기
                brightnessLeftButton.onClick.Invoke();
                break;

            case 3: // 스크립트 속도
                scriptSpeedLeftButton.onClick.Invoke();
                break;

            case 4: // BGM
                bgmSlider.value = Mathf.Max(bgmSlider.minValue, bgmSlider.value - 1);
                break;

            case 5: // 효과음
                effectSlider.value = Mathf.Max(effectSlider.minValue, effectSlider.value - 1);
                break;
        }
    }

    void HandleRightAction()
    {
        switch (currentMenuIndex)
        {
            case 0: // 화면 모드
                screenModeRightButton.onClick.Invoke();
                break;

            case 1: // 해상도);
                resolutionRightButton.onClick.Invoke();
                break;

            case 2: // 밝기
                brightnessRightButton.onClick.Invoke();
                break;

            case 3: // 스크립트 속도
                scriptSpeedRightButton.onClick.Invoke();
                break;

            case 4: // BGM
                bgmSlider.value = Mathf.Min(bgmSlider.maxValue, bgmSlider.value + 1);
                break;

            case 5: // 효과음
                effectSlider.value = Mathf.Min(effectSlider.maxValue, effectSlider.value + 1);
                break;
        }
    }

    void HighlightCurrentMenu()
    {
        for (int i = 0; i < menuLabels.Length; i++)
        {
            if (menuLabels[i] == null)
            {
                Debug.LogWarning($"menuLabels[{i}]가 null입니다.");
                continue;
            }

            if (i == currentMenuIndex)
                menuLabels[i].color = Color.black;
            else
                menuLabels[i].color = Color.grey;
        }
    }

    public void OnMenuClicked(int index)
    {
        currentMenuIndex = index;
        HighlightCurrentMenu();
    }
}
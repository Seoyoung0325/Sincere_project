using UnityEngine;
using TMPro;

public class MenuSelector : MonoBehaviour
{
    public TextMeshProUGUI[] menuTexts;

    public Color selectedColor = Color.black;
    public Color defaultColor = Color.gray;

    // 버튼 OnClick 이벤트에서 호출
    public void OnMenuClick(TextMeshProUGUI clickedText)
    {
        // 모든 메뉴 텍스트를 기본 색으로 초기화
        foreach (var txt in menuTexts)
        {
            txt.color = defaultColor;
        }

        // 클릭된 메뉴 텍스트만 검은색으로 변경
        clickedText.color = selectedColor;
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueUI : MonoBehaviour
{
    public static ClueUI instance;

    public GameObject cluePanel;         // 단서용 판넬 
    public Text clueNameText;            // 단서 UI 텍스트
    public Image clueImage;              // 단서 UI 이미지

    public GameObject questionPanel;     // 의문점용 판넬
    public Text questionNameText;        // 의문점 UI 텍스트 

    public float displayTime = 1.0f;     // 화면 표출 시간

    private Coroutine clueRoutine;
    private Coroutine questionRoutine;

    private void Awake()
    {
        // 싱글톤 초기화 (중복 생성 방지)
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // 시작 시 모든 패널 숨겨두기
        if (cluePanel != null) cluePanel.SetActive(false);
        if (questionPanel != null) questionPanel.SetActive(false);
    }


    public void ShowNotification(ClueData clue)
    {
        if (cluePanel == null) return;

        // 단서 이름 출력
        if (clueNameText != null) clueNameText.text = clue.clueName;

        if (clueImage != null)
        {
            if (!string.IsNullOrEmpty(clue.clueImagePath))
            {
                Sprite sprite = Resources.Load<Sprite>(clue.clueImagePath);
                if (sprite != null)
                {
                    clueImage.sprite = sprite;
                    clueImage.gameObject.SetActive(true);
                }
                else clueImage.gameObject.SetActive(false);
            }
            else clueImage.gameObject.SetActive(false);
        }

        if (clueRoutine != null) StopCoroutine(clueRoutine);
        clueRoutine = StartCoroutine(ShowAndHideSequence(cluePanel));
    }

    public void ShowQuestionNotification(QuestionData question)
    {
        if (questionPanel == null) return;

        // 텍스트 설정
        if (questionNameText != null) questionNameText.text = question.questionName;

        if (questionRoutine != null) StopCoroutine(questionRoutine);
        questionRoutine = StartCoroutine(ShowAndHideSequence(questionPanel));
    }

    IEnumerator ShowAndHideSequence(GameObject targetPanel)
    {
        // 패널 켜기 
        targetPanel.SetActive(true);

        // 표시 시간 유지
        yield return new WaitForSeconds(displayTime);

        // 패널 끄기
        targetPanel.SetActive(false);
    }
}
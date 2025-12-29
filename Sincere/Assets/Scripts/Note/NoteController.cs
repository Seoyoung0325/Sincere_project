using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NoteController : MonoBehaviour
{
    public GameObject cluePanel;       // 단서 기록
    public GameObject questionPanel;   // 의문점
    public GameObject mapPanel;        // 지도

    [Header("단서 UI")]
    public Transform clueContent;      // 윤서영 추가 - 단서가 생성될 부모 (Scroll View의 Content)
    public GameObject cluePrefab;      // 윤서영 추가 - 단서 UI 프리팹

    [Header("의문점 UI")]
    public Transform questionContent;  // 윤서영 추가 - 의문점이 생성될 부모 (Scroll View의 Content)
    public GameObject questionPrefab;  // 윤서영 추가 - 의문점 UI 프리팹

    private int currentIndex = 0;      // 현재 섹션 인덱스
    private bool isOpen = false;       // 노트 열림 여부

    private static NoteController instance;

    void Start()
    {
        // 시작할 때는 모두 꺼둠
        cluePanel.SetActive(false);
        questionPanel.SetActive(false);
        mapPanel.SetActive(false);
    }

    void Awake()
    {
        // 윤서영 추가 - 불러오기 버그로 인해 싱글톤 삭제함
    }

    void Update()
    {
        // E키 입력
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleNotebook();
        }

        // 노트가 열려있을 때만 방향키 입력 처리
        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentIndex++;
                if (currentIndex > 2) currentIndex = 0; // 순환
                ShowSection(currentIndex);
            }
        }
    }

    // 버튼에서 호출할 함수
    public void OpenNotebook()
    {
        isOpen = true;
        Time.timeScale = 0; // 게임 캐릭터 안움직이게 처리
        currentIndex = 0;

        // 윤서영 추가 - 단서와 의문점 업데이트
        if (DataManager.instance != null)
        {
            List<ClueData> clues = DataManager.instance.GetAcquiredClues();
            List<QuestionData> questions = DataManager.instance.GetAcquiredQuestions();
        }
        UpdateClueList();
        UpdateQuestionList();

        ShowSection(currentIndex);
    }

    public void CloseNotebook()
    {
        isOpen = false;
        Time.timeScale = 1; // 게임 캐릭터가 다시 움직이게 함
        HideAllSections();
    }

    private void ToggleNotebook()
    {
        if (isOpen)
            CloseNotebook();
        else
            OpenNotebook();
    }

    private void ShowSection(int index)
    {
        HideAllSections();

        switch (index)
        {
            case 0:
                cluePanel.SetActive(true);
                UpdateClueList();
                break;
            case 1:
                questionPanel.SetActive(true);
                UpdateQuestionList();
                break;
            case 2:
                mapPanel.SetActive(true);
                break;
        }
    }

    private void HideAllSections()
    {
        cluePanel.SetActive(false);
        questionPanel.SetActive(false);
        mapPanel.SetActive(false);
    }




    // 윤서영 추가 - 단서 업데이트
    private void UpdateClueList()
    {
        if (clueContent == null || cluePrefab == null)
        {
            Debug.LogWarning("clueContent 또는 cluePrefab이 설정되지 않았습니다!");
            return;
        }

        // 기존 단서 UI 전부 삭제
        foreach (Transform child in clueContent)
        {
            Destroy(child.gameObject);
        }

        // DataManager에서 습득한 단서 가져오기
        if (DataManager.instance == null)
        {
            Debug.LogWarning("DataManager가 없습니다!");
            return;
        }

        List<ClueData> clues = DataManager.instance.GetAcquiredClues();

        // 각 단서마다 UI 생성
        foreach (ClueData clue in clues)
        {
            GameObject clueItem = Instantiate(cluePrefab, clueContent);

            Transform actualPrefab = clueItem.transform.Find("CluePrefab");
            if (actualPrefab == null)
            {
                // Canvas가 없으면 clueItem 자체 사용
                actualPrefab = clueItem.transform;
            }

            // 단서 UI 설정
            ClueItem clueItemScript = clueItem.GetComponent<ClueItem>();
            if (clueItemScript != null)
            {
                clueItemScript.Setup(clue);
            }
            else
            {
                // ClueItem 스크립트가 없으면 기본 Text 설정
                Text clueText = clueItem.GetComponentInChildren<Text>();
                if (clueText != null)
                {
                    clueText.text = $"{clue.clueName}\n{clue.clueDescription}";
                }
            }
        }
    }


    // 윤서영 추가 - 의문점 업데이트
    private void UpdateQuestionList()
    {
        if (questionContent == null || questionPrefab == null)
        {
            Debug.LogWarning("questionContent 또는 questionPrefab이 설정되지 않았습니다!");
            return;
        }

        // 기존 의문점 UI 전부 삭제
        foreach (Transform child in questionContent)
        {
            Destroy(child.gameObject);
        }

        // DataManager에서 습득한 의문점 가져오기
        if (DataManager.instance == null)
        {
            Debug.LogWarning("DataManager가 없습니다!");
            return;
        }
        List<QuestionData> questions = DataManager.instance.GetAcquiredQuestions();

        // 각 의문점마다 UI 생성
        foreach (QuestionData question in questions)
        {
            GameObject questionItem = Instantiate(questionPrefab, questionContent);

            Transform actualPrefab = questionItem.transform.Find("QuestionPrefab");
            if (actualPrefab == null)
            {
                actualPrefab = questionItem.transform;
            }

            // 의문점 UI 설정
            QuestionItem questionItemScript = questionItem.GetComponent<QuestionItem>();
            if (questionItemScript != null)
            {
                questionItemScript.Setup(question);
            }
            else
            {
                // QuestionItem 스크립트가 없으면 기본 Text 설정
                Text questionText = questionItem.GetComponentInChildren<Text>();
                if (questionText != null)
                {
                    questionText.text = $"{question.questionName}\n{question.questionDescription}";
                }
            }
        }
    }
}
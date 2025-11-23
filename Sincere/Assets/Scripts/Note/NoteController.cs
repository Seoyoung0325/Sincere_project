using UnityEngine;

public class NoteController : MonoBehaviour
{
    public GameObject cluePanel;       // 단서 기록
    public GameObject questionPanel;   // 의문점
    public GameObject mapPanel;        // 지도

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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
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
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentIndex++;
                if (currentIndex > 2) currentIndex = 0; // 순환
                ShowSection(currentIndex);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentIndex--;
                if (currentIndex < 0) currentIndex = 2; // 순환
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
            case 0: cluePanel.SetActive(true); break;
            case 1: questionPanel.SetActive(true); break;
            case 2: mapPanel.SetActive(true); break;
        }
    }

    private void HideAllSections()
    {
        cluePanel.SetActive(false);
        questionPanel.SetActive(false);
        mapPanel.SetActive(false);
    }
}
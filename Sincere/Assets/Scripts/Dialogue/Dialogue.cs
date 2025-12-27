using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI; //대화창 전체
    [SerializeField] private Image characterImage; //캐릭터 스프라이트
    [SerializeField] private Text nameText; //캐릭터 이름
    [SerializeField] private Text dialogueText; //대사 텍스트

    private DialogueData currentDialogueData;
    private int currentDialogueIndex = 0;
    private Action DialogueEndCallback;
    private Action<int> OnClueTimingCallback;
    private Action<int> OnQuestionTimingCallback;


    void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NextDialogue();
        }
    }

    //대화 종료 콜백 설정
    public void SetOnDialogueEnd(Action callback)
    {
        DialogueEndCallback = callback;
    }

    // 단서/의문점 습득 콜백 설정
    public void SetTimingCallbacks(Action<int> clueCallback, Action<int> questionCallback)
    {
        OnClueTimingCallback = clueCallback;
        OnQuestionTimingCallback = questionCallback;
    }



    //시작 (InvestObject에서 호출)
    public void StartDialogue(string dialogueID)
    {
        // DataManager에서 대화 데이터 로드
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager가 없습니다!");
            return;
        }

        // dialogueID로 데이터 찾기
        currentDialogueData = DataManager.instance.GetDialogueByID(dialogueID);

        if (currentDialogueData == null)
        {
            Debug.LogError($"대화 ID '{dialogueID}'를 찾을 수 없습니다!");
            return;
        }

        currentDialogueIndex = 0;

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(true);
        }

        ShowDialogue();
    }


    //다음 대사로
    public void NextDialogue()
    {
        currentDialogueIndex++;

        // 현재 대사 인덱스가 단서 타이밍과 일치하면
        if (currentDialogueData.clueTiming == currentDialogueIndex)
        {
            OnClueTimingCallback?.Invoke(currentDialogueIndex);
        }

        // 현재 대사 인덱스가 의문점 타이밍과 일치하면
        if (currentDialogueData.questionTiming == currentDialogueIndex)
        {
            OnQuestionTimingCallback?.Invoke(currentDialogueIndex);
        }

        if (currentDialogueIndex >= currentDialogueData.dialogue.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowDialogue();
        }
    }


    // 현재 대사 표시
    private void ShowDialogue()
    {
        if (currentDialogueData == null || currentDialogueData.dialogue.Length == 0) return;

        // 캐릭터 이름 표시
        if (nameText != null && currentDialogueData.characterName.Length > currentDialogueIndex)
        {
            nameText.text = currentDialogueData.characterName[currentDialogueIndex];
        }

        // 대사 텍스트 표시
        if (dialogueText != null)
        {
            dialogueText.text = currentDialogueData.dialogue[currentDialogueIndex];
        }

        // 캐릭터 스프라이트 표시
        if (characterImage != null && currentDialogueData.characterSprite != null
        && currentDialogueData.characterSprite.Length > currentDialogueIndex)
        {
            string spritePath = currentDialogueData.characterSprite[currentDialogueIndex];
            if (!string.IsNullOrEmpty(spritePath))
            {
                Sprite sprite = Resources.Load<Sprite>(spritePath);
                if (sprite != null)
                {
                    characterImage.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning($"스프라이트를 찾을 수 없습니다");
                }
            }
        }
    }

    // 대화 종료
    private void EndDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }

        currentDialogueIndex = 0;
        currentDialogueData = null;

        DialogueEndCallback?.Invoke();
        DialogueEndCallback = null;
    }
}
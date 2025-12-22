using UnityEngine;
using UnityEngine.UI;
using System;

public class Dialogue : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialogueUI; //대화창 전체
    [SerializeField] private Image characterImage; //캐릭터 스프라이트
    [SerializeField] private Text nameText; //캐릭터 이름
    [SerializeField] private Text dialogueText; //대사 텍스트

    [Header("데이터")]
    [SerializeField] private string[] dialogues; // 대사 배열
    [SerializeField] private string[] characterNames; // 캐릭터 이름 배열
    [SerializeField] private Sprite[] characterSprites; // 캐릭터 스프라이트 배열

    private int currentDialogueIndex = 0;
    private Action DialogueEndCallback;


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


    //시작 (InteractableObject에서 호출)
    public void StartDialogue()
    {
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

        if (currentDialogueIndex >= dialogues.Length)
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
        if (dialogues.Length == 0) return;

        // 캐릭터 스프라이트 표시
        if (characterImage != null && characterSprites.Length > currentDialogueIndex)
        {
            characterImage.sprite = characterSprites[currentDialogueIndex];
        }

        // 캐릭터 이름 표시
        if (nameText != null && characterNames.Length > currentDialogueIndex)
        {
            nameText.text = characterNames[currentDialogueIndex];
        }

        // 대사 텍스트 표시
        if (dialogueText != null)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
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

        DialogueEndCallback.Invoke();
        DialogueEndCallback = null;
    }
}
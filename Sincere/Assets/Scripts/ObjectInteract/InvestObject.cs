using UnityEngine;

public enum ObjectType
{
    DialogueOnly, ClueAndDestroy, ClueAndKeep
}

public class InvestObject : MonoBehaviour
{
    public GameObject player;

    private float interactionDistance = 2f;  //상호작용 가능 거리 임의설정, 추후 수정가능
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private Dialogue dialogue;

    [Header("오브젝트 정보")]
    [SerializeField] private string objectID;

    private bool isPlayerInRange = false;


    void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        if (player == null)
        {
            Debug.LogWarning("Player 오브젝트가 할당되지 않았습니다!");
        }
    }

    void Update()
    {
        if (player == null) return;

        //플레이어와의 거리 계산
        float distance = Vector2.Distance(transform.position, player.transform.position);

        //거리에 따라 UI 표시/숨김
        if (distance <= interactionDistance && !isPlayerInRange)
        {
            ShowUI();
            isPlayerInRange = true;
        }
        else if (distance > interactionDistance && isPlayerInRange)
        {
            HideUI();
            isPlayerInRange = false;
        }

        //Z키 입력 체크
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    private void ShowUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }
    }

    private void HideUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }


    //UI클릭 시 호출
    public void OnUIClick()
    {
        if (isPlayerInRange)
        {
            Interact();
        }
    }

    private void Interact()
    {
        HideUI();

        // 대화가 끝났을 때 호출될 콜백 설정
        dialogue.SetOnDialogueEnd(OnDialogueEnd);
        dialogue.StartDialogue();
    }

    
    private void OnDialogueEnd()
    {
        if (!string.IsNullOrEmpty(objectID))
        {
            ObjectData objData = DataManager.instance.allObjects.Find(o => o.objectID == objectID);
            if (objData == null)
            {
                Debug.LogWarning($"'{objectID}'가 존재하지 않음");
                return;
            }

            switch (objData.objectType)
            {
                case "DialogueOnly":
                    //타입1: 상호작용 시 대사창만 뜸→맵의 오브젝트 유지
                    break;

                case "ClueAndDestroy":
                    //타입2 : 상호작용 시 대사창→단서 업데이트→맵의 오브젝트 삭제/수정
                    UpdateClue();
                    Destroy(gameObject);
                    break;

                case "ClueAndKeep":
                    //타입3 : 상호작용 시 대사창→단서 업데이트→맵의 오브젝트 유지
                    UpdateClue();
                    break;
            }
        }
        else
        {
            Debug.Log("오브젝트ID 설정 안됨");
            return;
        }
    }


    // 단서,의문점 업데이트
    private void UpdateClue()
    {
        if (!string.IsNullOrEmpty(objectID))
        {
            DataManager.instance.UpdateObjectClue(objectID);
        }
        else
        {
            Debug.Log("오브젝트ID 설정 안됨");
            return;
        }
    }
}
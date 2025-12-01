using Unity.VisualScripting;
using UnityEngine;

public class InvestObject : MonoBehaviour
{
    public GameObject player;

    private float interactionDistance = 2f;  //상호작용 가능 거리 임의설정, 추후 수정가능
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private GameObject dialogueUI;

    private bool isPlayerInRange = false;


    void Start()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
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
        Debug.Log("오브젝트 조사");

        HideUI();

        if (dialogueUI != null)
        {
            dialogueUI.SetActive(true);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SaveData : MonoBehaviour
{
    private Button button;
    public SelectSlot slotPanel;

    public GameObject player;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // 슬롯 패널 숨기기
        if (slotPanel != null)
        {
            slotPanel.gameObject.SetActive(false);
        }
    }


    //저장하기 버튼 클릭 -> 데이터 저장 -> 슬롯패널 열기
    private void OnClick()
    {
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager 연결 안됨");
            return;
        }

        if (player != null)
        {
            DataManager.instance.SavePlayerPosition(player.transform.position);
        }
        else { print("플레이어 연결 안됨"); }

        if (slotPanel != null)
        {
            slotPanel.gameObject.SetActive(true);
            slotPanel.SetupForSave(OnSaveComplete);  //저장 모드
        }
    }


    // 슬롯 선택 완료 후
    private void OnSaveComplete()
    {
        StartCoroutine(SaveWithScreenshot());
    }

    // 스크린샷 찍고 저장
    private IEnumerator SaveWithScreenshot()
    {
        // 스크린샷 찍기
        yield return StartCoroutine(DataManager.instance.CaptureScreenshotCoroutine());

        // 데이터 저장
        DataManager.instance.SaveData();

        Debug.Log("저장 완료");
    }
}
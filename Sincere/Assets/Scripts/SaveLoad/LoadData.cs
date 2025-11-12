using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadData : MonoBehaviour
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


    //저장하기 버튼 클릭 -> 슬롯패널 열기
    private void OnClick()
    {
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager가 없습니다!");
            return;
        }

        if (slotPanel != null)
        {
            slotPanel.gameObject.SetActive(true);
            slotPanel.SetupForLoad(GetLoadedData);  //불러오기 모드
        }
    }

    //불러온 데이터 가져오기
    private void GetLoadedData()
    {
        SceneManager.LoadScene("MainScene");
    }
}
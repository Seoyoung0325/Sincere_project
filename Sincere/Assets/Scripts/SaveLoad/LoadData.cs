using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadData : MonoBehaviour
{
    private Button button;
    public GameObject loading;
    public SelectSlot slotPanel;
    public Button saveActionButton;
    public Button loadActionButton;

    public GameObject player;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // 슬롯 패널 숨기기
        /*if (slotPanel != null)
        {
            slotPanel.gameObject.SetActive(false);
        }*/
    }


    //이어하기 버튼 클릭 -> 슬롯패널 열기
    private void OnClick()
    {
        saveActionButton.gameObject.SetActive(false);
        loadActionButton.gameObject.SetActive(true);

        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager 연결 안됨");
            return;
        }

        if (loading != null)
        {
            loading.SetActive(true);
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
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadData : MonoBehaviour
{
    private Button button;
    public GameObject loading;
    public SelectSlot slotPanel;
    public GameObject closeBtn; // 민채은 수정 - 닫기 버튼 추가

    public GameObject player;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        // 슬롯 패널 숨기기
        if (slotPanel != null)
        {
            slotPanel.gameObject.SetActive(false);
            closeBtn.gameObject.SetActive(false);   // 닫기 버튼 추가
        }
    }


    //이어하기 버튼 클릭 -> 슬롯패널 열기
    private void OnClick()
    {
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager 연결 안됨");
            return;
        }

        if (loading != null)
        {
            loading.SetActive(true);
            slotPanel.gameObject.SetActive(true);
            closeBtn.SetActive(true);   // 닫기 버튼 추가
            slotPanel.SetupForLoad(GetLoadedData);  //불러오기 모드
        }
    }

    //불러온 데이터 가져오기
    private void GetLoadedData()
    {
        SceneManager.LoadScene("MainScene");
    }


    //닫기 버튼 클릭시 패널 닫힘
    public void CloseLoading()
    {
        loading.SetActive(false);
    }
}
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SelectSlot : MonoBehaviour
{
    [Header("슬롯 UI")]
    public GameObject loadingSlot;
    public Button[] slotButtons;  //슬롯버튼 배열
    public Text[] blankTexts;  //빈 슬롯버튼의 텍스트 배열
    public Image[] mapImages;  //저장된 슬롯버튼의 맵 이미지 배열
    public Image[] TimeImages;  //저장된 슬롯버튼의 시간대 이미지 배열
    public Text[] mapTexts;  //저장된 슬롯버튼의 맵 텍스트 배열
    public Text[] timeTexts;  //저장된 슬롯버튼의 플탐 텍스트 배열
    public Outline[] slotOutlines;  //슬롯 테두리

    [Header("추가 버튼")]
    public Button saveActionButton;  // 저장하기 버튼
    public Button loadActionButton;  // 불러오기 버튼
    public Button cancelActionButton;  // 취소 버튼

    [Header("팝업 UI")]
    public GameObject confirmPopup;  // 확인 팝업 패널
    public Text confirmText;  // 확인 메시지 텍스트
    public Button confirmYesButton;  // 예 버튼
    public Button confirmNoButton;  // 아니오 버튼

    bool[] saveFile = new bool[3];  //슬롯별 세이브파일 존재유무
    private System.Action onActionComplete;  // 완료 시 실행할 콜백
    private bool isForLoading = false;  // 불러오기 모드인지 여부
    private int selectedSlotNumber = -1;  // 현재 선택된 슬롯 번호
    private bool isSaveMode = false;  // 저장 모드인지 여부


    private void Awake()
    {
        // DataManager 체크
        if (DataManager.instance == null)
        {
            Debug.LogError("DataManager가 없습니다!");
            return;
        }

        // 버튼 클릭 이벤트 등록
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotNumber = i;
            slotButtons[i].onClick.AddListener(() => OnSlotClicked(slotNumber));
        }


        // 액션 버튼 이벤트 등록
        if (saveActionButton != null)
        {
            saveActionButton.onClick.RemoveAllListeners();
            saveActionButton.onClick.AddListener(OnSaveActionClicked);
        }

        if (loadActionButton != null)
            loadActionButton.onClick.AddListener(OnLoadActionClicked);

        if (cancelActionButton != null)
            cancelActionButton.onClick.AddListener(OnCancelActionClicked);

        // 확인 팝업 버튼 이벤트 등록
        if (confirmYesButton != null)
            confirmYesButton.onClick.AddListener(OnConfirmYes);

        if (confirmNoButton != null)
            confirmNoButton.onClick.AddListener(OnConfirmNo);

        // 초기 상태 설정
        confirmPopup.SetActive(false);

        // 첫 시작 시 슬롯 정보 업데이트
        HideAllOutlines();
        UpdateSlotUI();
    }


    //// 1. 슬롯 클릭
    private void OnSlotClicked(int slotNumber)
    {
        selectedSlotNumber = slotNumber;
        DataManager.instance.slot = slotNumber;

        Debug.Log($"슬롯 {slotNumber} 선택됨");

        HideAllOutlines();

        // 선택된 슬롯 테두리만 켜기
        if (slotOutlines != null && slotNumber < slotOutlines.Length && slotOutlines[slotNumber] != null)
        {
            slotOutlines[slotNumber].enabled = true;
        }
    }

    // 모든 슬롯 테두리 끄기
    private void HideAllOutlines()
    {
        for (int i = 0; i < slotOutlines.Length; i++)
        {
            if (slotOutlines[i] != null)
            {
                slotOutlines[i].enabled = false;
            }
        }
    }


    //// 2-1. 저장하기 버튼 클릭
    private void OnSaveActionClicked()
    {
        Debug.Log("저장클릭");
        if (selectedSlotNumber < 0)
        {
            Debug.LogError("슬롯이 선택되지 않았습니다!");
            return;
        }

        // 확인 팝업 표시
        string message = $"이 파일을 덮어쓰시겠습니까?";
        ShowConfirmPopup(message, true);  // true = 저장 모드
    }


    //// 2-2. 불러오기 버튼 클릭
    private void OnLoadActionClicked()
    {
        Debug.Log("불러오기클릭");
        if (selectedSlotNumber < 0)
        {
            Debug.LogError("슬롯이 선택되지 않았습니다!");
            return;
        }

        // 세이브 파일이 없으면 불러오기 불가
        if (!saveFile[selectedSlotNumber])
        {
            Debug.LogWarning($"슬롯 {selectedSlotNumber}에 데이터가 없습니다!");
            return;
        }

        // 확인 팝업 표시
        string message = $"이 파일을 불러오시겠습니까?";
        ShowConfirmPopup(message, false);  // false = 불러오기 모드
    }


    // 뒤로가기 버튼 클릭
    private void OnCancelActionClicked()
    {
        selectedSlotNumber = -1;
        if (loadingSlot != null)
            loadingSlot.SetActive(false);
    }



    //// 3-1. 확인 팝업에서 "예" 클릭
    private void OnConfirmYes()
    {
        Debug.Log("예");
        HideConfirmPopup();

        if (isSaveMode)
        {
            // 4단계: 저장 실행
            Debug.Log("저장 실행");
            ExecuteSave();
        }
        else
        {
            // 4단계: 불러오기 실행
            Debug.Log("불러오기 실행");
            ExecuteLoad();
        }
    }

    //// 3-1. 확인 팝업에서 "아니요" 클릭
    private void OnConfirmNo()
    {
        Debug.Log("아니요");
        HideConfirmPopup();
    }



    //// 4-1. 실제 저장 실행
    private void ExecuteSave()
    {
        Debug.Log($"슬롯 {selectedSlotNumber}에 저장 중...");

        DataManager.instance.slot = selectedSlotNumber;

        // 패널 닫기
        gameObject.SetActive(false);

        // 완료 콜백 실행 (SaveData.cs에서 스크린샷과 함께 저장)
        onActionComplete?.Invoke();
    }


    //// 4-1. 실제 저장 실행
    private void ExecuteLoad()
    {
        Debug.Log($"슬롯 {selectedSlotNumber} 불러오기 중...");

        // 데이터가 있는지 확인
        if (saveFile[selectedSlotNumber])
        {
            // 불러오기 실행
            DataManager.instance.slot = selectedSlotNumber;
            DataManager.instance.LoadData();

            // 패널 닫기
            gameObject.SetActive(false);

            // 완료 콜백 실행
            onActionComplete?.Invoke();
        }
        else
        {
            Debug.LogError($"슬롯 {selectedSlotNumber}에 데이터가 없습니다!");
        }
    }




    // 확인 팝업 열기
    private void ShowConfirmPopup(string message, bool saveMode)
    {
        Debug.Log("팝업 표시 시작");
        isSaveMode = saveMode;

        // 버튼들 일시 비활성화
        for (int i = 0; i < slotButtons.Length; i++)
        {
            slotButtons[i].interactable = false;
        }
        saveActionButton.interactable = false;
        loadActionButton.interactable = false;
        cancelActionButton.interactable = false;

        // loadingSlot이 꺼지지 않도록 보장
        if (loadingSlot != null && !loadingSlot.activeSelf)
        {
            Debug.LogWarning("loadingSlot이 비활성화되어 있습니다. 다시 켭니다.");
            loadingSlot.SetActive(true);
        }


        // 팝업 활성화
        if (confirmPopup != null)
        {
            confirmPopup.SetActive(true);
            confirmPopup.transform.SetAsLastSibling(); // UI 최상위로 이동

            if (confirmText != null)
            {
                confirmText.text = message;
                Debug.Log("팝업 텍스트 설정 완료");
            }
            else
            {
                Debug.LogError("confirmText가 null입니다!");
            }

            Debug.Log("팝업 표시 완료");
        }
        else
        {
            Debug.LogError("confirmPopup이 null입니다!");
        }
    }



    // 확인 팝업 닫기
    private void HideConfirmPopup()
    {
        if (confirmPopup != null)
            confirmPopup.SetActive(false);

        // 버튼들 다시 활성화
        for (int i = 0; i < slotButtons.Length; i++)
        {
            slotButtons[i].interactable = true;
        }
        if (saveActionButton != null)
            saveActionButton.interactable = true;
        if (loadActionButton != null)
            loadActionButton.interactable = true;
        if (cancelActionButton != null)
            cancelActionButton.interactable = true;

        Debug.Log("팝업 닫힘, 버튼들 다시 활성화");
    }




    public void UpdateSlotUI()
    {
        // 현재 슬롯 번호 백업
        int currentSlot = DataManager.instance.slot;

        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.savePath + $"{i}.json"))  //세이브파일O
            {
                saveFile[i] = true;  //해당 슬롯 번호의 bool값 true로 변환

                PlayerData tempData = LoadSlotData(i);
                if (tempData != null)
                {
                    mapImages[i].gameObject.SetActive(true);
                    TimeImages[i].gameObject.SetActive(true);
                    mapTexts[i].gameObject.SetActive(true);
                    timeTexts[i].gameObject.SetActive(true);
                    blankTexts[i].gameObject.SetActive(false);

                    mapTexts[i].text = "현재 진행 맵(추후 구현)";
                    timeTexts[i].text = DataManager.FormatPlayTime(tempData.playTime);
                    LoadScreenshot(i);
                }
                else
                {
                    mapImages[i].color = Color.red;
                    TimeImages[i].color = Color.green;
                    mapTexts[i].text = "오류 발생";
                    timeTexts[i].text = "오류 발생";
                }

                // 불러오기 모드면 모든 버튼 활성화
                slotButtons[i].interactable = true;
            }

            else  //세이브파일X
            {
                saveFile[i] = false;  //해당 슬롯 번호의 bool값 true로

                mapImages[i].gameObject.SetActive(false);
                TimeImages[i].gameObject.SetActive(false);
                mapTexts[i].gameObject.SetActive(false);
                timeTexts[i].gameObject.SetActive(false);
                blankTexts[i].gameObject.SetActive(true);

                blankTexts[i].text = $"(빈 슬롯)";

                mapImages[i].sprite = null;
                mapImages[i].color = new Color(0.3f, 0.3f, 0.3f);

                slotButtons[i].interactable = true;
            }
        }
        // 원래 슬롯 번호 복원
        DataManager.instance.slot = currentSlot;
    }


    // 특정 슬롯의 데이터만 읽어오기
    private PlayerData LoadSlotData(int slotNumber)
    {
        string filePath = DataManager.instance.savePath + $"{slotNumber}.json";

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);

                // 수정: GameData로 먼저 파싱!
                GameData gameData = JsonUtility.FromJson<GameData>(json);

                if (gameData != null && gameData.player != null)
                {
                    return gameData.player;
                }
                else
                {
                    Debug.LogError($"[슬롯 {slotNumber}] GameData 파싱 실패");
                    return null;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"슬롯 {slotNumber} 로드 오류: {e.Message}");
                return null;
            }
        }

        return null;
    }


    // 저장 모드로 설정
    public void SetupForSave(System.Action onSaveComplete = null)
    {
        isForLoading = false;
        onActionComplete = onSaveComplete;
        UpdateSlotUI();
    }


    // 불러오기 모드로 설정
    public void SetupForLoad(System.Action onLoadComplete = null)
    {
        isForLoading = true;
        onActionComplete = onLoadComplete;
        UpdateSlotUI();
    }


    // 게임 시작 모드로 설정 (원래 기능)
    public void SetupForGameStart()
    {
        isForLoading = false;
        onActionComplete = null;  // 기본 동작 사용
        UpdateSlotUI();
    }



    // 스크린샷 로드
    private void LoadScreenshot(int slotIndex)
    {
        string screenshotPath = DataManager.instance.savePath + $"{slotIndex}.png";
        StartCoroutine(LoadScreenshotCoroutine(slotIndex, screenshotPath));
    }


    // 안전한 스크린샷 로드
    private IEnumerator LoadScreenshotCoroutine(int slotIndex, string screenshotPath)
    {
        // 파일 잠금 해제 대기
        yield return new WaitForSeconds(0.2f);

        try
        {
            byte[] fileData;

            // FileShare.ReadWrite로 공유 읽기
            using (FileStream fs = new FileStream(
                screenshotPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite))
            {
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
            }

            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(fileData))
            {
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );

                mapImages[slotIndex].sprite = sprite;
                mapImages[slotIndex].color = Color.white;

            }
            else
            {
                mapImages[slotIndex].sprite = null;
                mapImages[slotIndex].color = new Color(0.3f, 0.5f, 0.8f);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"스크린샷 로드 실패: {e.Message}");
            mapImages[slotIndex].sprite = null;
            mapImages[slotIndex].color = new Color(0.3f, 0.5f, 0.8f);
        }
    }
}

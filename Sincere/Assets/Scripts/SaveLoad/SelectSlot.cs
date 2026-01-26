using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SelectSlot : MonoBehaviour
{
    public Button[] slotButtons;  //슬롯버튼 배열
    public Text[] blankTexts;  //빈 슬롯버튼의 텍스트 배열
    public Image[] mapImages;  //저장된 슬롯버튼의 맵 이미지 배열
    public Image[] TimeImages;  //저장된 슬롯버튼의 시간대 이미지 배열
    public Text[] mapTexts;  //저장된 슬롯버튼의 맵 텍스트 배열
    public Text[] timeTexts;  //저장된 슬롯버튼의 플탐 텍스트 배열

    bool[] saveFile = new bool[3];  //슬롯별 세이브파일 존재유무
    private System.Action<int> onSlotSelected;  // 슬롯 선택 시 실행할 함수
    private bool isForLoading = false;  // 불러오기 모드인지 여부

    private void Start()
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
            int slotNumber = i;  //클로저 문제 방지
            slotButtons[i].onClick.AddListener(() => Slot(slotNumber));
        }

        // 첫 시작 시 슬롯 정보 업데이트
        UpdateSlotUI();
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

                if (isForLoading)
                {
                    //불러오기 모드면 빈 슬롯은 비활성화
                    slotButtons[i].interactable = false;
                }
                else
                {
                    slotButtons[i].interactable = true;
                }
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


    //슬롯 선택
    public void Slot(int number)
    {
        DataManager.instance.slot = number;  //슬롯번호 입력

        // 등록된 콜백 함수 실행
        if (onSlotSelected != null)
        {
            onSlotSelected.Invoke(number);
        }
        else
        {
            // 기본 동작: 게임 시작
            DefaultSlotAction(number);
        }
    }


    // 기본 슬롯 동작 (게임 시작용)
    private void DefaultSlotAction(int number)
    {
        if (saveFile[number])
        {
            //현재 슬롯에 세이브파일 있으면 데이터 불러오기
            DataManager.instance.LoadData();
        }
        else
        {
            //현재 슬롯에 세이브파일 없으면 현재 데이터 저장
            DataManager.instance.player = new PlayerData();
            DataManager.instance.SaveData();
        }

        SceneManager.LoadScene("MainScene");
    }


    // 저장 모드로 설정
    public void SetupForSave(System.Action onSaveComplete = null)
    {
        isForLoading = false;
        UpdateSlotUI();

        onSlotSelected = (slotNumber) =>
        {
            // 저장 실행
            DataManager.instance.slot = slotNumber;
            DataManager.instance.SaveData();

            // 패널 닫기
            gameObject.SetActive(false);

            // 완료 콜백 실행
            onSaveComplete?.Invoke();
        };
    }


    // 불러오기 모드로 설정
    public void SetupForLoad(System.Action onLoadComplete = null)
    {
        isForLoading = true;
        UpdateSlotUI();

        onSlotSelected = (slotNumber) =>
        {
            // 데이터가 있는지 확인
            if (saveFile[slotNumber])
            {
                // 불러오기 실행
                DataManager.instance.slot = slotNumber;
                DataManager.instance.LoadData();

                // 패널 닫기
                gameObject.SetActive(false);

                // 완료 콜백 실행
                onLoadComplete?.Invoke();
            }
            else
            {
                Debug.LogError($"슬롯 {slotNumber}에 데이터가 없음");
            }
        };
    }


    // 게임 시작 모드로 설정 (원래 기능)
    public void SetupForGameStart()
    {
        isForLoading = false;
        UpdateSlotUI();
        onSlotSelected = null;  // 기본 동작 사용
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

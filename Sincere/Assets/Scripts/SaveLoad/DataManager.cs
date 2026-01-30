using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 플레이 중 생성되는 데이터 전체
[System.Serializable]
public class PlayerData
{
    public float posX;
    public float posY;
    public float posZ;
    public float playTime;
}
[System.Serializable]
public class ObjectData
{
    public string objectID;
    public string objectClue;
    public string objectQuestion;
    public string dialogueID;
    public string objectType;
}
[System.Serializable]
public class ClueData
{
    public string clueID;
    public string clueMap;
    public string clueName;
    public string clueImagePath;
    public string clueDescription;
}
[System.Serializable]
public class QuestionData
{
    public string questionID;
    public string questionMap;
    public string questionName;
    public string questionDescription;
    public string questionClue;
}
[System.Serializable]
public class DialogueData
{
    public string dialogueID;
    public string[] characterName;
    public string[] characterSprite;
    public string[] dialogue;
    public int clueTiming;
    public int questionTiming;
}
[System.Serializable]
public class GameData
{
    public PlayerData player;
    public List<ClueData> acquiredClues;
    public List<QuestionData> acquiredQuestions;
    public List<string> destroyedObjects;
}


// 게임 전체 데이터 관리 스크립트
public class DataManager : MonoBehaviour
{
    public static DataManager instance;  //싱글톤 설정

    public PlayerData player = new PlayerData();

    public List<ClueData> acquiredClues = new List<ClueData>();
    public List<QuestionData> acquiredQuestions = new List<QuestionData>();
    public List<string> destroyedObjects = new List<string>();

    public List<ObjectData> allObjects = new List<ObjectData>();
    public List<ClueData> allClues = new List<ClueData>();
    public List<QuestionData> allQuestions = new List<QuestionData>();
    public List<DialogueData> allDialogues = new List<DialogueData>();

    public string savePath;
    public int slot;

    private float playTimeStart;  //플레이타임 추적


    private void Awake()
    {
        #region //싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            savePath = Application.persistentDataPath + "/Save";  //저장 파일 생성위치 설정
            if (!Directory.Exists(savePath))  //생성위치가 없으면 폴더 생성
            {
                Directory.CreateDirectory(savePath);
            }

            savePath += "/";
            print("저장경로" + savePath);

            LoadGameData();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        #endregion

        playTimeStart = Time.time;
    }


    public void Update()
    {
        if (player != null)
        {
            player.playTime = Time.time - playTimeStart;
        }
    }


    // 전체 게임 데이터 로드
    public void LoadGameData()
    {
        // Resources/GameData/objects.json
        TextAsset objectsJson = Resources.Load<TextAsset>("GameData/objects");
        if (objectsJson != null)
        {
            ObjectDataList objectList = JsonUtility.FromJson<ObjectDataList>(objectsJson.text);
            allObjects = objectList.objects;
        }
        else { Debug.Log($"오브젝트 데이터 로드 실패"); }

        // Resources/GameData/clues.json
        TextAsset cluesJson = Resources.Load<TextAsset>("GameData/clues");
        if (cluesJson != null)
        {
            ClueDataList clueList = JsonUtility.FromJson<ClueDataList>(cluesJson.text);
            allClues = clueList.clues;
        }
        else { Debug.Log($"단서 데이터 로드 실패"); }

        // Resources/GameData/questions.json
        TextAsset questionsJson = Resources.Load<TextAsset>("GameData/questions");
        if (questionsJson != null)
        {
            QuestionDataList questionList = JsonUtility.FromJson<QuestionDataList>(questionsJson.text);
            allQuestions = questionList.questions;
        }
        else { Debug.Log($"의문점 데이터 로드 실패"); }

        // Resources/GameData/dialogues.json
        TextAsset dialoguesJson = Resources.Load<TextAsset>("GameData/dialogues");
        if (dialoguesJson != null)
        {
            DialogueDataList dialoguesList = JsonUtility.FromJson<DialogueDataList>(dialoguesJson.text);
            allDialogues = dialoguesList.dialogues;
        }
        else { Debug.Log($"대사 데이터 로드 실패"); }
    }

    // dialogueID로 대화 데이터 찾기
    public DialogueData GetDialogueByID(string dialogueID)
    {
        return allDialogues.Find(d => d.dialogueID == dialogueID);
    }




    // 전체 데이터 저장
    public void SaveData()
    {
        player.playTime = Time.time - playTimeStart;

        GameData saveData = new GameData
        {
            player = player,
            acquiredClues = acquiredClues,
            acquiredQuestions = acquiredQuestions,
            destroyedObjects = destroyedObjects
        };

        string savedData = JsonUtility.ToJson(saveData, true);  //데이터 -> JSON으로 변환
        string path = savePath + slot.ToString() + ".json";
        File.WriteAllText(path, savedData);  //JSON -> 로컬에 파일로 저장
        print($"{slot} 저장");
    }


    // 전체 데이터 불러오기
    public void LoadData()
    {
        string path = savePath + slot.ToString() + ".json";

        if (File.Exists(path))
        {
            string loadedData = File.ReadAllText(path);  //로컬에 저장된 파일 -> JSON
            GameData saveData = JsonUtility.FromJson<GameData>(loadedData);  //JSON -> 데이터로 변환

            player = saveData.player;
            acquiredClues = saveData.acquiredClues;
            acquiredQuestions = saveData.acquiredQuestions;
            destroyedObjects = saveData.destroyedObjects;

            // 플레이타임 초기화 (이어서 계속)
            playTimeStart = Time.time - player.playTime;

            Debug.Log($"슬롯 {slot} 불러오기 완료");
        }
    }



    // 플레이어 위치 저장
    public void SavePlayerPosition(Vector3 position)
    {
        player.posX = position.x;
        player.posY = position.y;
        player.posZ = position.z;
    }

    // 플레이어 위치 불러오기
    public Vector3 GetPlayerPosition()
    {
        return new Vector3(player.posX, player.posY, player.posZ);
    }



    // 오브젝트 상호작용 시 단서 업데이트
    public void UpdateObjectClue(string objectID)
    {
        // 1. 오브젝트 데이터 찾기
        ObjectData obj = allObjects.Find(o => o.objectID == objectID);
        if (obj == null)
        {
            Debug.LogWarning($"오브젝트 ID '{objectID}'를 찾을 수 없습니다.");
            return;
        }

        // 2. 연관된 단서 추가
        if (obj.objectClue != null)
        {
            ClueData clue = allClues.Find(c => c.clueID == obj.objectClue);
            if (clue != null)
            {
                AddClue(clue);
            }
        }
    }

    // 오브젝트 상호작용 시 의문점 업데이트
    public void UpdateObjectQuestion(string objectID)
    {
        // 1. 오브젝트 데이터 찾기
        ObjectData obj = allObjects.Find(o => o.objectID == objectID);
        if (obj == null)
        {
            Debug.LogWarning($"오브젝트 ID '{objectID}'를 찾을 수 없습니다.");
            return;
        }

        // 2. 연관된 의문점 추가
        if (obj.objectQuestion != null)
        {
            QuestionData question = allQuestions.Find(q => q.questionID == obj.objectQuestion);
            if (question != null)
            {
                AddQuestion(question);
            }
        }
    }



    // 단서 습득 - 민채은 수정
    public void AddClue(ClueData clueID)
    {
        if (!acquiredClues.Contains(clueID))
        {
            acquiredClues.Add(clueID);
            print($"단서 추가: {clueID}");

            if (ClueUI.instance != null)
            {   // UI 출력
                ClueUI.instance.ShowNotification(clueID);
            }
        }
    }

    // 전체 습득한 단서 불러오기
    public List<ClueData> GetAcquiredClues()
    {
        return new List<ClueData>(acquiredClues);
    }



    // 의문점 습득 - 민채은 수정
    public void AddQuestion(QuestionData questionID)
    {
        if (!acquiredQuestions.Contains(questionID))
        {
            acquiredQuestions.Add(questionID);
            print($"의문점 추가: {questionID}");

            if (ClueUI.instance != null)
            {   // UI 출력
                ClueUI.instance.ShowQuestionNotification(questionID);
            }
        }
    }

    // 전체 습득한 의문점 불러오기
    public List<QuestionData> GetAcquiredQuestions()
    {
        return new List<QuestionData>(acquiredQuestions);
    }


    // 오브젝트 파괴 기록
    public void RegisterDestroyedObject(string objectID)
    {
        if (!destroyedObjects.Contains(objectID))
        {
            destroyedObjects.Add(objectID);
            Debug.Log($"{objectID} 오브젝트 파괴");
        }
    }

    // 오브젝트가 파괴되었는지 확인
    public bool IsObjectDestroyed(string objectID)
    {
        return destroyedObjects.Contains(objectID);
    }



    // 플레이타임 포맷팅
    public static string FormatPlayTime(float seconds)
    {
        int hour = (int)(seconds / 3600);
        int min = (int)((seconds % 3600) / 60);
        int sec = (int)(seconds % 60);

        return $"{hour}:{min}:{sec}";
    }


    //스크린샷 저장
    public IEnumerator CaptureScreenshotCoroutine()
    {
        string screenshotPath = savePath + $"{slot}.png";
        

        // 렌더링 완료 대기
        yield return new WaitForEndOfFrame();

        // 화면을 Texture2D로 직접 캡처
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        // PNG로 인코딩
        byte[] bytes = screenshot.EncodeToPNG();

        // 파일 저장
        try
        {
            System.IO.File.WriteAllBytes(screenshotPath, bytes);

            // 확인
            if (System.IO.File.Exists(screenshotPath))
            {
                FileInfo info = new FileInfo(screenshotPath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"스크린샷 저장 실패: {e.Message}");
        }

        // 메모리 해제
        Destroy(screenshot);
    }
}



// JSON 배열 파싱용 래퍼 클래스들
[System.Serializable]
public class ObjectDataList
{
    public List<ObjectData> objects;
}

[System.Serializable]
public class ClueDataList
{
    public List<ClueData> clues;
}

[System.Serializable]
public class QuestionDataList
{
    public List<QuestionData> questions;
}

[System.Serializable]
public class DialogueDataList
{
    public List<DialogueData> dialogues;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 플레이 중 생성되는 데이터 전체
public class PlayerData
{
    public float posX;
    public float posY;
    public float posZ;
}


// 게임 전체 데이터 관리 스크립트
public class DataManager : MonoBehaviour
{
    public static DataManager instance;  //싱글톤 설정
    public PlayerData player = new PlayerData();

    public string savePath;
    public int slot;


    private void Awake()
    {
        #region //싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); /////////////////
            return;
        }
        #endregion

        savePath = Application.persistentDataPath + "/Save";  //저장 파일 생성위치 설정
        if (!Directory.Exists(savePath))  //생성위치가 없으면 폴더 생성
        {
            Directory.CreateDirectory(savePath);
        }

        savePath += "/";
        print("저장경로" + savePath);
    }


    // 데이터 저장
    public void SaveData()
    {
        string savedData = JsonUtility.ToJson(player, true);  //데이터 -> JSON으로 변환
        string path = savePath + slot.ToString() + ".json";
        File.WriteAllText(path, savedData);  //JSON -> 로컬에 파일로 저장
        print($"{slot} 저장");
    }

    // 플레이어 위치 저장
    public void SavePlayerPosition(Vector3 position)
    {
        player.posX = position.x;
        player.posY = position.y;
        player.posZ = position.z;
    }


    // 데이터 불러오기
    public void LoadData()
    {
        string path = savePath + slot.ToString() + ".json";

        if (File.Exists(path))
        {
            string LoadedData = File.ReadAllText(path);  //로컬에 저장된 파일 -> JSON
            player = JsonUtility.FromJson<PlayerData>(LoadedData);  //JSON -> 데이터로 변환
        }
    }

    // 플레이어 위치 불러오기
    public Vector3 GetPlayerPosition()
    {
        return new Vector3(player.posX, player.posY, player.posZ);
    }
}

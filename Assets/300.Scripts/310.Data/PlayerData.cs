using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO; // 파일 저장 및 불러오기용
using Newtonsoft.Json;
[Serializable]
public class PlayerData
{
    public float hp;
    public int level;
    public float attack;
    public float defense;
    public int luk;
    public float critcleRate;
    public float critcleDmg;
    public float stamina;
    public float staminaAutoRestoration;
    public float hpAutoRestoration;
    public int money;
    public int skillCount;
    public int slotNum;
    public string name = string.Empty;
    public int currentExp = 0;

    public float maxGuardValue;
    public float currentGuardValue;

    public List<int> rewardNumList = new List<int>();
    public List<bool> rewardClearList = new List<bool>();
    public int currentMapNum = 0;

    public float mapDatax;
    public float mapDatay;

    public int hpPotionSelectnum = 0;
    public int staminaPotionSelectnum = 0;
    public int maxHpPotionCount = 5;
    public int maxStaminaPotionCount = 5;
    public int levelUpExp;

    public bool[] isBuff = new bool[4];
    public float[] buffRemainTime = new float[4];

    public int moneyUp;
    public int expUp;

    public Inventory inventory = new Inventory();
    public Skill skill = new Skill();

    public HashSet<int> openedChestIds = new HashSet<int>(); // 연 보물상자 ID 목록

    #region 플레이어 위치 저장
    public float positionX = 0f;
    public float positionY = 0f;

    public Vector2 GetPosition() => new Vector2(positionX, positionY);
    public void SetPosition(Vector2 pos)
    {
        positionX = pos.x;
        positionY = pos.y;
    }
    #endregion

    #region 장비 장착 아이템 이름
    public string[] equippedItemNames = new string[9]; // 각 부위별 장착된 아이템 이름
    #endregion

    void LevelDb(int _level)
    {
        level = _level;
        critcleDmg = 150;
        if (level == 1)
        {
            hp = 100;
            attack = 6;
            defense = 4;
            luk = 5;
            critcleRate = 5;
            stamina = 50;
            skillCount = 0;
            staminaAutoRestoration = 2f;
            hpAutoRestoration = 0;
            levelUpExp = 200;
            maxGuardValue = 100;
            currentGuardValue = 100;
        }
        else
        {
            if (level == 2)
            {
                hp = Mathf.Round(100 * 1.2f);
                attack = Mathf.Round(8 * 1.2f);
                defense = Mathf.Round(4 * 1.2f);
                luk = Mathf.RoundToInt(5 * 1.2f);
                critcleRate = Mathf.Round(5 * 1.2f);
                stamina = Mathf.Round(50 * 1.2f);
                staminaAutoRestoration = Mathf.Round(2f * 1.2f);
                levelUpExp = Mathf.RoundToInt(200 * 2.5f);               
            }
            else
            {
                hp = Mathf.Round(100 * Mathf.Pow(1.2f, (_level - 1)));
                attack = Mathf.Round(8 * Mathf.Pow(1.2f, (_level - 1)));
                defense = Mathf.Round(12 * Mathf.Pow(1.2f, (_level - 1)));
                luk = Mathf.RoundToInt(7 * Mathf.Pow(1.2f, (_level - 1)));
                critcleRate = Mathf.Round(5 * Mathf.Pow(1.2f, (_level - 1)));
                stamina = Mathf.Round(50 * Mathf.Pow(1.2f, (_level - 1)));
                staminaAutoRestoration = Mathf.Round(2 * Mathf.Pow(1.2f, (_level - 1)));
                levelUpExp = Mathf.RoundToInt(200 * Mathf.Pow(2.5f, (_level - 1)));               
            }           
        }
    }

    public void SetLevel(int _level)
    {
        LevelDb(_level);
        PM.playerData.hp = hp;
        PM.playerData.attack = attack;
        PM.playerData.defense = defense;
        PM.playerData.luk = luk;
        PM.playerData.critcleRate = critcleRate;
        PM.playerData.stamina = stamina;
        PM.playerData.skillCount = skillCount;
        PM.playerData.critcleDmg = critcleDmg;
        PM.playerData.staminaAutoRestoration = staminaAutoRestoration;
        PM.playerData.hpAutoRestoration = hpAutoRestoration;
        PM.playerData.levelUpExp = levelUpExp;
        PM.playerData.maxGuardValue = maxGuardValue;
        PM.playerData.currentGuardValue = currentGuardValue;
    }
}

public class PM
{
    internal static PlayerData playerData = new PlayerData();
    internal static List<PlayerData> playerList = new List<PlayerData>();
    private static string path = Environment.CurrentDirectory + "/playerData.json"; // 파일 경로 설정

    internal static void RegisterNewPlayer(PlayerData player, int slotIndex)
    {
        playerData.slotNum = slotIndex;

        // JSON 파일에서 기존 데이터를 불러오기
        LoadPlayerData();

        // 슬롯 리스트의 크기를 미리 맞춤 (최대 3개의 슬롯)
        while (playerList.Count <= slotIndex)
        {
            playerList.Add(null);  // 빈 슬롯을 추가하여 리스트 크기를 맞춤
        }

        if (slotIndex >= 0 && slotIndex < 3)
        {
            if (slotIndex < playerList.Count)
            {
                // 기존 슬롯의 데이터를 덮어쓰기
                playerList[slotIndex] = player;
                Debug.Log($"슬롯 {slotIndex}의 데이터 덮어쓰기 완료: {JsonUtility.ToJson(player)}");
            }
            else
            {
                // 새로운 슬롯 추가
                playerList.Add(player);
                Debug.Log($"슬롯 {slotIndex}에 새 데이터 추가 완료: {JsonUtility.ToJson(player)}");
            }
            
        }
        else
        {
            Debug.LogWarning("유효하지 않은 슬롯 인덱스입니다. 최대 3개의 슬롯만 지원됩니다.");
        }

        // playerList가 제대로 업데이트되었는지 확인
        Debug.Log($"현재 playerList 상태: {playerList.Count}개의 항목");
        SavePlayerData();
    }

    // 슬롯의 데이터 삭제
    internal static void DeletePlayerData(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < playerList.Count)
        {
            playerList.RemoveAt(slotIndex); // 선택한 인덱스의 데이터 삭제
        }
        else
        {
            Debug.LogWarning("유효하지 않은 슬롯 인덱스입니다.");
        }

        SavePlayerData();
    }

    // 플레이어 데이터 저장 (JSON 형식)
    private static void SavePlayerData()
    {
        string json = JsonConvert.SerializeObject(new PlayerListWrapper { playerList = playerList }, Formatting.Indented);
        Debug.Log("저장할 JSON 데이터: " + json);
        File.WriteAllText(path, json);
        Debug.Log("플레이어 데이터 저장 완료");
    }

    // 플레이어 데이터 로드
    internal static void LoadPlayerData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerListWrapper wrapper = JsonConvert.DeserializeObject<PlayerListWrapper>(json);
            playerList = wrapper.playerList ?? new List<PlayerData>();
            Debug.Log("플레이어 데이터 로드 완료");
        }
        else
        {
            Debug.Log("저장된 플레이어 데이터가 없습니다.");
        }
    }
}

// 플레이어 리스트 래퍼 클래스 (JSON 직렬화를 위한 클래스)
[Serializable]
public class PlayerListWrapper
{
    public List<PlayerData> playerList = new List<PlayerData>();
}
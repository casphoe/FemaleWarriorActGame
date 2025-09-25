using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO; // 파일 저장 및 불러오기용
using Newtonsoft.Json;

[Serializable]
public class PlayerStat
{
    public int str; //힘 1씩 증가할 때 마다 공격력 0.1씩 증가
    public int intellect; //지력 1씩 증가할 때 마다 스킬 데미지 0.2씩 증가
    public int condition; //체력 1씩 증가할 때 마다 hp값 2씩 증가
    public int dex; //민첩 //민첩 1씩 증가할 때 마다 스테미나 2씩 증가
    public int luk; //운 //운 1씩 증가할 때 마다 크리티컬 확률 0.5씩 증가 크리티컬 데미지양 1% 증가
    
    //총 포인트 양
    public int strStatCount; //힘에 스텟 포인트 투자한 양
    public int intellectStatCount;
    public int conditonStatCount;
    public int dexStatCount;
    public int lukStatCount;

    public int curStrStatCount;
    public int curIntellectStatCount;
    public int curConditonStatCount;
    public int curDexStatCount;
    public int curLukStatCount;

    //게임 맨처음에 시작 할 때 스텟 초기화
    public void StatStartRest()
    {
        strStatCount = 0;
        str = 0;
        intellectStatCount = 0;
        intellect = 0;
        condition = 0;
        conditonStatCount = 0;
        dex = 0;
        dexStatCount = 0;
        luk = 5;
        lukStatCount = 0;
        StatCountRest();
    }

    public void StatCountRest()
    {
        curStrStatCount = 0; curIntellectStatCount = 0; curConditonStatCount = 0; curDexStatCount = 0; curLukStatCount = 0;
    }
}

[Serializable]
public class PlayerData
{
    public float hp;
    public int level;
    public float attack;
    public float defence;
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
    public int statPoint = 0; //스텟을 찍을 수 있는 스텟 포인트
    public float skillDamageBonus; // INT 1당 +0.2 누적 (스킬 데미지 계산에 사용)

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
    public PlayerStat _stat = new PlayerStat();

    public HashSet<int> openedChestIds = new HashSet<int>(); // 연 보물상자 ID 목록

    public List<QuestData> questList = new List<QuestData>(); // 수락된 퀘스트

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
            defence = 4;
            critcleRate = 5;
            stamina = 50;
            skillCount = 0;
            skillDamageBonus = 0;
            staminaAutoRestoration = 2f;
            hpAutoRestoration = 0;
            levelUpExp = 200;
            maxGuardValue = 100;
            currentGuardValue = 100;
            statPoint = 0;
            _stat.StatStartRest();
        }
        else
        {
            statPoint += 7;
            skillCount += 5;
            levelUpExp = Mathf.RoundToInt(200 * Mathf.Pow(2.5f, (_level - 1)));
            currentExp = 0;
        }
    }

    public void SetLevel(int _level)
    {
        LevelDb(_level);
        PM.playerData.hp = hp;
        PM.playerData.attack = attack;
        PM.playerData.defence = defence;
        PM.playerData._stat.luk = _stat.luk;
        PM.playerData.critcleRate = critcleRate;
        PM.playerData.stamina = stamina;
        PM.playerData.skillCount = skillCount;
        PM.playerData.critcleDmg = critcleDmg;
        PM.playerData.staminaAutoRestoration = staminaAutoRestoration;
        PM.playerData.hpAutoRestoration = hpAutoRestoration;
        PM.playerData.levelUpExp = levelUpExp;
        PM.playerData.maxGuardValue = maxGuardValue;
        PM.playerData.currentGuardValue = currentGuardValue;
        PM.playerData.statPoint = statPoint;
        PM.playerData.currentExp = currentExp;
        PM.playerData.levelUpExp = levelUpExp;
        PM.playerData._stat.str = _stat.str;
        PM.playerData._stat.intellect = _stat.intellect;
        PM.playerData._stat.dex = _stat.dex;
        PM.playerData._stat.condition = _stat.condition;
        PM.playerData.skillDamageBonus = skillDamageBonus;
    }
}

public class PM
{
    internal static PlayerData playerData = new PlayerData();
    internal static List<PlayerData> playerList = new List<PlayerData>();
    private static string path => Path.Combine(Application.persistentDataPath, "playerData.json");

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
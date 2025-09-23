using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
//레벨업 시 캐릭터 스텟을 찍을 수 있고 현재 장착 아이템 및 현재 캐릭터 스텟을 볼 수 있는 Panel
public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] GameObject playerInfoPanel;
    [Header("캐릭터 장착 장비 이미지")]
    [SerializeField] Image[] imgCharacterEquip;

    [Header("캐릭터 스텟 - 공격력, 방어력 등")]
    [SerializeField] Text[] txtCharcterStatData;

    [Header("해당 캐릭터 스텟에 스텟 포인트 사용한 값")]
    [SerializeField] Text[] txtCharacterStatCount;
    //현재 스텟포인트
    [SerializeField] Text txtStatPoint;
    //현재 캐릭터 레벨
    [SerializeField] Text txtLevel;
    [SerializeField] Text txtExp;
    [SerializeField] Text txtName;

    [Header("스텟 포인트를 사용해서 캐릭터 스텟을 올려주는 버튼")]
    [SerializeField] Button[] btnStatUp;
    [Header("스텟 포인트를 사용해서 캐릭터 스텟을 올려준 스텟을 다시 내려주는 버튼")]
    [SerializeField] Button[] btnStatDown;

    [Header("찍은 스텟 포인트 저장,찍었던 스텟 포인트를 되돌리는 버튼")]
    //한번 저장하면 스텟을 찍은 것을 직전으로 되돌릴 수 없음
    [SerializeField] Button[] btnStat;
    [Header("스텟의 대한 설명")]
    [SerializeField] Text[] txtStat;

    [Header("스텟마다 들어가는 스텟 포인트 량의 대한 텍스트")]
    [SerializeField] Text[] txtCharcterStatPoints;

    Player currentPlayerData;

    StatSnapshot _lastSaved;     // 최근 저장(또는 패널 오픈) 기준
    bool _sessionActive = false; // 현재 패널 내 임시 배분 세션 여부


    private void Awake()
    {
        Utils.OnOff(playerInfoPanel, false);
    }


    private void Start()
    {
        for(int i = 0; i < imgCharacterEquip.Length; i++)
        {
            Utils.OnOff(imgCharacterEquip[i].gameObject, false);
        }
        currentPlayerData = GameObject.Find("Player").GetComponent<Player>();
        btnStat[0].onClick.AddListener(() => StatPointOnClickEvent(0));
        btnStat[1].onClick.AddListener(() => StatPointOnClickEvent(1));
    }

    private void Update()
    {
        if(PlayerManager.GetCustomKeyDown(CustomKeyCode.PlayerInfo))
        {
            PlayerManager.instance.isPlayerInfo = !PlayerManager.instance.isPlayerInfo;
            var player = PlayerManager.instance.player;
            Utils.OnOff(playerInfoPanel, PlayerManager.instance.isPlayerInfo);
            StartCoroutine(PlayerEquipImage(PlayerManager.instance.isPlayerInfo));
            BeginStatSession(player);
            PlayerStatSetting(PlayerManager.instance.isPlayerInfo, player);
            StatBtnUISetting(PlayerManager.instance.isPlayerInfo, player);
            StatChange(player);
        }
    }

    IEnumerator PlayerEquipImage(bool isAictive)
    {
        yield return null;
        if(isAictive == true)
        {
            for (int i = 0; i < imgCharacterEquip.Length; i++)
            {
                imgCharacterEquip[i].sprite = EquipmentPanel.instance.imgEquip[i].sprite;

                if (imgCharacterEquip[i].sprite != null)
                {
                    Utils.OnOff(imgCharacterEquip[i].gameObject, true);
                    imgCharacterEquip[i].preserveAspect = true;
                }
            }
        }
    }

    void PlayerStatSetting(bool isAictive, PlayerData playerData)
    {
        if (!isAictive)
            return;
        var player = playerData;
        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtStatPoint.text = "스탯 포인트 : " + player.statPoint.ToString("N0");
                txtStat[0].text = "힘 :" + player._stat.str.ToString();
                txtStat[1].text = "지능 :" + player._stat.intellect.ToString();
                txtStat[2].text = "체력 :" + player._stat.condition.ToString();
                txtStat[3].text = "민첩 :" + player._stat.dex.ToString();
                txtStat[4].text = "운 :" + player._stat.luk.ToString();
                txtLevel.text = "레벨 : " + player.level.ToString();
                txtName.text = "플레이어 이름 : " + player.name;
                break;
            case LANGUAGE.ENG:
                txtStatPoint.text = "Stat Point : " + player.statPoint.ToString("N0");
                txtStat[0].text = "Str :" + player._stat.str.ToString();
                txtStat[1].text = "Int :" + player._stat.intellect.ToString();
                txtStat[2].text = "Con :" + player._stat.condition.ToString();
                txtStat[3].text = "Dex :" + player._stat.dex.ToString();
                txtStat[4].text = "Luk :" + player._stat.luk.ToString();
                txtLevel.text = "Level : " + player.level.ToString();
                txtName.text = "Player Name : " + player.name;
                break;
        }
        txtExp.text = player.currentExp.ToString("N0") + " / " + player.levelUpExp.ToString("N0");
        txtCharacterStatCount[0].text = player._stat.strStatCount.ToString();
        txtCharacterStatCount[1].text = player._stat.intellectStatCount.ToString();
        txtCharacterStatCount[2].text = player._stat.conditonStatCount.ToString();
        txtCharacterStatCount[3].text = player._stat.dexStatCount.ToString();
        txtCharacterStatCount[4].text = player._stat.lukStatCount.ToString();
    }

    void StatBtnUISetting(bool isAictive, PlayerData playerData)
    {
        if (!isAictive)
            return;
        var player = playerData;

        // + 버튼은 "잔여 포인트가 '다음 1포인트 비용' 이상"일 때만 활성화
        int[] currentStats = {
        player._stat.str,
        player._stat.intellect,
        player._stat.condition,
        player._stat.dex,
        player._stat.luk
    };

        for (int i = 0; i < btnStatUp.Length; i++)
        {
            int need = GetCostForNextPoint_StatIndex(i, player);
            btnStatUp[i].interactable = (player.statPoint >= need);
            txtCharcterStatPoints[i].text = need.ToString();
        }

        // - 버튼은 세션 내에서 올린 양(curXXXStatCount) 기준으로만 활성화
        btnStatDown[0].interactable = player._stat.curStrStatCount > 0;
        btnStatDown[1].interactable = player._stat.curIntellectStatCount > 0;
        btnStatDown[2].interactable = player._stat.curConditonStatCount > 0;
        btnStatDown[3].interactable = player._stat.curDexStatCount > 0;
        btnStatDown[4].interactable = player._stat.curLukStatCount > 0;
    }

    public void StatPointMinusClickEvent(int num)
    {
        var player = PlayerManager.instance.player;

        // 되돌릴 수 있는지(이번 세션에서 +한 양이 있는지) 먼저 체크
        bool canDown = false;
        switch (num)
        {
            case 0: canDown = player._stat.curStrStatCount > 0; break;
            case 1: canDown = player._stat.curIntellectStatCount > 0; break;
            case 2: canDown = player._stat.curConditonStatCount > 0; break;
            case 3: canDown = player._stat.curDexStatCount > 0; break;
            case 4: canDown = player._stat.curLukStatCount > 0; break;
        }
        if (!canDown) return;

        // 환급 비용 = "지금 수치"에 해당하는 티어 비용
        int refund = GetRefundForCurrentPoint_StatIndex(num, player);

        // 실제 감소 및 카운트 정리
        switch (num)
        {
            case 0:
                player._stat.curStrStatCount -= 1;
                player._stat.strStatCount -= 1;
                player._stat.str -= 1;
                break;
            case 1:
                player._stat.curIntellectStatCount -= 1;
                player._stat.intellectStatCount -= 1;
                player._stat.intellect -= 1;
                break;
            case 2:
                player._stat.curConditonStatCount -= 1;
                player._stat.conditonStatCount -= 1;
                player._stat.condition -= 1;
                break;
            case 3:
                player._stat.curDexStatCount -= 1;
                player._stat.dexStatCount -= 1;
                player._stat.dex -= 1;
                break;
            case 4:
                player._stat.curLukStatCount -= 1;
                player._stat.lukStatCount -= 1;
                player._stat.luk -= 1;
                break;
        }
        ApplyDerivedOnStatChanged(player, num, -1);
        player.statPoint += refund;

        // UI 갱신
        PlayerStatSetting(true, player);
        StatBtnUISetting(true, player);
        StatChange(player);
        GameCanvas.instance.SliderEquipChange();
    }

    public void StatPointPlusClickEvent(int num)
    {
        var player = PlayerManager.instance.player;
        int cost = GetCostForNextPoint_StatIndex(num, player);
        
        // 포인트 부족하면 무시
        if (player.statPoint < cost) return;

        // 비용 지불 + 실제 스탯/카운트 증가

        switch (num)
        {
            case 0:
                player._stat.curStrStatCount += 1;
                player._stat.strStatCount += 1;
                player._stat.str += 1;
                break;
            case 1:
                player._stat.curIntellectStatCount += 1;
                player._stat.intellectStatCount += 1;
                player._stat.intellect += 1;
                break;
            case 2:
                player._stat.curConditonStatCount += 1;
                player._stat.conditonStatCount += 1;
                player._stat.condition += 1;
                break;
            case 3:
                player._stat.curDexStatCount += 1;
                player._stat.dexStatCount += 1;
                player._stat.dex += 1;
                break;
            case 4:
                player._stat.curLukStatCount += 1;
                player._stat.lukStatCount += 1;
                player._stat.luk += 1;
                break;
        }
        ApplyDerivedOnStatChanged(player, num, +1);
        player.statPoint -= cost;
        PlayerStatSetting(true, player);
        StatBtnUISetting(true, player);
        StatChange(player);
        GameCanvas.instance.SliderEquipChange();
    }
    #region 스텟 포인트의 대한 함수
    // 다음 +1에 필요한 포인트 비용 (현재값 -> 현재값+1)
    int GetCostForNextPoint_WithBlock(int currentStat, int blockSize)
    {
        int nextVal = currentStat + 1;
        return ((nextVal - 1) / blockSize) + 1;
    }

    // 현재 수치에서 -1 할 때 환급 포인트
    int GetRefundForCurrentPoint_WithBlock(int currentStat, int blockSize)
    {
        if (currentStat <= 0) return 0;
        return ((currentStat - 1) / blockSize) + 1;
    }

    int BlockSizeForStatIndex(int num)
    {
        // INT(1)만 3칸 단위, 나머지는 5칸 단위
        // LUK(4)는 블록 규칙 미사용(별도 n+2 / n+1 규칙 적용)
        if (num == 1) return 3;  // INT
        return 5;                // STR, CON, DEX (LUK는 이 값 안 씀)
    }

    int GetCostForNextPoint_StatIndex(int num, PlayerData p)
    {
        if (num == 4) // LUK 특수 규칙
            return GetCostForNextPoint_Luk(p._stat.luk);

        int block = BlockSizeForStatIndex(num); // INT=3칸, 그 외=5칸
        int cur = 0;
        switch (num)
        {
            case 0: cur = p._stat.str; break;
            case 1: cur = p._stat.intellect; break;
            case 2: cur = p._stat.condition; break;
            case 3: cur = p._stat.dex; break;
        }
        return GetCostForNextPoint_WithBlock(cur, block);
    }

    int GetRefundForCurrentPoint_StatIndex(int num, PlayerData p)
    {
        if (num == 4) // LUK 특수 규칙
            return GetRefundForCurrentPoint_Luk(p._stat.luk);

        int block = BlockSizeForStatIndex(num);
        int cur = 0;
        switch (num)
        {
            case 0: cur = p._stat.str; break;
            case 1: cur = p._stat.intellect; break;
            case 2: cur = p._stat.condition; break;
            case 3: cur = p._stat.dex; break;
        }
        return GetRefundForCurrentPoint_WithBlock(cur, block);
    }

    // LUK 전용: 다음 +1 비용 (cur = n) => n + 2
    int GetCostForNextPoint_Luk(int currentLuk)
    {
        return currentLuk + 2;
    }


    // LUK 전용: 현재 수치에서 -1 할 때 환급 (cur = k) => k + 1
    int GetRefundForCurrentPoint_Luk(int currentLuk)
    {
        if (currentLuk <= 0) return 0;
        return currentLuk + 1;
    }
    #endregion

    #region 스텟 포인트 상승 및 하강시 스텟 올라가게 하는 함수
    // statIndex: 0=STR, 1=INT, 2=CON, 3=DEX, 4=LUK
    // delta: +1 (증가) or -1 (감소)
    void ApplyDerivedOnStatChanged(PlayerData p, int statIndex, int delta)
    {
        switch (statIndex)
        {
            case 0: // STR
                p.attack += 0.1f * delta;
                break;
            case 1: // INT
                p.skillDamageBonus += 0.2f * delta;
                break;
            case 2: // CON
                p.hp += 2f * delta;
                if (p.hp < 1f) p.hp = 1f; // 안전클램프(선택)
                currentPlayerData.currentHp += 2f * delta;

                ApplyConDexDefenceMilestoneDelta(p, 2, delta);
                break;
            case 3: // DEX
                p.stamina += 2f * delta;
                if (p.stamina < 0f) p.stamina = 0f; // 안전클램프(선택)
                currentPlayerData.currentStamina += 2f * delta;

                ApplyConDexDefenceMilestoneDelta(p, 3, delta);

                break;
            case 4: // LUK
                p.critcleDmg += 1 * delta;
                p.critcleRate += 0.5f * delta;
                break;
        }
    }

    // CON/DEX로부터 "공통 4단위 달성 단계"를 계산 (0,1,2,...) 
    static int ConDexMilestone(int con, int dex)
    {
        con = Mathf.Max(con, 0);
        dex = Mathf.Max(dex, 0);
        return Mathf.Min(con / 4, dex / 4);
    }

    // 이전 단계 → 현재 단계 변화만큼 방어력 반영 (±0.5 * 변화량)
    static void ApplyConDexDefenceMilestoneDelta(PlayerData p, int changedStatIndex, int delta)
    {
        // 공통 4단위 달성( CON 변경, Dex 변경)
        int newCon = Mathf.Max(p._stat.condition, 0);
        int newDex = Mathf.Max(p._stat.dex, 0);

        int prevCon = newCon;
        int prevDex = newDex;

        if (changedStatIndex == 2)        // CON이 이번에 바뀜
            prevCon = Mathf.Max(newCon - delta, 0);
        else if (changedStatIndex == 3)   // DEX가 이번에 바뀜
            prevDex = Mathf.Max(newDex - delta, 0);

        int prevM = ConDexMilestone(prevCon, prevDex);
        int newM = ConDexMilestone(newCon, newDex);
        int stepDelta = newM - prevM;
        if (stepDelta != 0)
            p.defence += 0.5f * stepDelta;
    }
    #endregion


    #region 캐릭터 총 스텟 증가량
    void StatChange(PlayerData player)
    {      
        bool kor = GameManager.data.lanauge == LANGUAGE.KOR;

        string Hp = kor ? "체력" : "Hp";
        string STA = kor ? "스태미나" : "Stamina";
        string ATK = kor ? "공격력" : "ATK";
        string DEF = kor ? "방어력" : "DEF";
        string Skill = kor ? "스킬 데미지 보너스" : "SkillDamgeBonus";
        string CR = kor ? "치확" : "Crit%";
        string CD = kor ? "치피" : "CritDmg";
        string LUK = kor ? "운" : "LUK";

        txtCharcterStatData[0].text = Hp + " : " + currentPlayerData.currentHp.ToString() + " / " + player.hp.ToString();
        txtCharcterStatData[1].text = STA + " : " + currentPlayerData.currentStamina.ToString() + " / " + player.stamina.ToString();
        txtCharcterStatData[2].text = ATK + " : " + (player.attack).ToString();
        txtCharcterStatData[3].text = DEF + " : " + (player.defence).ToString();
        txtCharcterStatData[4].text = Skill + " : " + (player.skillDamageBonus).ToString();
        txtCharcterStatData[5].text = CR + " : " + (player.critcleRate).ToString();
        txtCharcterStatData[6].text = CD + " : " + (player.critcleDmg).ToString();
        txtCharcterStatData[7].text = LUK + " : " + (player._stat.luk).ToString();
    }
    #endregion

    #region 스텟 저장 및 취소
    void StatPointOnClickEvent(int num)
    {
        switch(num)
        {
            case 0:
                OnClick_StatSave();
                break;
            case 1:
                OnClick_StatCancel();
                break;
        }
    }

    public void OnClick_StatSave()
    {
        var player = PlayerManager.instance.player;

        // 현재 상태를 '저장된 기준'으로 확정
        CommitStatSession(player);

        // UI 갱신
        PlayerStatSetting(true, player);
        StatBtnUISetting(true, player);
        StatChange(player);
        GameCanvas.instance.SliderEquipChange();
    }


    public void OnClick_StatCancel()
    {
        var player = PlayerManager.instance.player;

        // 마지막 저장(또는 패널 오픈) 상태로 롤백
        CancelStatSession(player);

        // UI 갱신
        PlayerStatSetting(true, player);
        StatBtnUISetting(true, player);
        StatChange(player);
        GameCanvas.instance.SliderEquipChange();
    }

    // 스냅샷 캡처/복원 유틸
    StatSnapshot CaptureSnapshot(PlayerData p)
    {
        return new StatSnapshot
        {
            level = p.level,
            statPoint = p.statPoint,

            str = p._stat.str,
            intellect = p._stat.intellect,
            condition = p._stat.condition,
            dex = p._stat.dex,
            luk = p._stat.luk,

            strStatCount = p._stat.strStatCount,
            intellectStatCount = p._stat.intellectStatCount,
            conditonStatCount = p._stat.conditonStatCount,
            dexStatCount = p._stat.dexStatCount,
            lukStatCount = p._stat.lukStatCount,

            hp = p.hp,
            stamina = p.stamina,
            attack = p.attack,
            defence = p.defence,
            critcleRate = p.critcleRate,
            critcleDmg = p.critcleDmg,
            skillDamageBonus = p.skillDamageBonus,

            curHp = currentPlayerData != null ? currentPlayerData.currentHp : p.hp,
            curStamina = currentPlayerData != null ? currentPlayerData.currentStamina : p.stamina,
        };
    }

    void RestoreFromSnapshot(ref PlayerData p, in StatSnapshot s)
    {
        p.level = s.level;
        p.statPoint = s.statPoint;

        p._stat.str = s.str;
        p._stat.intellect = s.intellect;
        p._stat.condition = s.condition;
        p._stat.dex = s.dex;
        p._stat.luk = s.luk;

        p._stat.strStatCount = s.strStatCount;
        p._stat.intellectStatCount = s.intellectStatCount;
        p._stat.conditonStatCount = s.conditonStatCount;
        p._stat.dexStatCount = s.dexStatCount;
        p._stat.lukStatCount = s.lukStatCount;

        p.hp = s.hp;
        p.stamina = s.stamina;
        p.attack = s.attack;
        p.defence = s.defence;
        p.critcleRate = s.critcleRate;
        p.critcleDmg = s.critcleDmg;
        p.skillDamageBonus = s.skillDamageBonus;

        if (currentPlayerData != null)
        {
            currentPlayerData.currentHp = s.curHp;
            currentPlayerData.currentStamina = s.curStamina;
        }

        // 세션 증가분 초기화
        p._stat.StatCountRest();
    }
    // 패널 세션 시작: 스냅샷 만들고 세션 카운트 0으로
    void BeginStatSession(PlayerData p)
    {
        _lastSaved = CaptureSnapshot(p);
        _sessionActive = true;
        p._stat.StatCountRest();
    }

    // 저장: 현 상태를 스냅샷으로 갱신 + 세션 카운트 초기화
    void CommitStatSession(PlayerData p)
    {
        _lastSaved = CaptureSnapshot(p);
        p._stat.StatCountRest();
        _sessionActive = true;

        // ※ 실제 파일 저장까지 원하면 여기서 PM.Save 같은 함수 호출
        // PM.SavePlayerData(); // 저장 로직이 있으면 연결
    }

    // 취소: 스냅샷으로 롤백
    void CancelStatSession(PlayerData p)
    {
        RestoreFromSnapshot(ref p, _lastSaved);
        _sessionActive = true; // 세션은 유지 (계속 수정 가능)
    }
    #endregion
}

// 세이브 스냅샷: '저장 직후' or '패널 open 시점'의 기준 상태
[Serializable]
struct StatSnapshot
{
    // PlayerData 쪽
    public int level;
    public int statPoint;

    public int str, intellect, condition, dex, luk;
    public int strStatCount, intellectStatCount, conditonStatCount, dexStatCount, lukStatCount;

    public float hp, stamina, attack, defence, critcleRate, critcleDmg, skillDamageBonus;

    // Player(런타임) 쪽 현재 수치 (UI에 같이 보여주던 값)
    public float curHp, curStamina;
}

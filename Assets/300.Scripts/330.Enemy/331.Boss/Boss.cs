using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState { Idle, Telegraph, Attack, Recover, Stunned }


public class Boss : MonoBehaviour
{
    [Header("보스 테이블 인덱스 (CSV의 Index 컬럼)")]
    public int bossIndex = 1;

    [Header("런타임 스탯(읽기전용 확인용)")]
    public string bossNameEng;
    public string bossNameKor;

    public int addExp;
    public int addCoin;

    public float maxHp;
    public float currentHp;
    public float power;
    public float defense;
    public float guardGauge;
    public float stunRecoveryTimer;

    public bool Enraged { get; private set; }

    [Header("레퍼런스")]
    public Transform player;
    public Animator anim;
    public BossPatternRunner runner;

    public BossState state { get; private set; } = BossState.Idle;

    public void SetEnraged(bool v) => Enraged = v;

    public void SetState(BossState s) => state = s;

    private void Awake()
    {
        TryLoadFromTable(bossIndex);
    }

    public bool TryLoadFromTable(int index)
    {
        if (ExcelCsvReader.instance == null)
        {
            Debug.LogError("[Boss] ExcelCsvReader.instance 가 없습니다. 씬에 ExcelCsvReader를 배치/활성화 해주세요.");
            return false;
        }

        var table = ExcelCsvReader.instance.stringBossTable;
        if (table == null || table.rows == null || table.rows.Count == 0)
        {
            Debug.LogError("[Boss] BossStringTable이 비었습니다. ExcelCsvReader에서 CSV 로드를 확인하세요.");
            return false;
        }

        if (!table.rows.TryGetValue(index, out var row))
        {
            Debug.LogError($"[Boss] Index={index} 를 테이블에서 찾을 수 없습니다.");
            return false;
        }

        ApplyRow(row);
        return true;
    }

    void ApplyRow(BossStringTableRow row)
    {
        bossIndex = row.Index;

        bossNameEng = row.NameEng;
        bossNameKor = row.NameKor;

        maxHp = Mathf.Max(1f, row.Hp);
        currentHp = maxHp;

        power = row.Power;
        defense = row.Defense;
        guardGauge = row.GuardGauge;
        stunRecoveryTimer = Mathf.Max(0f, row.StunRecoveryTimer);

        addExp = row.AddExp;
        addCoin = row.AddCoin;
      
        // 디버그 로그
        Debug.Log($"[Boss] Loaded: #{bossIndex} {bossNameEng}/{bossNameKor} HP={maxHp}, POW={power}, DEF={defense}, GG={guardGauge}, StunRecov={stunRecoveryTimer}, Exp+={addExp}, Coin+={addCoin}");
    }


}

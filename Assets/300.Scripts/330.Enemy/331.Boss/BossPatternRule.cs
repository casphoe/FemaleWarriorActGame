using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossPatternRuleEntry
{
    public string patternId;                     // "JumpSmash", "TeleportStrike", "BerserkRage" ...
    [Range(0f, 10f)] public float weightMultiplier = 1f; // 최종 가중치 = pattern.weight * multiplier

    [Header("HP 조건(%)")]
    [Range(0f, 1f)] public float minHpPct = 0f;
    [Range(0f, 1f)] public float maxHpPct = 1f;

    [Header("분노 조건")]
    public bool requiresEnraged = false;        // 분노일 때만 허용
    public bool forbidWhenEnraged = false;      // 분노 중 금지

    [Header("해당 패턴 쿨타임")]
    public float cooldown = 3.0f;

    [Header("패턴 지속 시간")]
    public float remainTime = 3.0f;

    public bool enabled = true;                 // 편의 토글
}

[System.Serializable]
public class BossPatternRule
{
    public int bossIndex;                        // 예: 1 = 고블린 킹
    public List<BossPatternRuleEntry> entries;  // 해당 보스가 사용할 패턴들
}

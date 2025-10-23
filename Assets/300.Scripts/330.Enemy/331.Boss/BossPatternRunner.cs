using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternRunner : MonoBehaviour
{
    [Header("Owner Boss (필수)")]
    public Boss boss; // Inspector에 할당 또는 GetComponent

    [Header("보스별 패턴 규칙 (BossPatternRule)")]
    public List<BossPatternRule> rules = new List<BossPatternRule>();

    [Header("공통 연출 시간(패턴 전역 기본값)")]
    [Tooltip("전조(텔레그래프) 연출 시간")]
    public float defaultTelegraphTime = 0.5f;
    [Tooltip("패턴 종료 후 회수 시간")]
    public float defaultRecoverTime = 0.4f;

    [Header("JumpSmash 설정")]
    public Transform player;                  // 보스가 노리는 대상
    public LayerMask groundMask;              // 지면 레이어
    public GroundShockwave2D shockwavePrefab; // 앞서 만든 파동 프리팹 할당
    public float jumpDuration = 0.8f;         // 점프~착지까지 시간
    public float arcHeight = 3.5f;            // 포물선 고도
    public float predictTime = 0.25f;         // 착지 위치 예측 시간(플레이어 속도 보정)
    public float postLandDelay = 0.2f;        // 착지 후 약간의 텀
    public int shockwaveBaseDamage = 20;      // 파동 기본 데미지
    public float shockwaveSpeed = 14f;
    public float shockwaveLength = 12f;
    public float shockwaveLife = 0.7f;

    // 러너 전체 간격(원하면 0으로 둬도 됨). 패턴 고유 쿨타임은 _patternCooldowns로 별도 관리
    [Header("러너 전역 간격(옵션)")]
    public float globalSpacing = 0.0f;

    float _globalTimer;                                  // 러너 전역 쿨타임
    readonly Dictionary<string, float> _patternCooldowns = new(); // 개별 패턴 쿨타임
    public bool Ready => _globalTimer <= 0f;             // 러너가 다음 패턴을 받을 수 있는가?

    void Awake()
    {
        if (!boss) boss = GetComponent<Boss>();
    }

    void Update()
    {
        if (_globalTimer > 0f) _globalTimer -= Time.deltaTime;

        // 개별 패턴 쿨타임 감소
        if (_patternCooldowns.Count > 0)
        {
            var keys = new List<string>(_patternCooldowns.Keys);
            foreach (var k in keys)
            {
                _patternCooldowns[k] -= Time.deltaTime;
            }
        }
    }

    // 외부에서 호출: 인덱스 규칙 기반 자동 선택 & 실행
    public void TriggerAuto()
    {
        if (!Ready || boss == null) return;

        var entry = PickEntryForBoss(boss);
        if (entry == null)
        {
            Debug.LogWarning($"[Runner] bossIndex={boss.bossIndex} 에 사용할 패턴 엔트리가 없습니다.");
            return;
        }

        StartCoroutine(RunByEntry(entry, boss));
    }

    // 인덱스 규칙 기반 후보 고르기(HP/분노/쿨타임/가중치)
    BossPatternRuleEntry PickEntryForBoss(Boss b)
    {
        // 1) 규칙 찾기
        var rule = rules.Find(r => r.bossIndex == b.bossIndex);
        if (rule == null || rule.entries == null || rule.entries.Count == 0) return null;

        float hpPct = (b.maxHp > 0f) ? (b.currentHp / b.maxHp) : 1f;
        bool enraged = b.Enraged;

        // 2) 조건 & 쿨타임 필터
        List<(BossPatternRuleEntry e, float w)> candidates = new();
        foreach (var e in rule.entries)
        {
            if (e == null || !e.enabled) continue;
            if (hpPct < e.minHpPct || hpPct > e.maxHpPct) continue;
            if (e.requiresEnraged && !enraged) continue;
            if (e.forbidWhenEnraged && enraged) continue;

            // 개별 패턴 쿨타임 체크
            float cd = 0f;
            _patternCooldowns.TryGetValue(e.patternId, out cd);
            if (cd > 0f) continue;

            // 가중치(여기선 multiplier 자체를 가중치로 사용)
            float w = Mathf.Max(0f, e.weightMultiplier);
            if (w > 0f) candidates.Add((e, w));
        }

        if (candidates.Count == 0) return null;

        // 3) 가중치 랜덤
        float sum = 0f; foreach (var c in candidates) sum += c.w;
        float r = Random.value * sum;
        foreach (var c in candidates)
        {
            r -= c.w;
            if (r <= 0f) return c.e;
        }
        return candidates[0].e;
    }

    // 선택된 엔트리로 실행
    IEnumerator RunByEntry(BossPatternRuleEntry entry, Boss owner)
    {
        // 전조
        owner.SetState(BossState.Telegraph);
        owner.anim?.Play($"{entry.patternId}_Telegraph");
        yield return new WaitForSeconds(defaultTelegraphTime);

        // 공격 본체
        owner.SetState(BossState.Attack);
        yield return StartCoroutine(DoAttackById(entry.patternId, owner, entry.remainTime));

        // 회수
        owner.SetState(BossState.Recover);
        yield return new WaitForSeconds(defaultRecoverTime);

        // 쿨타임 설정: 개별 패턴 쿨타임 + 전역 간격
        _patternCooldowns[entry.patternId] = Mathf.Max(0f, entry.cooldown);
        _globalTimer = Mathf.Max(_globalTimer, globalSpacing);

        owner.SetState(BossState.Idle);
    }

    // 패턴 식별자별 실행
    IEnumerator DoAttackById(string patternId, Boss owner, float remainTime)
    {
        switch (patternId)
        {
            case "JumpSmash":
                yield return JumpSmash(owner);
                break;

            case "TeleportStrike":
                yield return TeleportStrike(owner);
                break;

            case "BerserkRage":
                yield return BerserkRage(owner, remainTime);
                break;

            // TODO: DashStab / TrapPlant / Boomerang 등 필요 시 추가
            default:
                Debug.LogWarning($"[Runner] 알 수 없는 patternId: {patternId}");
                break;
        }
    }

    // JumpSmash
    IEnumerator JumpSmash(Boss b)
    {
        float vx = 0f;
        var prb = player ? player.GetComponent<Rigidbody2D>() : null;
        if (prb) vx = prb.velocity.x;

        float targetX = player ? player.position.x + vx * predictTime : b.transform.position.x;

        Vector2 rayStart = new Vector2(targetX, b.transform.position.y + 8f);
        var hit = Physics2D.Raycast(rayStart, Vector2.down, 20f, groundMask);
        float targetY = hit ? hit.point.y + 0.01f : b.transform.position.y;

        Vector2 end = new Vector2(targetX, targetY);

        b.anim?.Play("JumpSmash_Jump");
        yield return MoveParabola(b.transform, end, jumpDuration, arcHeight);

        b.anim?.Play("JumpSmash_Land");
        SpawnShockwave(end);
        yield return new WaitForSeconds(postLandDelay);
    }

    void SpawnShockwave(Vector2 impactPoint)
    {
        if (!shockwavePrefab) return;

        var right = Instantiate(shockwavePrefab, impactPoint, Quaternion.identity);
        right.baseDamage = shockwaveBaseDamage;
        right.speed = shockwaveSpeed;
        right.maxLength = shockwaveLength;
        right.lifeTime = shockwaveLife;
        right.groundMask = groundMask;
        right.Fire(impactPoint, +1f);

        var left = Instantiate(shockwavePrefab, impactPoint, Quaternion.identity);
        left.baseDamage = shockwaveBaseDamage;
        left.speed = shockwaveSpeed;
        left.maxLength = shockwaveLength;
        left.lifeTime = shockwaveLife;
        left.groundMask = groundMask;
        left.Fire(impactPoint, -1f);
    }

    public static IEnumerator MoveParabola(Transform target, Vector2 end, float duration, float arcHeight)
    {
        Vector2 start = target.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, duration);
            float u = Mathf.Clamp01(t);

            float x = Mathf.Lerp(start.x, end.x, u);
            float y = Mathf.Lerp(start.y, end.y, u);
            y += arcHeight * (4f * u * (1f - u));

            target.position = new Vector2(x, y);
            yield return null;
        }
        target.position = end;
    }

    // 텔레포트 스트라이크
    IEnumerator TeleportStrike(Boss b)
    {
        // 텔레포트 아웃
        b.anim?.Play("TeleportStrike_Out");
        yield return new WaitForSeconds(0.12f);

        // 플레이어 뒤/옆 위치로 이동
        Vector2 target = player ? (Vector2)player.position + new Vector2(Random.value < 0.5f ? -1.8f : 1.8f, 0f)
                                : (Vector2)b.transform.position;
        b.transform.position = target;

        // 타격
        b.anim?.Play("TeleportStrike_Hit");
        // TODO: 히트박스 Enable & 데미지 적용
        yield return new WaitForSeconds(0.25f);

        // 잔상/딜레이
        yield return new WaitForSeconds(0.1f);
    }

    // 광폭화 (remainTime 동안 분노 유지/버프)
    IEnumerator BerserkRage(Boss b, float remainTime)
    {
        b.anim?.Play("BerserkRage_Start");
        b.SetEnraged(true);

        // 예시: 버프 적용 (원하는 방식으로 교체)
        // b.ApplyTempBuff(powerMul: 1.25f, defenseMul: 0.9f, duration: remainTime);

        float t = Mathf.Max(0f, remainTime);
        while (t > 0f)
        {
            // 분노 상태에서의 반복 행동(간단 예: 으르렁/근접휘두르기 등)
            // TODO: 필요 시 패턴 내 소패턴 루프 추가
            t -= Time.deltaTime;
            yield return null;
        }

        b.anim?.Play("BerserkRage_End");
        b.SetEnraged(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternRunner : MonoBehaviour
{
    [Header("패턴 목록 (ScriptableObject)")]
    public List<BossPattern> patterns;

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

    float _cooldownTimer;
    public bool Ready => _cooldownTimer <= 0f;


    void Update()
    {
        if (_cooldownTimer > 0f) _cooldownTimer -= Time.deltaTime;
    }

    public BossPattern PickPattern(int phase)
    {
        // phase에 맞는 것만 후보
        var cands = patterns.FindAll(p => phase >= p.phaseMin && phase <= p.phaseMax);
        // 가중치 랜덤
        int sum = 0; foreach (var c in cands) sum += c.weight;
        int r = Random.Range(0, sum);
        foreach (var c in cands) { r -= c.weight; if (r < 0) return c; }
        return cands[0];
    }

    public IEnumerator Run(BossPattern p, GoblinBoss boss)
    {
        boss.SetState(BossState.Telegraph);
        // 전조 연출(애니/사운드/바닥 텔레그래프)
        boss.anim.Play($"{p.patternId}_Telegraph");
        yield return new WaitForSeconds(p.telegraphTime);

        boss.SetState(BossState.Attack);
        // 실제 공격 실행
        yield return StartCoroutine(DoAttack(p, boss));

        boss.SetState(BossState.Recover);
        yield return new WaitForSeconds(p.recoverTime);

        _cooldownTimer = p.cooldown;
        boss.SetState(BossState.Idle);
    }

    IEnumerator DoAttack(BossPattern p, GoblinBoss boss)
    {
        switch (p.patternId)
        {
            case "JumpSmash": yield return JumpSmash(boss); break;
            //case "BombLob": yield return BombLob(boss, 2); break;
            //case "DashStab": yield return DashStab(boss); break;
            //case "TrapPlant": yield return TrapPlant(boss); break;
            //case "Boomerang": yield return Boomerang(boss); break;
        }
    }
    // 여기부터 JumpSmash 본체 (Rigidbody 없이 Transform으로 이동)
    IEnumerator JumpSmash(GoblinBoss b)
    {
        // 1) 플레이어 예측 X
        float vx = 0f;
        var prb = player ? player.GetComponent<Rigidbody2D>() : null;
        if (prb) vx = prb.velocity.x;

        float targetX = player ? player.position.x + vx * predictTime : b.transform.position.x;

        // 2) 레이캐스트로 지면 Y 찾기 (못 찾으면 현재 y)
        Vector2 rayStart = new Vector2(targetX, b.transform.position.y + 8f);
        var hit = Physics2D.Raycast(rayStart, Vector2.down, 20f, groundMask);
        float targetY = hit ? hit.point.y + 0.01f : b.transform.position.y;

        Vector2 start = b.transform.position;
        Vector2 end = new Vector2(targetX, targetY);

        // 3) 포물선 이동 (Transform만 사용)
        b.anim.Play("JumpSmash_Jump");
        yield return MoveParabola(b.transform, end, jumpDuration, arcHeight);

        // 4) 착지 연출 & 충격파
        b.anim.Play("JumpSmash_Land");
        SpawnShockwave(end);
        yield return new WaitForSeconds(postLandDelay);
    }

    // 착지 지점에서 좌/우 파동 생성
    void SpawnShockwave(Vector2 impactPoint)
    {
        if (!shockwavePrefab) return;

        // 오른쪽
        var right = Instantiate(shockwavePrefab, impactPoint, Quaternion.identity);
        right.baseDamage = shockwaveBaseDamage;
        right.speed = shockwaveSpeed;
        right.maxLength = shockwaveLength;
        right.lifeTime = shockwaveLife;
        right.groundMask = groundMask;             // 지면 추적용
        // right.enemyMask는 프리팹에서 설정해도 됨
        right.Fire(impactPoint, +1f);

        // 왼쪽
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

            // x, y 선형 보간
            float x = Mathf.Lerp(start.x, end.x, u);
            float y = Mathf.Lerp(start.y, end.y, u);

            // 포물선 고도 추가 : u(1-u)의 최대는 0.25 → *4로 0~1 스케일
            y += arcHeight * (4f * u * (1f - u));

            target.position = new Vector2(x, y);
            yield return null;
        }

        target.position = end;
    }
}

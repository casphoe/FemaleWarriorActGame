using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//일정 범위 순찰, 공격, 죽음, 플레이어 발견시 추적
public enum State
{
    Patrol, Attack,Death, Chase
}

public class Enemy : MonoBehaviour
{
    //0 : 슬라임, 1 : 박쥐, 2 : 쥐, 3: 동굴게, 4 : 이블아이, 5 : 머시룸, 6 : 고블린, 7 : 해골, 8 : 스켈레톤 , 9 : 골렘, 10 : 갑옷 골렘
    public int id;
    [Header("구글 스프레트 시트에서 가져온 적 데이터 - 변하지 않음")]
    public float hp;
    public float attack;
    public float defence;
    public int addMoney;
    public int addExp;
    [Header("난이도에 따른 적 데이터")]
    public float currentHp;
    public float maxHp;
    public float currentAttack;
    public float currentDefence;
    public int currentAddMoney;
    public int currentAddExp;
    #region 적 상태 설정
    public State enemyState = State.Patrol;

    [Header("AI Settings")]
    public float detectionRange = 2f;
    public float moveSpeed = 2f;

    [Header("Patrol Path")]
    public List<Vector2> patrolPoints; // 순찰 경로 직접 지정

    [Header("Chase Path")]
    public List<Vector2> chasePoints;

    private int currentPatrolIndex = 0;
    public float patrolWaitTime = 2f;

    private Vector2 patrolTarget;
    private float waitTimer;
    private bool isFacingRight = true;

    float chasePersistTime = 2f;
    float chaseTimer = 0f;

    public bool gotHit = false;

    #endregion
    public Slider hpSlider;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sprite;

    private Day lastCheckedDay;

    private float baseMultiplier = 1f; // 난이도 계수 저장용

    #region 적 데이터 관리 
    //EnemyManager에서 적 데이터 가져오는 함수
    public void Init(EnemyData data)
    {
        id = data.id;
        hp = data.hp;
        attack = data.attack;
        defence = data.defence;
        addMoney = data.addMoney;
        addExp = data.addExp;
        EnemyDifficuitySetting();
        ApplyTimeMultiplier();
    }

    //난이도에 따른 적 데이터 설정
    void EnemyDifficuitySetting()
    {
        switch (GameManager.data.diffucity)
        {
            case Diffucity.Easy:
                baseMultiplier = 0.5f;
                break;
            case Diffucity.Normal:
                baseMultiplier = 1f;
                break;
            case Diffucity.Hard:
                baseMultiplier = 1.5f;
                break;
        }
    }

    //시간에 따라서 적 강도 설정
    void ApplyTimeMultiplier()
    {
        float timeMultiplier = (GameManager.data.day == Day.Night) ? 1.5f : 1f;

        float oldMaxHp = maxHp;
        float hpRatio = (oldMaxHp > 0f) ? currentHp / oldMaxHp : 1f;

        // 능력치 재계산
        maxHp = hp * baseMultiplier * timeMultiplier;
        currentHp = maxHp * hpRatio; //  비율 그대로 반영

        // 원본 * 난이도 * 시간
        currentAttack = attack * baseMultiplier * timeMultiplier;
        currentDefence = defence * baseMultiplier * timeMultiplier;
        currentAddMoney = Mathf.RoundToInt(addMoney * baseMultiplier * timeMultiplier);
        currentAddExp = Mathf.RoundToInt(addExp * baseMultiplier * timeMultiplier);

        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        lastCheckedDay = GameManager.data.day;
        patrolTarget = patrolPoints.Count > 0 ? patrolPoints[0] : transform.localPosition;
    }

    private void Update()
    {
        if (enemyState == State.Death) return;


        if (GameManager.data.day != lastCheckedDay)
        {
            ApplyTimeMultiplier();
            lastCheckedDay = GameManager.data.day;
        }

        bool playerInSight = IsPlayerInSight();

        // 상태 갱신
        switch (enemyState)
        {
            case State.Patrol:
                if (gotHit || playerInSight)
                {
                    enemyState = State.Chase;
                    return;
                }
                Patrol();
                break;

            case State.Chase:
                if (!playerInSight)
                {
                    // 플레이어가 감지 범위를 벗어나야만 gotHit 해제
                    gotHit = false;
                    enemyState = State.Patrol;
                    return;
                }
                ChasePlayer();
                break;
        }
    }

    #region 이동 및 추적
    //순찰
    void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector2 target = patrolPoints[currentPatrolIndex];
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);

        // Y 고정한 방향 계산
        Vector2 moveTarget = new Vector2(target.x, currentPos.y);
        Vector2 dir = (moveTarget - currentPos).normalized;
        // 너무 가까우면 움직이지 않음
        if ((moveTarget - currentPos).magnitude > 0.01f)
        {
            MoveInDirection(dir);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (Mathf.Abs(transform.position.x - target.x) < 0.1f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                waitTimer = 0f;
            }
        }
    }
    //적이 플레이어 감지 함수
    bool IsPlayerInSight()
    {
        Vector2 toPlayer = (player.position - transform.position);
        float dist = toPlayer.magnitude;
        if (dist > detectionRange) return false;

        Vector2 facing = isFacingRight ? Vector2.right : Vector2.left;
        float dot = Vector2.Dot(facing, toPlayer.normalized);

        return dot > 0.2f; // 1이면 정면, 0은 수직, -1은 반대
    }
    //추적 함수
    void ChasePlayer()
    {
        if (player == null && PlayerManager.instance.IsDead == true) return;

        Vector2 playerPos = new Vector2(player.position.x, transform.position.y); // Y 고정
        Vector2 dir = (playerPos - (Vector2)transform.position);

        if (dir.magnitude < 0.05f)
        {
            anim?.SetBool("isMoving", false);
            return;
        }

        dir.Normalize();
        MoveInDirection(dir);
    }

    bool IsPlayerWithinChaseBounds()
    {
        if (chasePoints == null || chasePoints.Count < 2)
            return true; // chasePoints가 비정상일 경우 무제한 추적

        float minX = Mathf.Min(chasePoints[0].x, chasePoints[1].x);
        float maxX = Mathf.Max(chasePoints[0].x, chasePoints[1].x);

        float playerX = player.position.x;

        return playerX >= minX && playerX <= maxX;
    }

    void MoveInDirection(Vector2 dir)
    {
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        if (dir.x != 0)
        {
            bool shouldFaceRight = dir.x > 0;
            if (shouldFaceRight != isFacingRight)
                Flip();
        }

        if (dir.magnitude < 0.01f)
        {
            anim.SetBool("isMoving", false);
            return;
        }

        anim?.SetBool("isMoving", true);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    #endregion

    public void TakeDamage(float damage)
    {
        // 방어력보다 데미지가 낮을 경우에도 최소 50%는 받도록 처리
        float reducedDamage = damage - currentDefence;

        // 데미지 최소 50% 보장
        float finalDamage = (reducedDamage > 0) ? reducedDamage : damage * 0.5f;

        currentHp -= finalDamage;
        gotHit = true;

        if (currentHp <= 0)
        {
            currentHp = 0;
            enemyState = State.Death;
            anim.SetTrigger("Death");
            Utils.OnOff(gameObject, false);
        }
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp / maxHp;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 감지 반경 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 시야 방향 (빨간 선)
        Gizmos.color = Color.red;
        Vector3 facingDir = isFacingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(transform.position, transform.position + facingDir * detectionRange);

        // 시야 부채꼴 (Dot 조건 시각화) - 파란 선으로 부채꼴 그리기
        Gizmos.color = Color.cyan;
        float angleRange = 45f; // 시야 각도 (총 90도)
        int steps = 10;

        for (int i = -steps; i <= steps; i++)
        {
            float angle = angleRange * i / steps;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 dir = rot * facingDir;
            Gizmos.DrawLine(transform.position, transform.position + dir.normalized * detectionRange);
        }
    }
}
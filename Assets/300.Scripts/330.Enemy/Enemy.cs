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
    public List<Transform> patrolPoints; // 순찰 경로 직접 지정
    private int currentPatrolIndex = 0;
    public float patrolWaitTime = 2f;

    private Vector2 patrolTarget;
    private float waitTimer;
    private bool isFacingRight = true;

    private bool gotHit = false;

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
        patrolTarget = patrolPoints.Count > 0 ? patrolPoints[0].position : transform.position;
    }

    private void Update()
    {
        if (enemyState == State.Death) return;


        if (GameManager.data.day != lastCheckedDay)
        {
            ApplyTimeMultiplier();
            lastCheckedDay = GameManager.data.day;
        }
    }

    #region 이동 및 해동
    void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Transform target = patrolPoints[currentPatrolIndex];
        Vector2 dir = (target.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                waitTimer = 0f;
            }
        }
    }
    #endregion
}
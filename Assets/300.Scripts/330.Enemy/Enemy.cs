﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//일정 범위 순찰, 공격, 죽음, 플레이어 발견시 추적
/*
 * 순찰 -> 플레이어 감지 or 피격시 추적 -> 시야에서 놓치거나 범위 벗어나면 다시 순찰
 */
public enum State
{
    Patrol, Attack,Death, Chase, Stun
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
    public float critcleRate;
    public float critcleDmg;
    public string attackPattern;
    public float attackRange;
    public float guardValue;
    public float guardRecoverycoolTime;
    public float guardRecoveryValue;

    [Header("난이도에 따른 적 데이터")]
    public float currentHp;
    public float maxHp;
    public float currentAttack;
    public float currentDefence;
    public int currentAddMoney;
    public int currentAddExp;
    public float currentCritcleRate;
    public float currentCritcleDmg;
    public float currentAttackRange;
    public float currentGuardValue;
    public float currentMaxGuardValue;
    public float currentGuardRecoverycoolTime;
    public float currentGuardRecoveryValue;

    #region 적 상태 설정
    public State enemyState = State.Patrol;

    [Header("AI Settings")]
    //적 감지 범위
    public float detectionRange = 2f;
    //적 이동 속도
    private float moveSpeed = 2f;

    private float maxMoveSpeed = 2f;
    private float minMoveSpeed = 1f;

    [Header("Patrol Path")]
    public List<Vector2> patrolPoints; // 순찰 경로 직접 지정

    [Header("Chase Path")]
    public List<Vector2> chasePoints; //위 범위 안에서만 추적 가능합니다 

    [Header("함정에 부딪혔는지 판단하는 변수")]
    public bool wasHitByTrap = false;

    private int currentPatrolIndex = 0;
    public float patrolWaitTime = 2f;

    private Vector2 patrolTarget;
    private float waitTimer;
    public bool isFacingRight = true;

    private bool isChasingPlayer = false;
    private Vector2 lastKnownPlayerPos;

    public bool gotHit = false;

    public int currentMapNum = 0;

    [Header("Attack Setting")]
    private bool isNearPlayer = false;

    public float attackTimer = 0;

    public Transform shockWaveAttackTrans;

    #endregion
    public Slider hpSlider;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sprite;
    private Text txtHp;
    private Transform hpSliderParent;
    private GuardShrinkSlider guardSlider;
    private float guardRecoverTimer;
    private float attackCoolTime = 0;

    private Day lastCheckedDay;

    private float baseMultiplier = 1f; // 난이도 계수 저장용
    private float reveseBaseMultiplier = 1;
    private bool canRecoverGuard = false;

    private GameObject currentConfusionEffect;

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
        critcleRate = data.critcleRate;
        critcleDmg = data.critcleDmg;
        attackPattern = data.attackPattern;
        attackRange = data.attackRange;
        guardValue = data.guardValue;
        guardRecoverycoolTime = data.guardRecoverycoolTime;
        guardRecoveryValue = data.guardRecoveryValue;
        currentHp = hp;
        EnemyDifficuitySetting();
        ApplyTimeMultiplier();
    }

    //난이도에 따른 적 데이터 설정
    public void EnemyDifficuitySetting()
    {
        switch (GameManager.data.diffucity)
        {
            case Diffucity.Easy:
                baseMultiplier = 0.5f;
                reveseBaseMultiplier = 1.5f;
                break;
            case Diffucity.Normal:
                baseMultiplier = 1f;
                reveseBaseMultiplier = 1f;
                break;
            case Diffucity.Hard:
                baseMultiplier = 1.5f;
                reveseBaseMultiplier = 0.5f;
                break;
        }
    }

    //시간에 따라서 적 강도 설정
    void ApplyTimeMultiplier()
    {
        float timeMultiplier = (GameManager.data.day == Day.Night) ? 1.5f : 1f;
        float reveaseTimeMultiplier = (GameManager.data.day == Day.Night) ? 0.5f : 1f;

        float oldMaxHp = maxHp; // 기존 maxHp 백업 먼저!

        maxHp = hp * baseMultiplier * timeMultiplier;

        float hpRatio = (oldMaxHp > 0f) ? currentHp / oldMaxHp : 1f;
        currentHp = maxHp * hpRatio;

        // 원본 * 난이도 * 시간
        currentAttack = attack * baseMultiplier * timeMultiplier;
        currentDefence = defence * baseMultiplier * timeMultiplier;
        currentAddMoney = Mathf.RoundToInt(addMoney * baseMultiplier * timeMultiplier);
        currentAddExp = Mathf.RoundToInt(addExp * baseMultiplier * timeMultiplier);

        currentCritcleRate = critcleRate * baseMultiplier * timeMultiplier;
        currentCritcleDmg = critcleDmg * baseMultiplier * timeMultiplier;
        currentAttackRange = attackRange * baseMultiplier * timeMultiplier;

        currentMaxGuardValue = guardValue * baseMultiplier * timeMultiplier;

        float oldMaxGuard = currentMaxGuardValue;
        float guardRatio = (oldMaxGuard > 0f) ? currentGuardValue / oldMaxGuard : 1f;

        currentGuardValue = guardValue * guardRatio;

        currentGuardRecoveryValue = guardRecoveryValue * baseMultiplier * timeMultiplier;
        currentGuardRecoverycoolTime = guardRecoverycoolTime * reveseBaseMultiplier * reveaseTimeMultiplier;

        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        txtHp.text = currentHp + " / " + maxHp;
    }

    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        isNearPlayer = false;
        txtHp = hpSlider.transform.GetChild(2).GetComponent<Text>();
        hpSliderParent = hpSlider.transform.parent;
        guardSlider = hpSliderParent.transform.GetChild(1).GetChild(0).GetComponent<GuardShrinkSlider>();
        canRecoverGuard = false;
        wasHitByTrap = false;
        attackCoolTime = 1.5f;
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
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void OnEnable()
    {
        isNearPlayer = false;
        canRecoverGuard = false;
        wasHitByTrap = false;
        attackCoolTime = 1.5f;
        ApplyTimeMultiplier();
    }

    private void Update()
    {
        if (enemyState == State.Death && enemyState == State.Stun) return;


        if (GameManager.data.day != lastCheckedDay)
        {
            ApplyTimeMultiplier(); //밤이 되면 능력치 강화 , 낮이면 되면 밤에 강화 되었던 능력치 약화
            lastCheckedDay = GameManager.data.day;
        }

        bool playerInSight = IsPlayerInSight();


        Attack(id);
        UpdateGuard();

        // Attack 상태면 이동하지 않고 즉시 리턴
        if (enemyState == State.Attack) return;

        // 상태 갱신
        switch (enemyState)
        {
            case State.Patrol:
                //플레이어가 보이거나 데미지를 받았으면 추적
                if (gotHit || playerInSight)
                {
                    lastKnownPlayerPos = player.position;
                    enemyState = State.Chase;
                    gotHit = false;
                    return;
                }
                Patrol();
                break;

            case State.Chase:
                //시야에 있는 동안 마지막 위치 갱신
                if (playerInSight)
                {
                    lastKnownPlayerPos = player.position;                  
                }             

                //시야에 없을 경우 마지막 위치에 도달 한 후 그 이후 시야에 없으면 순찰로 복귀
                if (!playerInSight && Mathf.Abs(transform.position.x - lastKnownPlayerPos.x) < 0.1f)
                {                  
                    enemyState = State.Patrol;
                    gotHit = false;
                    return;
                }

                //플레이어 위치가 chasePoints  범위를 벗어나면 즉시 순찰로 전환
                if (!IsWithinChaseRange(lastKnownPlayerPos))
                {
                    enemyState = State.Patrol;
                    gotHit = false;
                    return;
                }             

                MoveToPosition(lastKnownPlayerPos);
                break;
        }
    }

    #region 이동 및 추적
    //순찰
    void Patrol()
    {
        //순찰 지점이 하나도 없으면 함수 종료
        if (patrolPoints.Count == 0) return;

        //현재 순찰 지점
        //currentPatrolIndex 지금 가고 있는 지점의 인덱스 번호
        Vector2 target = patrolPoints[currentPatrolIndex];
        //현재 적 위치
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);

        // Y 고정한 방향 계산(x축 만 이동)
        Vector2 moveTarget = new Vector2(target.x, currentPos.y);
        //현재 위치에서 타겟까지의 방향 백터
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
        //x 축 기준으로 도착 했다고 판단
        //도착후 일정 시간 기다린 뒤 다음 지점으로 이동
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
        //적과 플레이어가 서로 다른 맵에 있을 경우 감지못하기 위해서 막는 조건
        if (Player.instance.currentMapNum != currentMapNum)
            return false;
        //적과 플레이어 방향을 나타내는 백터
        float xDistance = Mathf.Abs(player.position.x - transform.position.x);
        //플레이어가 적으로 감지 범위 바깥에 있다면 감지 불가하게 함(두점 사이의 직선 거리와 감지 범위를 통해서 false인지 true인지 반영)
        if(xDistance > detectionRange) return false;
        //적이 왼쪽/오른쪽 어느 방향을 보고 있는 판단해는 함수
        Vector2 facing = isFacingRight ? Vector2.right : Vector2.left;
        //적이 바라보는 방향과 플레이어 방향 사이의 코사인 값을 구함 1에 가까울수록 정면 0이면 90도 -1이면 완전한 반대 값
        Vector2 toPlayer = player.position - transform.position;
        float dot = Vector2.Dot(facing.normalized, toPlayer.normalized);

        //적이 바라보는 방향 기준으로 += 60도 (총 120도) 이내에 플레이어가 있으면 true를 반환
        return dot > 0.5f;
    }
    //적을 특정 위치까지 자연스럽게 이동시키는 함수
    void MoveToPosition(Vector2 target)
    {
        //목표 타겟
        Vector2 moveTarget = new Vector2(target.x, transform.position.y);
        //목표 위치까지 어느 방향으로 얼마나 가야하는지 알려주는 백터 값
        Vector2 dir = moveTarget - (Vector2)transform.position;

        //목표 지점까지 직선거리로 0.01 만큼 떨어져 잇으면 이동시키고 아니면 이동을 멈춤
        if (dir.magnitude > 0.01f)
        {
            MoveInDirection(dir.normalized);
        }
        else
        {        
            return;
        }
    }

    //지정된 추적 범위 안에 플레이어가 있는지 확인하는 함수
    bool IsWithinChaseRange(Vector2 pos)
    {
        float minX = Mathf.Min(chasePoints[0].x, chasePoints[1].x) - 1f;
        float maxX = Mathf.Max(chasePoints[0].x, chasePoints[1].x) + 1f;

        return pos.x >= minX && pos.x <= maxX;
    }

    //적이 주어진 방향으로 자연스럽게 이동시키는 함수
    void MoveInDirection(Vector2 dir)
    {
        //공격 상태일 때는 이동 애니메이션을 멈춤
        if (enemyState == State.Attack)
        {          
            return;
        }

        //프레임 속도에 관계 없이 일정한 속도로 이동
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        //좌우 방향 전환 처리
        if (dir.x != 0)
        {
            bool shouldFaceRight = dir.x > 0;
            if (shouldFaceRight != isFacingRight)
                Flip();
        }

        if (dir.magnitude < 0.01f)
        {         
            return;
        }

        anim.SetBool("isMoving", true);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        hpSliderParent.localScale = scale;
    }

    #endregion

    public void TakeDamage(float damage , float critcleRate, float critcleDmg, int num)
    {

        if (enemyState == State.Death)
            return;

        //크리티컬 판정
        int rand = Random.Range(1, 101); // 1~100 포함
        bool isCritical = false;

        if (num == 0)
        {
            currentGuardValue = 0;
            guardSlider.SetValue(0);
        }
        else
        {
            currentGuardValue -= damage;
            currentGuardValue = Mathf.Clamp(currentGuardValue, 0f, currentMaxGuardValue);

            float guardRatio = currentGuardValue / currentMaxGuardValue;
            guardSlider.SetValue(guardRatio);
        }
      

        if (currentGuardValue <= 0f && enemyState != State.Stun && enemyState != State.Death)
        {
            ApplyStun(currentGuardRecoverycoolTime, 1);
        }

        if (enemyState == State.Stun)
        {
            isCritical = true;
        }
        else
        {
            isCritical = rand <= critcleRate;
        }

        if (isCritical)
        {
            damage = damage * (critcleDmg / 100f); // 예: 150 → 1.5배
        }

        // 방어력보다 데미지가 낮을 경우에도 최소 50%는 받도록 처리
        float reducedDamage = damage - currentDefence;

        // 데미지 최소 50% 보장
        float finalDamage = (reducedDamage > 0) ? reducedDamage : damage * 0.5f;

        currentHp -= finalDamage;
        gotHit = true;

        if (isCritical)
        {
            ObjectPool.instance.SetDamageText(transform.position, 1, finalDamage);
        }
        else
        {
            ObjectPool.instance.SetDamageText(transform.position, 0, finalDamage);
        }

        switch(id)
        {
            case 0:
                SoundManager.Instance.PlaySFX("SlimeSplash");
                break;
            case 1:
                anim.SetTrigger("Hurt");
                SoundManager.Instance.PlaySFX("bat_hurt");
                break;
            case 2:
                anim.SetTrigger("Hurt");
                SoundManager.Instance.PlaySFX("rat_pain");
                break;
            case 3:
                anim.SetTrigger("Hurt");
                break;
            case 6:
                SoundManager.Instance.PlaySFX("goblin_hurt");
                break;
        }

        if (currentHp <= 0)
        {
            currentHp = 0;
            attackTimer = 0;
            attackCoolTime = 1000;
            switch (id)
            {
                case 0:
                    SoundManager.Instance.PlaySFX("Slime_death");
                    break;
                case 1:
                    break;
                case 2:
                    SoundManager.Instance.PlaySFX("rat_death");
                    break;
                case 3:
                    break;
                case 6:
                    SoundManager.Instance.PlaySFX("Goblin_Death");
                    break;
            }
            PlayerManager.instance.AddExp(addExp);
            PlayerManager.instance.AddMoney(addMoney);
            QuestManager.instance.EnemyDeathQuestChange(id);
            enemyState = State.Death;
            anim.SetTrigger("Death");
        }
        UpdateHpBar();
        guardRecoverTimer = 0f;
        canRecoverGuard = false;
    }

    public void DeathEnd()
    {
        if(currentConfusionEffect != null)
        {
            if(currentConfusionEffect.activeSelf)
            {
                currentConfusionEffect.SetActive(false);
            }
            currentConfusionEffect = null;
        }
        Utils.OnOff(gameObject, false);
    }

    void UpdateHpBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHp;
            txtHp.text = currentHp + " / " + maxHp;
        }
    }

    #region 가드 회복
    void UpdateGuard()
    {
        if (enemyState != State.Death && enemyState != State.Stun && currentGuardValue < currentMaxGuardValue)
        {
            guardRecoverTimer += Time.deltaTime;

            if (!canRecoverGuard && guardRecoverTimer >= currentGuardRecoverycoolTime)
            {
                canRecoverGuard = true;
            }

            if (canRecoverGuard)
            {
                currentGuardValue += currentGuardRecoveryValue * Time.deltaTime;
                currentGuardValue = Mathf.Clamp(currentGuardValue, 0f, currentMaxGuardValue);

                float guardRatio = currentGuardValue / currentMaxGuardValue;
                guardSlider.SetValue(guardRatio);                
            }
        }
    }
    #endregion

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

    #region 공격 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (attackPattern != "long")
                isNearPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(attackPattern != "long")
            {
                isNearPlayer = false;
                attackTimer = 0f;
            }
        }
    }

    void Attack(int id)
    {
        if (enemyState != State.Death && enemyState != State.Stun)
        {        
            //원거리 공격
            if(attackPattern == "long")
            {
                //두 오브젝트 사이의 x축 거리를 절댓값으로 계산하기 위해서 사용(플레이어와 적 사이의 수평거리)
                float xDistance = Mathf.Abs(player.position.x - transform.position.x);
                //플레이어가 적 원거리 공격 범위 안에 들어왔는지 확인
                bool isPlayerInRange = xDistance <= currentAttackRange;
                //적이 추적상태이거나 또는 플레이어가 사거리 안에 있다면 공격
                if (enemyState == State.Chase || isPlayerInRange)
                {
                    attackTimer += Time.deltaTime;
                    if(attackTimer >= attackCoolTime)
                    {
                        enemyState = State.Attack;
                        anim.SetBool("Attack", true);
                        anim.SetBool("isMoving", false);


                        // 바라보는 방향으로 발사(투사체가 날아가는 방향)
                        Vector2 dir = isFacingRight ? Vector2.right : Vector2.left;
                        ObjectPool.instance.SetSlash(shockWaveAttackTrans.position, currentAttack, currentCritcleRate, currentCritcleDmg, 3, 3, dir, 0.3f, SlashState.Enemy);
                        switch(id)
                        {
                            case 1:
                                SoundManager.Instance.PlaySFX("bat_attack");
                                break;
                        }
                        attackTimer = 0f;
                    }
                }
            }
            else
            {
                //근접 공격
                if (isNearPlayer)
                {
                    attackTimer += Time.deltaTime;

                    if (attackTimer >= attackCoolTime) // 공격 주기 조절 (1.5초 등)
                    {

                        enemyState = State.Attack;
                        anim.SetBool("Attack", true); // Animator에 "Attack" 트리거 있어야 함
                        anim.SetBool("isMoving", false);                        
                        switch(id)
                        {
                            case 0:
                                SoundManager.Instance.PlaySFX("Slime_Attack");
                                break;
                            case 2:
                                SoundManager.Instance.PlaySFX("rat_attack");
                                break;
                            case 6:
                                SoundManager.Instance.PlaySFX("Goblin_Attack");
                                break;
                        }

                        attackTimer = 0f;
                    }
                }
            }
        }
    }
    //공격 애니메이션 끝나는 타임에 Event로 등록해서 처리
    public void OnAttackEnd()
    {
        anim.SetBool("Attack", false); // 공격 애니메이션 종료
        anim.SetBool("isMoving", true);
        if (attackPattern == "long")
        {
            if (IsPlayerInSight() && Mathf.Abs(player.position.x - transform.position.x) <= currentAttackRange)
            {
                enemyState = State.Attack; // 플레이어가 범위 안에 있으면 다시 공격 준비
            }
            else if (IsPlayerInSight())
            {
                enemyState = State.Chase; // 플레이어가 시야에는 있지만 범위 밖이면 추적
            }
            else
            {
                enemyState = State.Patrol; // 플레이어가 시야에 없으면 순찰
            }
        }
        else
        {
            float xDistance = Mathf.Abs(player.position.x - transform.position.x);

            if (xDistance <= currentAttackRange * 0.8f && IsPlayerInSight()) // 약간의 여유값 주기
            {
                enemyState = State.Attack;
            }
            else if (IsPlayerInSight())
            {
                lastKnownPlayerPos = player.position;
                enemyState = State.Chase;
            }
            else
            {
                enemyState = State.Patrol;
            }
        }
    }

    #region 근거리 적 공격

    public void EnemyAttackHit()
    {
        Vector2 attackCenter = (Vector2)transform.position + new Vector2(isFacingRight ? 0.5f : -0.5f, 0f);
        float radius = 0.8f; // 공격 범위

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCenter, radius, LayerMask.GetMask("Player")); // "Player" 레이어가 필요

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Player player))
            {
                player.TakeDamage(currentAttack, currentCritcleRate, currentCritcleDmg, 1);
            }
        }
    }

    #endregion
    #endregion

    #region 기절 효과
    public void ApplyStun(float stunDuration, int num)
    {
        if (enemyState == State.Death) return;       
        StopAllCoroutines(); // 혹시 이전 상태 처리 중이던 코루틴이 있다면 중단
        enemyState = State.Stun;

        anim.SetBool("isMoving", true);
        anim.SetBool("Attack", false);
        if(num == 0)
        {
            currentConfusionEffect = ObjectPool.instance.SetGameObjectConfusion(transform.position + new Vector3(0, 1.8f , 0), stunDuration * currentGuardRecoverycoolTime);
            StartCoroutine(StunRoutine(stunDuration * currentGuardRecoverycoolTime));
        }
        else
        {
            currentConfusionEffect = ObjectPool.instance.SetGameObjectConfusion(transform.position + new Vector3(0, 1.8f, 0), stunDuration);
            StartCoroutine(StunRoutine(stunDuration));           
        }

    }

    private IEnumerator StunRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // 기절 후, 다시 추적 상태로
        if (Player.instance != null)
        {
            lastKnownPlayerPos = Player.instance.transform.position;

            bool playerInSight = IsPlayerInSight();
            float xDistance = Mathf.Abs(player.position.x - transform.position.x);
            bool inRange = xDistance <= currentAttackRange;

            if (attackPattern == "long") // 원거리 적
            {
                if (playerInSight && inRange)
                {
                    enemyState = State.Attack;
                }
                else if (playerInSight)
                {
                    enemyState = State.Chase;
                }
                else
                {
                    enemyState = State.Patrol;
                }
            }
            else // 근거리 적
            {
                if (isNearPlayer)
                {
                    enemyState = State.Attack;
                }
                else if (playerInSight)
                {
                    enemyState = State.Chase;
                }
                else
                {
                    enemyState = State.Patrol;
                }
            }

            gotHit = (enemyState != State.Patrol); // 추적 또는 공격 시에는 gotHit 유지
        }
        else
        {
            enemyState = State.Patrol;
            gotHit = false;
        }
        //스턴이 끝나면 다시 기절 안되게 가드 게이지 최대치로 맞춤
        currentGuardValue = currentMaxGuardValue;
        guardSlider.SetValue(1);
    }
    #endregion

    #region 바위 포물선 함정에 당했을 때 실행되는 함수
    public void RockTrapHit()
    {
        currentHp = 0;
        hpSlider.value = 0;
        enemyState = State.Death;
        switch (id)
        {
            case 0:
                SoundManager.Instance.PlaySFX("Slime_death");
                break;
            case 1:
                break;
            case 2:
                SoundManager.Instance.PlaySFX("rat_death");
                break;
            case 3:
                break;
            case 6:
                SoundManager.Instance.PlaySFX("Goblin_Death");
                break;
        }
        anim.SetTrigger("Death");
        PlayerManager.instance.AddExp(addExp);
        PlayerManager.instance.AddMoney(addMoney);
    }
    #endregion
}
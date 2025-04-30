using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 변수
    public float moveSpeed;
    public float jumpSpeed;
    //0 : 체력 포션 쿨타임, 1 : 스태미나 포션 쿨타임
    public float[] shortKeyCoolTime = new float[2];
    //0 : 대시 쿨타임, 1 : 스태미나 회복 쿨타임, 2 : 체력 회복 쿨타임,3 : 점프 쿨타임, 4 : 공격 쿨타임, 
    public float[] CoolTime = new float[5];

    //체력 포션 재사용시간
    public float lastHpPotionUseTime = -Mathf.Infinity;
    //스태미나 포션 재사용 시간
    public float lastStaminaPotionUseTime = -Mathf.Infinity;
    //플레이어 이동 제한
    public List<Vector2> minLimitList = new List<Vector2>();
    public List<Vector2> maxLimitList = new List<Vector2>();

    public float currentStamina;
    public float currentHp;
    public Rigidbody2D rb;
    public Animator anim;

    public DownAttackTrajectory downAttackTrajectory;
    public int currentMapNum;

    public LayerMask enemyLayer;

    public static Player instance;

    //공격 애니메이션 실행중인지 판단
    private bool isAttacking = false;

    //대시 공격 여부
    private bool isDashAttacking = false;

    private Vector2 originalColliderOffset;

    SpriteRenderer shadowRender;

    private float lastDashTime;
    private float lastStaminaTime;
    private float lastHpTime;
    private float lastJumpTime;
    private float lastAttackTime;
    private float[] staminaCost = new float[2];
    private float guradRecoveryValue = 10;
    private float guradRecoveryCoolTime = 6;
    private float guardRecoverTimer = 0;
    bool canRecoverGuard = false;
    [SerializeField] GuardShrinkSlider playerGuardShrinkSlider;

    [SerializeField] Text hpText;
    [SerializeField] Text staminaText;

    private float lastMoveDirection = 5f; // 1: 오른쪽, -1: 왼쪽

    PlayerAttack playerAttack;
    PlayerInvincibility Invincibility;
    PlayerBlock block;

    public float moveDirection = 0f;

    #region 낙하 변수
    [Header("낙하 변수")]
    [SerializeField] float fallDamageVelocityThreshold = -15f;  // 이 속도보다 빠르게 떨어지면 데미지
    private float fallDamageMultiplier = 5f;           // 데미지 배율
    [SerializeField] bool wasFalling = false;                   // 이전 프레임 낙하 여부
    [SerializeField] float maxFallSpeed = 0f;
    #endregion

    #region 사다리 관련 변수
    [Header("Ladder 변수")]
    [SerializeField] private LayerMask ladderLayer;
    private bool isOnLadder = false;
    private bool isNearLadder = false;
    private float ladderCheckRadius = 0.2f;

    private float ladderTopY;
    private float ladderBottomY;
    private Collider2D currentLadder;
    [SerializeField] private float ladderExitBuffer = 0.2f; // 사다리 탈출 감지용 오차
    #endregion

    #region 튕김 변수
    [Header("함정 튕김 변수")]
    public bool isKnockback = false;
    #endregion

    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHp = PlayerManager.instance.player.hp;
        currentStamina = PlayerManager.instance.player.stamina;
        instance = this;
        enemyLayer = LayerMask.GetMask("Enemy");
        PlayerManager.instance.isGround = true;
        playerAttack = GetComponent<PlayerAttack>();
        downAttackTrajectory = GetComponent<DownAttackTrajectory>();
        block = GetComponent<PlayerBlock>();
        Invincibility = GetComponent<PlayerInvincibility>();
        currentMapNum = PlayerManager.instance.player.currentMapNum;
        shadowRender = transform.GetChild(1).GetComponent<SpriteRenderer>();
        maxFallSpeed = 0;
    }

    private void Start()
    {
        PlayerManager.instance.IsDead = false;
        PlayerManager.instance.isPause = false;
        PlayerManager.instance.isBuy = false;
        PlayerManager.instance.isState = false;
        PlayerManager.instance.isStun = false;
        PlayerManager.instance.isGuarding = false;
        HpOrStaminaCoolTime(0);
        HpOrStaminaCoolTime(1);
        staminaCost[0] = 5;
        staminaCost[1] = 3;
        canRecoverGuard = false;
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false && PlayerManager.instance.isBuy == false && PlayerManager.instance.isPause == false && PlayerManager.instance.isState == false && PlayerManager.instance.isInventroy == false && PlayerManager.instance.isEquipment == false && PlayerManager.instance.isSkillPage == false && PlayerManager.instance.isDownAttacking == false && PlayerManager.instance.isStun == false)
        {
            StaminaCostRestoration();
            AutoHpRestoration();
            Jump();
            Attack();
            HpPotionEat();
            StaminaPotionEat();
            Move();
            Dash();
            UpdateGuardSlider();
            CheckLadder();
            HandleLadderMovement();
            //플레이어 공중에 있고 아래로 떨어지고 있을 때 실행
            if (!PlayerManager.instance.isGround && rb.velocity.y < 0)
            {
                wasFalling = true;
                //가장 빠른 낙하 속도를 지속적으로 기록
                // 점프 후 위로 올라가면 velocity.y 양수가 됨
                // 따라서 낙하하면서 velocity.y 는 음수로 작아지기 때문에 기존 값들 중 제일 작은 값을 찾기 위해서 Mathf.Min 을 사용
                maxFallSpeed = Mathf.Min(maxFallSpeed, rb.velocity.y); // 음수이므로 Min 사용
            }
        }

        if(PlayerManager.instance.IsDead == false)
        {
            SkyManager.instance.HandleShadowVisibility(shadowRender.transform.gameObject, shadowRender, 5);
        }
        GameCanvas.instance.HpPotionUiSetting(lastHpPotionUseTime, shortKeyCoolTime[0]);
        GameCanvas.instance.StaminaPotionUiSetting(lastStaminaPotionUseTime, shortKeyCoolTime[1]);
        StaminaTextChange(currentStamina, PlayerManager.instance.player.stamina);
        HpTextChange(currentHp, PlayerManager.instance.player.hp);
    }

    public void HpOrStaminaCoolTime(int num)
    {
        switch(num)
        {
            case 0:
                int hplevel = (int)PlayerManager.instance.player.skill.GetSkillLevelByName("자동체력회복증가");
                if(hplevel != 0)
                {
                    CoolTime[2] = PlayerManager.instance.player.skill.GetSkillCoolTime("자동체력회복증가");
                }
                else if(hplevel == 0)
                {
                    CoolTime[2] = 60;
                }
                break;
            case 1:
                int stlevel = (int)PlayerManager.instance.player.skill.GetSkillLevelByName("자동스태미나회복증가");
                if (stlevel != 0)
                {
                    CoolTime[1] = PlayerManager.instance.player.skill.GetSkillCoolTime("자동스태미나회복증가");
                }
                else if(stlevel == 0)
                {
                    CoolTime[1] = 40;
                }
                break;
        }
    }

    #region Hp,Stamian Text 변경 함수
    public void HpTextChange(float current, float max)
    {
        hpText.text = current + " / " + max;
    }

    public void StaminaTextChange(float current, float max)
    {
        staminaText.text = current + " / " + max;
    }
    #endregion

    #region 데미지 입었을 때 실행되는 함수
    public void TakeDamage(float damage, float critcleRate,float critcleDmg,int num)
    {
        //죽거나 ,무적상태가 아닐때 데미지가 들어가야함
        if (PlayerManager.instance.IsDead || PlayerManager.instance.isInvincibility) return;

        if(block.isBlocking == true)
        {
            block.OnBlocked();
            SoundManager.Instance.PlaySFX("Block");
        }
        else
        {
            float defense = PlayerManager.instance.player.defense;

            int rand = UnityEngine.Random.Range(1, 101); // 1 ~ 100 포함

            bool isCritical = false;

            PlayerManager.instance.player.currentGuardValue -= damage;
            PlayerManager.instance.player.currentGuardValue = Mathf.Clamp(PlayerManager.instance.player.currentGuardValue, 0, PlayerManager.instance.player.maxGuardValue);

            float guardRatio = PlayerManager.instance.player.currentGuardValue / PlayerManager.instance.player.maxGuardValue;
            playerGuardShrinkSlider.SetValue(guardRatio);

            if (PlayerManager.instance.player.currentGuardValue <= 0f)
            {
                isCritical = true;
                if(PlayerManager.instance.isStun == false)
                {
                    ApplyStun(guradRecoveryCoolTime);
                }             
            }
            else
            {
                isCritical = rand <= critcleRate;
            }

            // 데미지 감소율을 방어력으로부터 계산 (최대 85% 제한)
            // 방어력이 높을 수록 데미지를 퍼센트로 감소 시키기 위해 사용
            /*
             * 방어력 상승으로 게임 밸런스를 막기 위해서 최대 85퍼센트 데미지만 들어가게 설정
             */
            float maxDamageReduction = 0.85f; // 최대 80% 데미지 감소

            float damageReductionPercent = Mathf.Min(defense / 100f, maxDamageReduction);

            float finalDamage = damage * (1 - damageReductionPercent);

            if (isCritical)
            {
                finalDamage *= critcleDmg;
            }

            currentHp -= finalDamage;
            currentHp = Mathf.Max(0, currentHp); // 체력 0 이하로 떨어지지 않도록 처리

            // UI 체력바 갱신
            GameCanvas.instance.SliderChange(0, 1, finalDamage); // (체력, 감소, 양)

            switch (num)
            {
                //함정
                case 0:
                    if (isCritical) //크리티컬 히트
                    {
                        ObjectPool.instance.SetDamageText(transform.position, 1, finalDamage);
                    }
                    else
                    {
                        ObjectPool.instance.SetDamageText(transform.position, 0, finalDamage);
                    }
                    break;
                //몬스터
                case 1:
                    if (isCritical) //크리티컬 히트
                    {
                        ObjectPool.instance.SetDamageText(transform.position, 1, finalDamage);
                    }
                    else
                    {
                        ObjectPool.instance.SetDamageText(transform.position, 0, finalDamage);
                    }
                    break;
            }

            if (currentHp <= 0)
            {
                anim.SetBool("Death", true); //  Animator에 Death 상태 전이 발생        
                PlayerManager.instance.IsDead = true;
            }
            else
            {
                anim.SetBool("Hurt", true); //  Animator에 Hurt 상태 전이 발생     

                //무적효과
                Invincibility.TriggerInvincibility();
            }

            guardRecoverTimer = 0;
            canRecoverGuard = false;
        }      
    }

    public void HurtEnd()
    {
        anim.SetBool("Hurt", false);
    }

    #endregion


    #region 체력,스태미나 자동 회복
    void StaminaCostRestoration()
    {
        if(currentStamina <= PlayerManager.instance.player.stamina)
        {
            if(Time.time >= lastStaminaTime + CoolTime[1])
            {
                currentStamina += PlayerManager.instance.player.staminaAutoRestoration;
                GameCanvas.instance.SliderChange(1, 0, PlayerManager.instance.player.staminaAutoRestoration);
                if (currentStamina >= PlayerManager.instance.player.stamina)
                {
                    currentStamina = PlayerManager.instance.player.stamina;
                }
                lastStaminaTime = Time.time;              
            }
        }
    }

    void AutoHpRestoration()
    {
        if(currentHp <= PlayerManager.instance.player.hp)
        {
            if (Time.time >= lastHpTime + CoolTime[2])
            {
                currentHp += PlayerManager.instance.player.hpAutoRestoration;
                GameCanvas.instance.SliderChange(0, 0, PlayerManager.instance.player.hpAutoRestoration);
                if (currentHp >= PlayerManager.instance.player.hp)
                {
                    currentHp = PlayerManager.instance.player.hp;
                }
                lastHpTime = Time.time;
            }
        }
    }
    #endregion


    #region 이동 함수
    void Move()
    {
        if (isKnockback) return; // 튕김 상태면 이동 금지

        moveDirection = 0;

        if (PlayerManager.GetCustomKey(CustomKeyCode.Left))
        {
            if(rb.position.x > minLimitList[currentMapNum].x)
            {
                moveDirection = -1f; // 왼쪽으로 이동                   
            }
            else
            {
                moveDirection = 0;
            }
        }
        else if (PlayerManager.GetCustomKey(CustomKeyCode.Right))
        {
            // 오른쪽으로 이동 시도
            if (rb.position.x < maxLimitList[currentMapNum].x) // 현재 위치가 최대 제한보다 왼쪽일 때만 이동 허용
            {
                moveDirection = 1f;               
            }
            else
            {
                moveDirection = 0f; // 제한 위치에 도달하면 이동 막음
            }
        }
        // 움직이면 방향 저장
        if (moveDirection != 0f)
        {
            lastMoveDirection = moveDirection;
        }

        // 매 프레임마다 방향 적용
        Vector3 newScale = transform.localScale;
        newScale.x = lastMoveDirection > 0 ? Mathf.Abs(newScale.x) : -Mathf.Abs(newScale.x);
        transform.localScale = newScale;
        playerAttack.SetFacingDir(newScale.x);

        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);       
        if (moveDirection != 0f)
        {
            anim.SetBool("Move", true);
        }
        else
        {
            anim.SetBool("Move", false);
        }
    }
    #endregion


    #region 공격 함수
    void Attack()
    {
        if (isDashAttacking) return; // ← 대시 공격 중이면 무시

        if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
        {
            if (!isAttacking && Time.time >= lastAttackTime + CoolTime[4] && currentStamina >= staminaCost[0])
            {
                isAttacking = true; // 공격 중 플래그 ON
                currentStamina -= staminaCost[0];
                lastAttackTime = Time.time;
                GameCanvas.instance.SliderChange(1, 1, staminaCost[0]);
                anim.SetBool("Attack", true);              
            }
        }
    }

    public void OnAttackEnd()
    {
        anim.SetBool("Attack", false);
        isAttacking = false;
    }

    #endregion


    #region 대시 함수
    void Dash()
    {
        if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Evasion))
        {
            if (Time.time >= lastDashTime + CoolTime[0] && currentStamina >= staminaCost[1])
            {
                PlayerManager.instance.isInvisble = true;
                currentStamina -= staminaCost[1];
                lastDashTime = Time.time;
                GameCanvas.instance.SliderChange(1, 1, staminaCost[1]);

                // 방향 판단 (왼쪽: -1, 오른쪽: 1)
                float dashDirection = lastMoveDirection;

                //대시 이동 처리
                Vector2 targetPosition = rb.position + new Vector2(dashDirection * moveSpeed * 1.2f, 0);

                // 대시 위치가 맵 제한을 벗어나지 않도록 제한
                targetPosition.x = Mathf.Clamp(targetPosition.x, minLimitList[currentMapNum].x, maxLimitList[currentMapNum].x);

                rb.MovePosition(targetPosition);
                
                anim.SetBool("Dash", true);

                //대시 중 공격 입력 체크
                StartCoroutine(HandleDashAttack());      
            }
        }
    }

    public void DashEnd()
    {
        anim.SetBool("Dash", false);
        PlayerManager.instance.isInvisble = false;
    }

    #endregion


    #region 대시 공격 어택 함수
    IEnumerator HandleDashAttack()
    {
        float timer = 0f;
        float maxDashAttackWindow = 0.3f; // 대시 중 공격 입력 허용 시간

        while (timer < maxDashAttackWindow)
        {
            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
            {
                anim.SetBool("Dash", false);
                anim.SetBool("Attack", false);
                anim.SetTrigger("DashAttack"); //  Animator 파라미터 설정 필요
                isDashAttacking = true;
                yield break; // 더 이상 감지하지 않음
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void DashAttackEnd()
    {
        anim.SetTrigger("Idle");
        isDashAttacking = false;
    }
    #endregion

    #region 점프
    void Jump()
    {
        if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Jump))
        {
            if(Time.time >= lastJumpTime + CoolTime[3] && PlayerManager.instance.isJump == false && PlayerManager.instance.isGround == true)
            {
                PlayerManager.instance.isJump = true;
                PlayerManager.instance.isGround = false;
                lastJumpTime = Time.time;

                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                anim.SetBool("Jump", true);  // 점프 애니메이션 활성화
            }
        }

        if (rb.velocity.y < 0 && !PlayerManager.instance.isGround && !PlayerManager.instance.isPlayerOnLadder)
        {
            anim.SetBool("IsFalling", true);  // 하강 애니메이션 활성화
        }

        if (PlayerManager.instance.isGround)
        {
            anim.SetBool("Jump", false);  // 점프 애니메이션 비활성화
            anim.SetBool("IsFalling", false);  // 하강 애니메이션 비활성화
            PlayerManager.instance.isJump = false;
        }
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            //직전에 낙하 중이었으면 true
            if (wasFalling)
            {
                //기록된 낙하 최고 속도가 설정한 fallDamageVelocityThreshold 값보다 낮은 음수 값이면 데미지를 입힘
                if (maxFallSpeed < fallDamageVelocityThreshold)
                {
                    //데미지 값 적용
                    //ex ) maxFallSpeed  = -15, fallDamageVelocityThreshold = -10 이면 damage = |(-15 + 10)| * fallDamageMultiplier => 5 * fallDamageMultiplier
                    float damage = Mathf.Abs(maxFallSpeed + Mathf.Abs(fallDamageVelocityThreshold)) * FallDamageMutiplyer();
                    TakeFallDamage(damage);
                }

                wasFalling = false;
                maxFallSpeed = 0f;
            }

            PlayerManager.instance.isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            PlayerManager.instance.isGround = false;  // 땅에서 떨어졌다고 인식
        }
    }

    #region 체력,스태미나 포션 먹는 함수
    void HpPotionEat()
    {
        if (PlayerManager.instance.hpPotionCount[PlayerManager.instance.player.hpPotionSelectnum] > 0  && currentHp < PlayerManager.instance.player.hp)
        {
            if (Time.time >= lastHpPotionUseTime + shortKeyCoolTime[0])
            {
                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.ShortcutKey1))
                {
                    // Consume HP potion
                    switch(PlayerManager.instance.player.hpPotionSelectnum)
                    {
                        case 0:
                            PlayerManager.instance.player.inventory.RemoveItemByName("체력포션", 1);
                            currentHp = currentHp + PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("체력포션");
                            GameCanvas.instance.SliderChange(0, 0, PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("체력포션"));
                            break;
                        case 1:
                            PlayerManager.instance.player.inventory.RemoveItemByName("중간체력포션", 1);
                            currentHp = currentHp + PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("중간체력포션");
                            GameCanvas.instance.SliderChange(0, 0, PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("중간체력포션"));
                            break;
                        case 2:
                            PlayerManager.instance.player.inventory.RemoveItemByName("상급체력포션", 1);
                            currentHp = currentHp + PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("상급체력포션");
                            GameCanvas.instance.SliderChange(0, 0, PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("상급체력포션"));
                            break;
                    }
                    if(currentHp > PlayerManager.instance.player.hp)
                    {
                        currentHp = PlayerManager.instance.player.hp;
                    }
                    lastHpPotionUseTime = Time.time;
                    GameCanvas.instance.PotionSetting();               
                }
            }
        }
    }

    public void StaminaPotionEat()
    {
        if (PlayerManager.instance.staminaPotionCount[PlayerManager.instance.player.staminaPotionSelectnum] > 0 && currentStamina< PlayerManager.instance.player.stamina)
        {
            if (Time.time >= lastStaminaPotionUseTime + shortKeyCoolTime[1])
            {
                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.ShortcutKey2))
                {
                    // Consume Stamina potion
                    switch(PlayerManager.instance.player.staminaPotionSelectnum)
                    {
                        case 0:
                            PlayerManager.instance.player.inventory.RemoveItemByName("스태미나포션", 1);
                            currentStamina = currentStamina + PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("스태미나포션");
                            GameCanvas.instance.SliderChange(1, 0, PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("스태미나포션"));
                            break;
                        case 1:
                            PlayerManager.instance.player.inventory.RemoveItemByName("중간스태미나포션", 1);
                            currentStamina = currentStamina + PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("중간스태미나포션");
                            GameCanvas.instance.SliderChange(1, 0, PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("중간스태미나포션"));
                            break;
                        case 2:
                            PlayerManager.instance.player.inventory.RemoveItemByName("상급스태미나포션", 1);
                            currentStamina = currentStamina + PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("상급스태미나포션");
                            GameCanvas.instance.SliderChange(1, 0, PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("상급스태미나포션"));
                            break;
                    }
                    if(currentStamina > PlayerManager.instance.player.stamina)
                    {
                        currentStamina = PlayerManager.instance.player.stamina;
                    }
                    lastStaminaPotionUseTime = Time.time;
                    GameCanvas.instance.PotionSetting();
                }
            }
        }
    }
    #endregion

    #region 가드 
    void UpdateGuardSlider()
    {
        if(PlayerManager.instance.player.currentGuardValue < PlayerManager.instance.player.maxGuardValue)
        {
            guardRecoverTimer += Time.deltaTime;

            if (!canRecoverGuard && guardRecoverTimer >= guradRecoveryCoolTime)
            {
                canRecoverGuard = true;
            }

            if (canRecoverGuard)
            {
                PlayerManager.instance.player.currentGuardValue += guradRecoveryValue * Time.deltaTime;
                PlayerManager.instance.player.currentGuardValue = Mathf.Clamp(PlayerManager.instance.player.currentGuardValue, 0f, PlayerManager.instance.player.maxGuardValue);

                float guardRatio = PlayerManager.instance.player.currentGuardValue / PlayerManager.instance.player.maxGuardValue;
                playerGuardShrinkSlider.SetValue(guardRatio);
            }
        }
    }
    #endregion

    #region 기절 효과
    void ApplyStun(float duration)
    {
        ObjectPool.instance.SetConfusion(transform.position, guradRecoveryCoolTime);
        // 플레이어가 죽어있으면 스턴할 필요 없음
        if (PlayerManager.instance.IsDead)
            return;
        PlayerManager.instance.isStun = true;
        anim.SetBool("Dash", false);
        anim.SetBool("Attack", false);
        isAttacking = false;
        anim.SetBool("Move", false);
        StartCoroutine(StunRoutine(guradRecoveryCoolTime));
    }

    private IEnumerator StunRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        PlayerManager.instance.isStun = false;
        PlayerManager.instance.player.currentGuardValue = PlayerManager.instance.player.maxGuardValue;
        playerGuardShrinkSlider.SetValue(1);
    }
    #endregion

    #region 낙하
    private void TakeFallDamage(float damage)
    {
        TakeDamage(damage, 0, 1.1f, 0); // critRate=0, critDmg=1, num=0(함정으로 처리)
    }

    float FallDamageMutiplyer()
    {
        float currentFallDamage = 1;
        switch (GameManager.data.diffucity)
        {
            case Diffucity.Easy:
                currentFallDamage = fallDamageMultiplier * 1;
                break;
            case Diffucity.Normal:
                currentFallDamage = fallDamageMultiplier * 1.2f;
                break;
            case Diffucity.Hard:
                currentFallDamage = fallDamageMultiplier * 1.5f;
                break;
        }
        return currentFallDamage;
    }
    #endregion

    #region 사다리 처리
    //사다리 근처에 있는지 감지 + 사다리 탈출 판단
    void CheckLadder()
    {
        //플레이어 중심보다 아래쪽 (-0.1f) 기준으로 가로 0.6, 세로 1.2 크기의 박스 영역안에 사다리가 존재하는 감지
        Collider2D hit = Physics2D.OverlapBox(transform.position + new Vector3(0, -0.1f), new Vector2(0.6f, 1.2f), 0f, ladderLayer);
        //사다리가 감지 되면 true, 안 되면 false
        isNearLadder = hit != null;

        if (isOnLadder && (!isNearLadder || transform.position.y > ladderTopY + ladderExitBuffer || transform.position.y < ladderBottomY - ladderExitBuffer))
        {
            ExitLadder();
        }
    }
    //사다리 타기 상태에서 입력 처리 및 이동
    void HandleLadderMovement()
    {
        //사다리와 닿기만 해도 자동 진입
        if (isNearLadder && !isOnLadder)
        {
            EnterLadder();
        }

        if (isOnLadder)
        {
            float vertical = 0f;

            if (PlayerManager.GetCustomKey(CustomKeyCode.Up))
                vertical = 1f;
            else if (PlayerManager.GetCustomKey(CustomKeyCode.Down))
                vertical = -1f;
            //아무 입력 없을 경우 낙하 방지
            float yVelocity = vertical != 0 ? vertical * moveSpeed : 0f;

            //좌우 이동은 허용하되 위 아래 입력 없으면 y속도는 고정
            rb.velocity = new Vector2(moveDirection * moveSpeed, vertical * moveSpeed);

            // 애니메이션 처리
            anim.SetBool("Ladder", true);
            anim.SetBool("Move", false);
            anim.SetBool("Jump", false);
            anim.SetBool("IsFalling", false);
        }
    }
    //사다리 상태로 진입(중력 제거, 위치 정보 기억)
    void EnterLadder()
    {
        //사다리 타는 상태로 전환
        isOnLadder = true;

        //중력 제거
        rb.gravityScale = 0f;

        //현재 속도 초기화 (불필요한 이동 제거)
        rb.velocity = Vector2.zero;

        PlayerManager.instance.isPlayerOnLadder = true;

        //땅 위에 있지 않음 (착지 관련 로직 분리)
        PlayerManager.instance.isGround = false;

        currentLadder = Physics2D.OverlapBox(transform.position + new Vector3(0, -0.1f), new Vector2(0.6f, 1.2f), 0f, ladderLayer);
        if (currentLadder != null)
        {
            Bounds bounds = currentLadder.bounds;
            //사다리 가장 윗 좌표 저장
            ladderTopY = bounds.max.y;
            //사다리 가장 아래 좌표 저장
            ladderBottomY = bounds.min.y;
        }

        anim.SetBool("Ladder", true);
    }
    //사다리 상태 해제
    void ExitLadder()
    {
        isOnLadder = false;
        rb.gravityScale = 1f;

        PlayerManager.instance.isPlayerOnLadder = false;
        anim.SetBool("Ladder", false);

        // 낙하 시 하강 애니메이션 적용
        if (rb.velocity.y < 0)
        {
            anim.SetBool("IsFalling", true);
        }
    }
    #endregion

    #region 바위 함정에 부딪 혔을 경우 튕겨 나가게 하고 튕김을 해제하는 기능
    public IEnumerator EndKnockback(float delay)
    {
        yield return new WaitForSeconds(delay);
        isKnockback = false;
    }
    #endregion
}

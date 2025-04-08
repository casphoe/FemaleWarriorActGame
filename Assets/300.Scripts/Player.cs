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

    public DownAttackTrajectory downAttackTrajectory;
    public int currentMapNum;

    public LayerMask enemyLayer;

    public static Player instance;

    //공격 애니메이션 실행중인지 판단
    private bool isAttacking = false;

    //대시 공격 여부
    private bool isDashAttacking = false;

    private Animator anim;

    private Vector2 originalColliderOffset;

    SpriteRenderer shadowRender;

    private float lastDashTime;
    private float lastStaminaTime;
    private float lastHpTime;
    private float lastJumpTime;
    private float lastAttackTime;
    private float[] staminaCost = new float[2];

    [SerializeField] Text hpText;
    [SerializeField] Text staminaText;

    private float lastMoveDirection = 5f; // 1: 오른쪽, -1: 왼쪽

    PlayerAttack playerAttack;
    PlayerInvincibility Invincibility;

    public float moveDirection = 0f;

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
        Invincibility = GetComponent<PlayerInvincibility>();
        currentMapNum = PlayerManager.instance.player.currentMapNum;
        shadowRender = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        PlayerManager.instance.IsDead = false;
        PlayerManager.instance.isPause = false;
        PlayerManager.instance.isBuy = false;
        PlayerManager.instance.isState = false;
        HpOrStaminaCoolTime(0);
        HpOrStaminaCoolTime(1);
        staminaCost[0] = 5;
        staminaCost[1] = 3;
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false && PlayerManager.instance.isBuy == false && PlayerManager.instance.isPause == false && PlayerManager.instance.isState == false && PlayerManager.instance.isInventroy == false && PlayerManager.instance.isEquipment == false && PlayerManager.instance.isSkillPage == false && PlayerManager.instance.isDownAttacking == false)
        {
            StaminaCostRestoration();
            AutoHpRestoration();
            Jump();
            Attack();
            HpPotionEat();
            StaminaPotionEat();
            GameCanvas.instance.HpPotionUiSetting(lastHpPotionUseTime, shortKeyCoolTime[0]);
            GameCanvas.instance.StaminaPotionUiSetting(lastStaminaPotionUseTime, shortKeyCoolTime[1]);
            Move();
            Dash();
        }

        if(PlayerManager.instance.IsDead == false)
        {
            SkyManager.instance.HandleShadowVisibility(shadowRender.transform.gameObject, shadowRender, 5);
            HpTextChange(currentHp, PlayerManager.instance.player.hp);
            StaminaTextChange(currentStamina, PlayerManager.instance.player.stamina);
        }
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


        float defense = PlayerManager.instance.player.defense;

        int rand = UnityEngine.Random.Range(1, 101); // 1 ~ 100 포함

        bool isCritical = rand <= critcleRate;

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

        switch(num)
        {
            //함정
            case 0:
                ObjectPool.instance.SetDamageText(transform.position, 0, finalDamage);
                break;
                //몬스터
            case 1:
                if(isCritical) //크리티컬 히트
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
        moveDirection = 0;

        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.Left]))
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
        else if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.Right]))
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

        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Attack]))
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
        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Evasion]))
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
            if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Attack]))
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

    void Jump()
    {
        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Jump]))
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

        if(rb.velocity.y < 0 && !PlayerManager.instance.isGround)
        {
            anim.SetBool("IsFalling", true);  // 하강 애니메이션 활성화
        }    

        if(PlayerManager.instance.isGround)
        {
            anim.SetBool("Jump", false);  // 점프 애니메이션 비활성화
            anim.SetBool("IsFalling", false);  // 하강 애니메이션 비활성화
            PlayerManager.instance.isJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
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
                if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey1]))
                {
                    // Consume HP potion
                    switch(PlayerManager.instance.player.hpPotionSelectnum)
                    {
                        case 0:
                            PlayerManager.instance.player.inventory.RemoveItemByName("체력포션", 1);
                            currentHp = currentHp + PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("체력포션");
                            break;
                        case 1:
                            PlayerManager.instance.player.inventory.RemoveItemByName("중간체력포션", 1);
                            currentHp = currentHp + PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("중간체력포션");
                            break;
                        case 2:
                            PlayerManager.instance.player.inventory.RemoveItemByName("상급체력포션", 1);
                            currentHp = currentHp + PlayerManager.instance.player.hp * PlayerManager.instance.player.inventory.EatHpPotion("상급체력포션");
                            break;
                    }
                    if(currentHp > PlayerManager.instance.player.hp)
                    {
                        currentHp = PlayerManager.instance.player.hp;
                    }
                    lastHpPotionUseTime = Time.time;
                    GameCanvas.instance.PotionSetting();
                    GameCanvas.instance.SliderChange(0, 0, 30);
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
                if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey2]))
                {
                    // Consume Stamina potion
                    switch(PlayerManager.instance.player.staminaPotionSelectnum)
                    {
                        case 0:
                            PlayerManager.instance.player.inventory.RemoveItemByName("스태미나포션", 1);
                            currentStamina = currentStamina + PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("스태미나포션");
                            break;
                        case 1:
                            PlayerManager.instance.player.inventory.RemoveItemByName("중간스태미나포션", 1);
                            currentStamina = currentStamina + PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("중간스태미나포션");
                            break;
                        case 2:
                            PlayerManager.instance.player.inventory.RemoveItemByName("상급스태미나포션", 1);
                            currentStamina = currentStamina + PlayerManager.instance.player.stamina * PlayerManager.instance.player.inventory.EatStaminaPotion("상급스태미나포션");
                            break;
                    }
                    if(currentStamina > PlayerManager.instance.player.stamina)
                    {
                        currentStamina = PlayerManager.instance.player.stamina;
                    }
                    lastStaminaPotionUseTime = Time.time;
                    GameCanvas.instance.PotionSetting();
                    GameCanvas.instance.SliderChange(1, 0, 15);
                }
            }
        }
    }
    #endregion
}

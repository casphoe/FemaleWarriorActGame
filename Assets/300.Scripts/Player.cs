using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
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

    private Animator anim;
    private SpriteRenderer render;
    private CapsuleCollider2D capsuleCollider;

    private Vector2 originalColliderOffset;

    SpriteRenderer shadowRender;

    private float lastDashTime;
    private float lastStaminaTime;
    private float lastHpTime;
    private float lastJumpTime;
    private float lastAttackTime;
    private float[] staminaCost = new float[2];
    PlayerInvincibility Invincibility;

    public float currentStamina;
    public float currentHp;
    public Rigidbody2D rb;

    public DownAttackTrajectory downAttackTrajectory;
    public int currentMapNum;

    public LayerMask enemyLayer;

    public static Player instance;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        currentHp = PlayerManager.instance.player.hp;
        currentStamina = PlayerManager.instance.player.stamina;
        instance = this;
        enemyLayer = LayerMask.GetMask("Enemy");
        PlayerManager.instance.isGround = true;
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
        staminaCost[1] = 10;
        originalColliderOffset = capsuleCollider.offset;
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false && PlayerManager.instance.isBuy == false && PlayerManager.instance.isPause == false && PlayerManager.instance.isState == false && PlayerManager.instance.isInventroy == false && PlayerManager.instance.isEquipment == false && PlayerManager.instance.isSkillPage == false && PlayerManager.instance.isDownAttacking == false)
        {           
            Move();
            Dash();
            StaminaCostRestoration();
            AutoHpRestoration();
            Jump();
            Attack();
            HpPotionEat();
            StaminaPotionEat();
            GameCanvas.instance.HpPotionUiSetting(lastHpPotionUseTime, shortKeyCoolTime[0]);
            GameCanvas.instance.StaminaPotionUiSetting(lastStaminaPotionUseTime, shortKeyCoolTime[1]);
        }

        if(PlayerManager.instance.IsDead == false)
        {
            SkyManager.instance.HandleShadowVisibility(shadowRender.transform.gameObject, shadowRender, 5);
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

    public void TakeDamage(float damage)
    {
        //죽거나 ,무적상태가 아닐때 데미지가 들어가야함
        if (PlayerManager.instance.IsDead || PlayerManager.instance.isInvincibility) return;


        currentHp -= damage;
        currentHp = Mathf.Max(0, currentHp); // 체력 0 이하로 떨어지지 않도록

        // UI 체력바 갱신
        GameCanvas.instance.SliderChange(0, 1, damage); // (체력, 감소, 양)

        if (currentHp <= 0)
        {
            anim.SetBool("Death", true); //  Animator에 Death 상태 전이 발생        
        }
        else
        {
            anim.SetBool("Hurt", true); //  Animator에 Hurt 상태 전이 발생     

            float hurtAnimLength = anim.runtimeAnimatorController.animationClips
        .FirstOrDefault(clip => clip.name == "Hurt")?.length ?? 0.3f;

            StartCoroutine(RestAnimation(2, hurtAnimLength));
            //무적효과
            Invincibility.TriggerInvincibility();
        }
    }

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

    void Move()
    {
        float moveDirection = 0f;

        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.Left]))
        {
            if(rb.position.x > minLimitList[currentMapNum].x)
            {
                moveDirection = -1f; // 왼쪽으로 이동
                render.flipX = true;
                AdjustColliderOffset(true);             
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
                render.flipX = false;
                AdjustColliderOffset(false);           
            }
            else
            {
                moveDirection = 0f; // 제한 위치에 도달하면 이동 막음
            }
        }

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

    void Attack()
    {
        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Attack]))
        {         
            if(Time.time >= lastAttackTime + CoolTime[4] && currentStamina >= staminaCost[0])
            {
                currentStamina -= staminaCost[0];
                lastAttackTime = Time.time;
                GameCanvas.instance.SliderChange(1, 1, staminaCost[0]);
                anim.SetBool("Attack", true);

                float attackLength = anim.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == "Attack")?.length ?? 0.3f;
                StartCoroutine(RestAnimation(1, attackLength));
            }
        }
    }

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

                //대시 방향 계산
                float dashDirection = render.flipX ? -1f : 1f;

                //대시 이동 처리
                Vector2 targetPosition = rb.position + new Vector2(dashDirection * moveSpeed * 1.2f, 0);

                // 대시 위치가 맵 제한을 벗어나지 않도록 제한
                targetPosition.x = Mathf.Clamp(targetPosition.x, minLimitList[currentMapNum].x, maxLimitList[currentMapNum].x);

                rb.MovePosition(targetPosition);
                
                anim.SetBool("Dash", true);

                //대시 중 공격 입력 체크
                StartCoroutine(HandleDashAttack());

                
                float dashAnimationLength = anim.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == "Dash")?.length ?? 0.3f;
                StartCoroutine(RestAnimation(0, dashAnimationLength));
            }
        }
    }

    IEnumerator RestAnimation(int num,float time)
    {
        yield return new WaitForSeconds(time);
        switch(num)
        {
            case 0:
                anim.SetBool("Dash", false);
                PlayerManager.instance.isInvisble = false;
                break;
            case 1:
                anim.SetBool("Attack", false);
                break;
            case 2:
                anim.SetBool("Hurt", false);
                break;
        }
    }

    IEnumerator HandleDashAttack()
    {
        float timer = 0f;
        float maxDashAttackWindow = 0.3f; // 대시 중 공격 입력 허용 시간

        while (timer < maxDashAttackWindow)
        {
            if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Attack]))
            {
                anim.SetBool("Dash", false);
                anim.SetTrigger("DashAttack"); //  Animator 파라미터 설정 필요

                float animLength = anim.runtimeAnimatorController.animationClips
                .FirstOrDefault(x => x.name == "Dash-Attack")?.length ?? 0.5f;

                yield return new WaitForSeconds(animLength);

                anim.SetTrigger("Idle");

                yield break; // 더 이상 감지하지 않음
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

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

    void AdjustColliderOffset(bool flip)
    {
        if(flip)
        {
            //좌측 반전시 collider 오프셋을 오른쪽으로 이동
            capsuleCollider.offset = new Vector2(-originalColliderOffset.x, capsuleCollider.offset.y);
        }
        else
        {
            // 반전하지 않으면 원래 오프셋을 유지
            capsuleCollider.offset = originalColliderOffset;
        }
    }
}

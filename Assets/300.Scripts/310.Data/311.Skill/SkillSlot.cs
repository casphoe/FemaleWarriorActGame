using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerClickHandler
{
    public string skillName = string.Empty;
    int slotIndex = 0;

    Image img;

    public float skillCoolTime =  -Mathf.Infinity;

    bool isCharging = false;

    private void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        slotIndex = transform.GetSiblingIndex();
        SkillSlotNum(slotIndex);
    }


    void SkillSlotNum(int index)
    {
        switch(index)
        {
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }

    void Update()
    {
        SkillSlotInput(slotIndex);
    }

    void SkillSlotInput(int index)
    {
        if (skillName != string.Empty)
        {
            if(skillName == "다운어택")
            {
                SkillData downAttack = PlayerManager.instance.player.skill.accquisitionSkillDataList.Find(skill => skill.nameKor == skillName);

                bool canUseSkill = Time.time >= skillCoolTime + downAttack.coolTime &&
                       Player.instance.currentStamina >= downAttack.StaminaConsumption &&
                       PlayerManager.instance.isGround;

                switch (index)
                {
                    case 2:
                        var key = GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3];

                        // DOWN - 스킬 시작 조건 체크
                        if (Input.GetKeyDown(key) && !isCharging)
                        {
                            if (Time.time >= skillCoolTime + downAttack.coolTime &&
                                Player.instance.currentStamina >= downAttack.StaminaConsumption &&
                                PlayerManager.instance.isGround)
                            {
                                isCharging = true;

                                Player.instance.currentStamina -= downAttack.StaminaConsumption;
                                GameCanvas.instance.SliderChange(1, 1, downAttack.StaminaConsumption);
                                Player.instance.downAttackTrajectory.DownAttackStat(downAttack.attackMovePoint, downAttack.attackRange);
                                Player.instance.downAttackTrajectory.StartTrajectory();

                                PlayerManager.instance.isDownAttacking = true;
                            }
                        }

                        // HOLD - 마우스 위치로 라인 이동
                        if (isCharging && Input.GetKey(key))
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        // UP - 스킬 발동
                        if (isCharging && Input.GetKeyUp(key))
                        {
                            isCharging = false;
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time;

                            Player.instance.downAttackTrajectory.ExecuteDownAttack();
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));

                            PlayerManager.instance.isDownAttacking = false;
                        }
                        break;
                    case 3:
                        var key2 = GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4];

                        // DOWN - 스킬 시작 조건 체크
                        if (Input.GetKeyDown(key2) && !isCharging)
                        {
                            if (Time.time >= skillCoolTime + downAttack.coolTime &&
                                Player.instance.currentStamina >= downAttack.StaminaConsumption &&
                                PlayerManager.instance.isGround)
                            {
                                isCharging = true;

                                Player.instance.currentStamina -= downAttack.StaminaConsumption;
                                GameCanvas.instance.SliderChange(1, 1, downAttack.StaminaConsumption);
                                Player.instance.downAttackTrajectory.DownAttackStat(downAttack.attackMovePoint, downAttack.attackRange);
                                Player.instance.downAttackTrajectory.StartTrajectory();

                                PlayerManager.instance.isDownAttacking = true;
                            }
                        }

                        // HOLD - 마우스 위치로 라인 이동
                        if (isCharging && Input.GetKey(key2))
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        // UP - 스킬 발동
                        if (isCharging && Input.GetKeyUp(key2))
                        {
                            isCharging = false;
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time;

                            Player.instance.downAttackTrajectory.ExecuteDownAttack();
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));

                            PlayerManager.instance.isDownAttacking = false;
                        }
                        break;
                    case 4:
                        var key3 = GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5];

                        // DOWN - 스킬 시작 조건 체크
                        if (Input.GetKeyDown(key3) && !isCharging)
                        {
                            if (Time.time >= skillCoolTime + downAttack.coolTime &&
                                Player.instance.currentStamina >= downAttack.StaminaConsumption &&
                                PlayerManager.instance.isGround)
                            {
                                isCharging = true;

                                Player.instance.currentStamina -= downAttack.StaminaConsumption;
                                GameCanvas.instance.SliderChange(1, 1, downAttack.StaminaConsumption);
                                Player.instance.downAttackTrajectory.DownAttackStat(downAttack.attackMovePoint, downAttack.attackRange);
                                Player.instance.downAttackTrajectory.StartTrajectory();

                                PlayerManager.instance.isDownAttacking = true;
                            }
                        }

                        // HOLD - 마우스 위치로 라인 이동
                        if (isCharging && Input.GetKey(key3))
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        // UP - 스킬 발동
                        if (isCharging && Input.GetKeyUp(key3))
                        {
                            isCharging = false;
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time;

                            Player.instance.downAttackTrajectory.ExecuteDownAttack();
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));

                            PlayerManager.instance.isDownAttacking = false;
                        }
                        break;
                    case 5:
                        var key4 = GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6];

                        // DOWN - 스킬 시작 조건 체크
                        if (Input.GetKeyDown(key4) && !isCharging)
                        {
                            if (Time.time >= skillCoolTime + downAttack.coolTime &&
                                Player.instance.currentStamina >= downAttack.StaminaConsumption &&
                                PlayerManager.instance.isGround)
                            {
                                isCharging = true;

                                Player.instance.currentStamina -= downAttack.StaminaConsumption;
                                GameCanvas.instance.SliderChange(1, 1, downAttack.StaminaConsumption);
                                Player.instance.downAttackTrajectory.DownAttackStat(downAttack.attackMovePoint, downAttack.attackRange);
                                Player.instance.downAttackTrajectory.StartTrajectory();

                                PlayerManager.instance.isDownAttacking = true;
                            }
                        }

                        // HOLD - 마우스 위치로 라인 이동
                        if (isCharging && Input.GetKey(key4))
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        // UP - 스킬 발동
                        if (isCharging && Input.GetKeyUp(key4))
                        {
                            isCharging = false;
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time;

                            Player.instance.downAttackTrajectory.ExecuteDownAttack();
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));

                            PlayerManager.instance.isDownAttacking = false;
                        }
                        break;
                    case 6:
                        var key5 = GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7];

                        // DOWN - 스킬 시작 조건 체크
                        if (Input.GetKeyDown(key5) && !isCharging)
                        {
                            if (Time.time >= skillCoolTime + downAttack.coolTime &&
                                Player.instance.currentStamina >= downAttack.StaminaConsumption &&
                                PlayerManager.instance.isGround)
                            {
                                isCharging = true;

                                Player.instance.currentStamina -= downAttack.StaminaConsumption;
                                GameCanvas.instance.SliderChange(1, 1, downAttack.StaminaConsumption);
                                Player.instance.downAttackTrajectory.DownAttackStat(downAttack.attackMovePoint, downAttack.attackRange);
                                Player.instance.downAttackTrajectory.StartTrajectory();

                                PlayerManager.instance.isDownAttacking = true;
                            }
                        }

                        // HOLD - 마우스 위치로 라인 이동
                        if (isCharging && Input.GetKey(key5))
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        // UP - 스킬 발동
                        if (isCharging && Input.GetKeyUp(key5))
                        {
                            isCharging = false;
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time;

                            Player.instance.downAttackTrajectory.ExecuteDownAttack();
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));

                            PlayerManager.instance.isDownAttacking = false;
                        }
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 2:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3]))
                        {

                        }
                        break;
                    case 3:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4]))
                        {

                        }
                        break;
                    case 4:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5]))
                        {

                        }
                        break;
                    case 5:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6]))
                        {

                        }
                        break;
                    case 6:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7]))
                        {

                        }
                        break;
                }
            }
        }
    }

    void DownAttackInputSetting(int index, SkillData _downattack)
    {
        if (Player.instance.currentStamina >= _downattack.StaminaConsumption && Time.time >= skillCoolTime + _downattack.coolTime && PlayerManager.instance.isGround == true)
        {
            Player.instance.currentStamina -= _downattack.StaminaConsumption;
            GameCanvas.instance.SliderChange(1, 1, _downattack.StaminaConsumption);
            Player.instance.downAttackTrajectory.DownAttackStat(_downattack.attackMovePoint, _downattack.attackRange);

            PlayerManager.instance.isDownAttacking = true;
            Player.instance.downAttackTrajectory.StartTrajectory();        
        }
    }

    IEnumerator UpdateCooldownUI(float cooldown)
    {
        float elapsed = 0f; // 경과 시간
        while (elapsed < cooldown)
        {
            elapsed = Time.time - skillCoolTime;
            img.fillAmount = Mathf.Clamp01(elapsed / cooldown); // 0 → 1로 증가
            yield return null; // 한 프레임 대기 후 다시 실행
        }
        img.fillAmount = 1f; // 쿨다운 종료 후 완전히 차도록 설정
    }

    public void SkillUIZero()
    {
        img.sprite = null;
        Utils.OnOff(img.gameObject, false);
        skillName = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 마우스 오른쪽 클릭
        {
            if(skillName != string.Empty)
            {
                SkillData skillToEquip = PlayerManager.instance.player.skill.accquisitionSkillDataList.Find(skill => skill.nameKor == skillName);
                if(skillToEquip != null)
                {
                    skillToEquip.equipPostion = SkillEquipPosition.None;
                    SkillUIZero();
                }
            }
        }
    }
}

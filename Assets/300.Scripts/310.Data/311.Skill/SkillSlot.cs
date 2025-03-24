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
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3]) && canUseSkill)
                        {
                            DownAttackInputSetting(index, downAttack);                         
                        }

                        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        if (Input.GetKeyUp(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time; // 스킬 사용 시간을 갱신
                            Player.instance.downAttackTrajectory.ExecuteDownAttack(); // 실제 이동
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));
                        }
                        break;
                    case 3:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4]) && canUseSkill)
                        {
                            DownAttackInputSetting(index, downAttack);
                        }

                        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        if (Input.GetKeyUp(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time; // 스킬 사용 시간을 갱신
                            Player.instance.downAttackTrajectory.ExecuteDownAttack(); // 실제 이동
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));
                        }
                        break;
                    case 4:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5]) && canUseSkill)
                        {
                            DownAttackInputSetting(index, downAttack);
                        }

                        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        if (Input.GetKeyUp(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time; // 스킬 사용 시간을 갱신
                            Player.instance.downAttackTrajectory.ExecuteDownAttack(); // 실제 이동
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));
                        }
                        break;
                    case 5:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6]) && canUseSkill)
                        {
                            DownAttackInputSetting(index, downAttack);
                        }

                        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        if (Input.GetKeyUp(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time; // 스킬 사용 시간을 갱신
                            Player.instance.downAttackTrajectory.ExecuteDownAttack(); // 실제 이동
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));
                        }
                        break;
                    case 6:
                        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7]) && canUseSkill)
                        {
                            DownAttackInputSetting(index, downAttack);
                        }

                        if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = true;
                            Player.instance.downAttackTrajectory.MoveTrajectory();
                        }

                        if (Input.GetKeyUp(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7]) && canUseSkill)
                        {
                            PlayerManager.instance.isAiming = false;
                            skillCoolTime = Time.time; // 스킬 사용 시간을 갱신
                            Player.instance.downAttackTrajectory.ExecuteDownAttack(); // 실제 이동
                            StartCoroutine(UpdateCooldownUI(downAttack.coolTime));
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

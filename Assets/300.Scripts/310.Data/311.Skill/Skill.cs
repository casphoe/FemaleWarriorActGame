using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Skill
{
    //스킬 리스트
    public List<SkillData> skillDataList = new List<SkillData>();

    //습득한 스킬 리스트
    public List<SkillData> accquisitionSkillDataList = new List<SkillData>();

    //스킬 레벨업
    public int LevelUpSkill(SkillData skillDb)
    {
        SkillData skill = skillDb;
        
        //최대 레벨 체크
        if(skill.level == SkillLevel.five)
        {
            return 0;
        }

        //선행 스킬 체크
        if(skill.requiredSkill != null)
        {
            SkillData prerequisiteSkillDB = accquisitionSkillDataList.Find(s => s == skill.requiredSkill);
            //선행 스킬을 습득 못함
            if (prerequisiteSkillDB == null || prerequisiteSkillDB.level == SkillLevel.zero)
            {
                return 1;
            }
        }

        int requiredPoints = skill.acquisitionPoints;
        if(PlayerManager.instance.player.skillCount < requiredPoints)
        {
            //스킬 포인트 부족
            return 2;
        }

        //스킬 포인트 차감 후 레벨 업
        PlayerManager.instance.player.skillCount -= requiredPoints;
        skill.level += 1;
            
        //스킬 레벨업 시 능력치 증가 적용
        skill.ApplyLevelUpBonuses(skill);

        //스킬이 처음 습득될 경우 리스트에 추가
        if(skill.level == SkillLevel.one)
        {
            SkillData newSkillDB = new SkillData();
            newSkillDB =  skill;
            accquisitionSkillDataList.Add(newSkillDB);
        }

        return 3;
    }

    //스킬 레벨업 다운
    public int levelDownSkill(SkillData skillDb)
    {
        SkillData skill = skillDb;

        // 최소 레벨 체크
        if (skill.level == SkillLevel.zero)
        {
            return 0; // 미습득 상태
        }

        // 선행 스킬 체크
        if (skill.requiredSkill != null)
        {
            SkillData prerequisiteSkillDB = accquisitionSkillDataList.Find(s => s == skill.requiredSkill);
            // 선행 스킬이 존재하고, 습득된 상태라면 레벨 1 이하로 내릴 수 없음
            if (prerequisiteSkillDB != null && prerequisiteSkillDB.level >= SkillLevel.one && skill.level == SkillLevel.one)
            {
                return 1; // 레벨 다운 불가
            }
        }

        // 레벨 다운 시 스킬 포인트를 되돌려준다.
        int previousAcquisitionPoints = (int)skill.levelUpBonuses[skill.level]["acquisitionPoints"];
        PlayerManager.instance.player.skillCount += previousAcquisitionPoints;

        //내리기 전 현제 레벨을 저장
        int currentLevel = (int)skill.level;

        //현재 레벨에 적용된 보너스를 제거
        if(skill.levelUpBonuses.ContainsKey((SkillLevel)currentLevel))
        {
            foreach (var bonus in skill.levelUpBonuses[(SkillLevel)currentLevel])
            {
                skill.RevertBonus(skill, bonus.Key, bonus.Value);
                switch(bonus.Key)
                {
                    case "HpUp":
                        PlayerManager.instance.player.hp -= bonus.Value;
                        Player.instance.currentHp -= bonus.Value;
                        break;
                    case "StaminaUp":
                        PlayerManager.instance.player.stamina -= bonus.Value;
                        Player.instance.currentStamina -= bonus.Value;
                        break;
                    case "attackUp":
                        PlayerManager.instance.player.attack -= bonus.Value;
                        break;
                    case "defenceUp":
                        PlayerManager.instance.player.defense -= bonus.Value;
                        break;
                    case "crictleRateUp":
                        PlayerManager.instance.player.critcleRate -= bonus.Value;
                        break;
                    case "crictleDmgUp":
                        PlayerManager.instance.player.critcleDmg -= bonus.Value;
                        break;
                    case "HpRestoration":
                        PlayerManager.instance.player.hpAutoRestoration -= bonus.Value;
                        break;
                    case "StaminaRestoration":
                        PlayerManager.instance.player.staminaAutoRestoration -= bonus.Value;
                        break;
                }
            }
            GameCanvas.instance.SliderEquipChange();
        }

        // 이전 보너스 정보 갱신 (있다면)
        if (skill.skillPreviousBonuses.ContainsKey(skill))
        {
            skill.skillPreviousBonuses[skill].Clear();
        }

        //스킬 레벨 다운
        skill.level = (SkillLevel)(currentLevel - 1);

        //스킬 레벨 다운시 능력치 감소 적용
        // 새로 적용할 레벨에 대해 보너스를 다시 적용합니다.
        if (skill.level != SkillLevel.zero && skill.levelUpBonuses.ContainsKey(skill.level))
        {
            foreach (var bonus in skill.levelUpBonuses[skill.level])
            {
                skill.ApplyBonus(skill, bonus.Key, bonus.Value);
                // 보너스 적용 정보를 저장 (추후 레벨 업/다운 시 활용)
                if (!skill.skillPreviousBonuses.ContainsKey(skill))
                {
                    skill.skillPreviousBonuses[skill] = new Dictionary<string, float>();
                }
                skill.skillPreviousBonuses[skill][bonus.Key] = bonus.Value;
            }
        }

        //스킬 레벨이 zero가 되면 
        if (skill.level == SkillLevel.zero)
        {
            //해당 스킬만 리스트에서 제거
            accquisitionSkillDataList.RemoveAll(s => s == skill);         
        }

        return 2;
    }

    //스킬 이름으로 스킬 레벨을 찾는 삼수
    public SkillLevel GetSkillLevelByName(string name)
    {
        SkillData skillDb = accquisitionSkillDataList.Find(s => s.nameKor == name);
        return skillDb != null ? skillDb.level : SkillLevel.zero;
    }

    //스킬 이름으로 스킬 쿨타임 가져오는 함수
    public float GetSkillCoolTime(string name)
    {
        SkillData skillDb = accquisitionSkillDataList.Find(s => s.nameKor == name);
        if (skillDb != null)
        {
            return skillDb.coolTime;
        }

        return -1;
    }


    public void CateGorizeSkillsByPosition(List<SkillData> skillList, int num)
    {
        skillList.Clear();

        switch(num)
        {
            case 0:
                foreach(var skill in skillDataList)
                {
                    if(skill.poistion == SkillPoistion.Active)
                    {
                        skillList.Add(skill);
                    }
                }
                break;
            case 1:
                foreach (var skill in skillDataList)
                {
                    if (skill.poistion == SkillPoistion.Passive)
                    {
                        skillList.Add(skill);
                    }
                }
                break;
        }
    }
}
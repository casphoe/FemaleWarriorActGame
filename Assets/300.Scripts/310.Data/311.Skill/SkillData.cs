using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillLevel
{
    zero,one,two,three,four,five
}

public enum SkillPoistion
{
    Active, Passive
}

public enum SkillEquipPosition
{
    None,NotRegistration,three,four,five,six,seven
}

[Serializable]
public class SkillData 
{
    public string nameEng; //스킬 영어 이름

    public string nameKor; //스킬 한국 이름

    public SkillLevel level;

    public SkillPoistion poistion; 

    public SkillEquipPosition equipPostion; // 단축키

    public int acquisitionPoints; //습득시 드는 포인트 량

    public float coolTime; //스킬 쿨타임

    public float currentCoolTime; //현재 스킬 남은 쿨타임

    public float buffTime; //스킬 시전시 이로운 효과 발동 제한 시간

    public float attackUp; //캐릭터 공격력 증가

    public float defenceUp; //캐릭터 방어력 증가

    public float crictleRateUp;

    public float crictleDmgUp;

    public float HpUp;

    public float StaminaUp;

    public float HpRestoration; //체력 자동 회복

    public float StaminaRestoration; //스태미나 자동 회복

    public float damage; //몬스터에서 들어가는 데미지

    public float attackRange; //다운어택에서 사용되는 떨어졌을 때 충돌 범위

    public float attackMovePoint; //다운어택 이동 범위

    public float StaminaConsumption; //스킬 사용시 들어가는 스태미나 량

    public float DamageAbsorption; //데미지 흡수

    public int skillConditionsRate; //스킬 조건 확률

    public SkillData requiredSkill; // 🔥 필수 선행 스킬 (없으면 null)

    // 개별 능력치 증가량 (스킬마다 다르게 설정 가능)
    public Dictionary<SkillLevel, Dictionary<string, float>> levelUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>();

    // 기존 레벨에서 가지고 있는 스킬 증가량
    public Dictionary<SkillData, Dictionary<string, float>> skillPreviousBonuses = new Dictionary<SkillData, Dictionary<string, float>>();


    public void SkillSetting(string _eng, string _kor, SkillLevel _level, SkillPoistion _position, SkillEquipPosition _equip, int _points, float _cool, float _current, float _buff,
        float _attackUp, float _defenceUp, float _crictleRateUp, float _crictleDmgUp, float _hpUp, float _staminaUp,float _hpRestoration, float _staminaRestoration, float _damage, float _attackRange, float _attackMovePoint, 
        float _staminaConsumption,float _DamageAbsorption,int _skillConditionsRate, Dictionary<SkillLevel, Dictionary<string, float>> _levelUpBonuses, SkillData _requiredSkill = null)
    {
        nameEng = _eng;
        nameKor = _kor;
        level = _level;
        poistion = _position;
        equipPostion = _equip;
        acquisitionPoints = _points;
        coolTime = _cool;
        currentCoolTime = _current;
        buffTime = _buff;
        attackUp = _attackUp;
        defenceUp = _defenceUp;
        crictleRateUp = _crictleRateUp;
        crictleDmgUp = _crictleDmgUp;
        HpUp = _hpUp;
        StaminaUp = _staminaUp;
        HpRestoration = _hpRestoration;
        StaminaRestoration = _staminaRestoration;
        damage = _damage;
        attackRange = _attackRange;
        attackMovePoint = _attackMovePoint;
        StaminaConsumption = _staminaConsumption;
        DamageAbsorption = _DamageAbsorption;
        skillConditionsRate = _skillConditionsRate;
        levelUpBonuses = _levelUpBonuses;

        requiredSkill = _requiredSkill;
    }

    // 레벨업 시 증가 적용 (각 능력치별로 설정한 값만 증가)
    public void ApplyLevelUpBonuses(SkillData skill)
    {
        //스킬별 이전 증가량 저장소 가져오기(없으면 새로 생성)
        if (!skillPreviousBonuses.ContainsKey(skill))
        {
            skillPreviousBonuses[skill] = new Dictionary<string, float>();
        }

        var previousBonuses = skillPreviousBonuses[skill];

        if (skill.levelUpBonuses.ContainsKey(skill.level))
        {
            foreach (var bonus in skill.levelUpBonuses[skill.level])
            {
                if (previousBonuses.ContainsKey(bonus.Key))
                {
                    // 기존 스킬 증가량 제거
                    RevertBonus(skill, bonus.Key, previousBonuses[bonus.Key]);
                }

                // 새로운 증가량 적용
                ApplyBonus(skill, bonus.Key, bonus.Value);
                //현재 증가량을 저장 (스킬 레벨업 한 스킬 데이터만 저장)
                previousBonuses[bonus.Key] = bonus.Value;
            }
        }
    }

    //스킬 스텟 증가
    public void ApplyBonus(SkillData skill, string stat, float value)
    {
        switch (stat)
        {
            case "coolTime": skill.coolTime = value; break;
            case "buffTime": skill.buffTime = value; break;
            case "attackUp": skill.attackUp += value; break;
            case "defenceUp": skill.defenceUp += value; break;
            case "crictleRateUp": skill.crictleRateUp += value; break;
            case "crictleDmgUp": skill.crictleDmgUp += value; break;
            case "HpUp": skill.HpUp += value; break;
            case "StaminaUp": skill.StaminaUp += value; break;
            case "damage": skill.damage += value; break;
            case "attackRange": skill.attackRange += value; break;
            case "attackMovePoint": skill.attackMovePoint += value; break;
            case "StaminaConsumption": skill.StaminaConsumption += value; break;
            case "HpRestoration": skill.HpRestoration += value; break;
            case "StaminaRestoration": skill.StaminaRestoration += value; break;
            case "DamageAbsorption": skill.DamageAbsorption += value; break;
            case "skillConditionsRate": skill.skillConditionsRate += (int)value; break;
            case "acquisitionPoints": skill.acquisitionPoints += (int)value; break;
        }
    }

    //스킬 스텟 감소
    public void RevertBonus(SkillData skill,string stat, float value)
    {
        switch (stat)
        {
            case "coolTime": skill.coolTime = value; break;
            case "buffTime": skill.buffTime = value; break;
            case "attackUp": skill.attackUp -= value; break;
            case "defenceUp": skill.defenceUp -= value; break;
            case "crictleRateUp": skill.crictleRateUp -= value; break;
            case "crictleDmgUp": skill.crictleDmgUp -= value; break;
            case "HpUp": skill.HpUp -= value; break;
            case "StaminaUp": skill.StaminaUp -= value; break;
            case "damage": skill.damage -= value; break;
            case "attackRange": skill.attackRange -= value; break;
            case "attackMovePoint": skill.attackMovePoint -= value; break;
            case "HpRestoration": skill.HpRestoration -= value; break;
            case "StaminaRestoration": skill.StaminaRestoration -= value; break;
            case "StaminaConsumption": skill.StaminaConsumption -= value; break;
            case "DamageAbsorption": skill.DamageAbsorption -= value; break;
            case "skillConditionsRate": skill.skillConditionsRate -= (int)value; break;
            case "acquisitionPoints": skill.acquisitionPoints -= (int)value; break;
        }
    }
}

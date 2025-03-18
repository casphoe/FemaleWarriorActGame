using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSetting : MonoBehaviour
{
    public static SkillSetting instance;


    public List<SkillData> skillPanelList = new List<SkillData>();

    private void Awake()
    {
        instance = this;
    }

    //스킬의 대한 함수 => 스킬 추가
    public void _SkillSetting()
    {
        _PassiveSkillSetting();
        _ActiveSkillSetting();
    }

    public void _skillDataSetting(int num)
    {
        PlayerManager.instance.player.skill.CateGorizeSkillsByPosition(skillPanelList, num);
    }

    void _PassiveSkillSetting()
    {
        #region 체력 증가 패시브
        var HpLevlSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero,new Dictionary<string, float>
                {
                    {"HpUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one,new Dictionary<string, float>
                {
                    {"HpUp",50 },
                    {"acquisitionPoints",2 }
                }
            },
            {
                SkillLevel.two,new Dictionary<string, float>
                {
                    {"HpUp",100 },
                    {"acquisitionPoints",4 }
                }
            },
             {
                SkillLevel.three,new Dictionary<string, float>
                {
                    {"HpUp",150 },
                    {"acquisitionPoints",6 }
                }
            },
              {
                SkillLevel.four,new Dictionary<string, float>
                {
                    {"HpUp",200 },
                    {"acquisitionPoints",8 }
                }
            },
               {
                SkillLevel.five,new Dictionary<string, float>
                {
                    {"HpUp",250 },
                    {"acquisitionPoints",10 }
                }
            },
        };

        SkillData passiveHpUpSkill = new SkillData();

        passiveHpUpSkill.SkillSetting("HpUp", "생명력 증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
           0,0,0,0,HpLevlSkillUpBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveHpUpSkill);

        #endregion

        #region 스태미나 패시브 스킬

        var StminaSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"StaminaUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one,  new Dictionary<string, float>
                {
                    {"StaminaUp",25 },
                    {"acquisitionPoints",2 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"StaminaUp",50 },
                    {"acquisitionPoints",4 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"StaminaUp",75 },
                    {"acquisitionPoints",6 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"StaminaUp",100 },
                    {"acquisitionPoints",8 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"StaminaUp",125 },
                    {"acquisitionPoints", 10 }
                }
            }
        };

        SkillData passiveStaminaUpSkill = new SkillData();

        passiveStaminaUpSkill.SkillSetting("StaminaUp", "스태미나 증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0,0,0,0,StminaSkillUpBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveStaminaUpSkill);
        #endregion

        #region 공격력 증가 패시브 스킬

        var AttackSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"attackUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"attackUp",1 },
                    {"acquisitionPoints",2 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"attackUp",2 },
                    {"acquisitionPoints",4 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"attackUp",3 },
                    {"acquisitionPoints",6 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"attackUp",4 },
                    {"acquisitionPoints",8 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"attackUp",5 },
                    {"acquisitionPoints",10 }
                }
            }
        };

        SkillData passiveAttackUpSkill = new SkillData();

        passiveAttackUpSkill.SkillSetting("AttackUp", "공격력 증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 2, 0, 0, 0, 0, 0, 0, 0
            ,0, 0, 0, 0, 0, 0, 0, 0, 0, 0, AttackSkillUpBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveAttackUpSkill);

        #endregion

        #region 방어력 증가 패시브 스킬

        var DefenceSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"defenceUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"defenceUp", 1},
                    {"acquisitionPoints", 2}
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"defenceUp", 2},
                    {"acquisitionPoints", 4}
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"defenceUp", 3},
                    {"acquisitionPoints", 6}
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"defenceUp", 4},
                    {"acquisitionPoints", 8}
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"defenceUp", 5},
                    {"acquisitionPoints", 10}
                }
            }
        };

        SkillData passiveDefenceUpSkill = new SkillData();

        passiveDefenceUpSkill.SkillSetting("DefenceUp", "방어력 증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 2, 0, 0, 0, 0, 0, 0
            , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, DefenceSkillUpBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveDefenceUpSkill);

        #endregion

        #region 체력 자동 회복 패시브 스킬

        var HpAutoRestorationSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"HpRestoration",0 },
                    {"acquisitionPoints",0 },
                    {"coolTime", 0 }
                }
            },
            {
                SkillLevel.one , new Dictionary<string, float>
                {
                    {"HpRestoration",3 },
                    {"acquisitionPoints",1 },
                    {"coolTime", 60 }
                }
            },
            {
                SkillLevel.two , new Dictionary<string, float>
                {
                    {"HpRestoration",6 },
                    {"acquisitionPoints",2 },
                    {"coolTime", 55 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"HpRestoration",9 },
                    {"acquisitionPoints",3 },
                    {"coolTime", 50 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"HpRestoration",12 },
                    {"acquisitionPoints",4 },
                    {"coolTime", 45 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"HpRestoration",15 },
                    {"acquisitionPoints",5 },
                    {"coolTime", 40 }
                }
            }
        };

        SkillData passiveAutoHpSkillUp = new SkillData();
        //체력증가를 찍어야지 체력 자동 회복증가를 찍을 수 있음
        passiveAutoHpSkillUp.SkillSetting("AutoHpRestUp", "자동체력회복증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 1, 60, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, HpAutoRestorationSkillUpBonuses, passiveHpUpSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveAutoHpSkillUp);

        #endregion

        #region 스태미나 자동 회복 패시브 스킬

        var StaminaAutoRestorationSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"StaminaRestoration",0 },
                    {"acquisitionPoints",0 },
                    {"coolTime", 0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"StaminaRestoration",3 },
                    {"acquisitionPoints",1 },
                    {"coolTime", 40 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"StaminaRestoration",6 },
                    {"acquisitionPoints",2 },
                    {"coolTime", 35 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"StaminaRestoration",9 },
                    {"acquisitionPoints",3 },
                    {"coolTime", 30 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"StaminaRestoration",12 },
                    {"acquisitionPoints",4 },
                    {"coolTime", 25 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"StaminaRestoration",15 },
                    {"acquisitionPoints",5 },
                    {"coolTime", 20 }
                }
            }
        };

        SkillData passiveAutoStaminaSkillUp = new SkillData();

        //스태미나 증가를 찍어야지 스태미나 자동회복 스킬 스킬을 습득 가능
        passiveAutoStaminaSkillUp.SkillSetting("AutoStaminaRestUp", "자동스태미나회복증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 1, 40, 0, 0, 0, 0, 0
            , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, StaminaAutoRestorationSkillUpBonuses, passiveStaminaUpSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveAutoStaminaSkillUp);

        #endregion

        #region 크리티컬 데미지 증가 패시브 스킬

        var CrictleDamgeUpSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"crictleDmgUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"crictleDmgUp",10 },
                    {"acquisitionPoints",3 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"crictleDmgUp",20 },
                    {"acquisitionPoints",6 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"crictleDmgUp",30 },
                    {"acquisitionPoints",9 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"crictleDmgUp",40 },
                    {"acquisitionPoints",12 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"crictleDmgUp",50 },
                    {"acquisitionPoints",15 }
                }
            }
        };

        SkillData passiveCrictleDamgeSkillUp = new SkillData();

        passiveCrictleDamgeSkillUp.SkillSetting("CrictleDamgeUp", "크리티컬데미지업", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 3, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CrictleDamgeUpSkillUpBonuses, passiveAttackUpSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveCrictleDamgeSkillUp);

        #endregion

        #region 공방 증가 패시브 스킬

        var AttackDefenceUpSkillUoBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"attackUp",0 },
                    {"defenceUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"attackUp",1 },
                    {"defenceUp",1 },
                    {"acquisitionPoints",4 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"attackUp",2 },
                    {"defenceUp",2 },
                    {"acquisitionPoints",8 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"attackUp",3 },
                    {"defenceUp",3 },
                    {"acquisitionPoints",12 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"attackUp",4 },
                    {"defenceUp",4 },
                    {"acquisitionPoints",16 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"attackUp",5 },
                    {"defenceUp",5 },
                    {"acquisitionPoints",20 }
                }
            }
        };

        SkillData passiveAttackDefenceSkillUp = new SkillData();
        passiveAttackDefenceSkillUp.SkillSetting("AttackDefenceUp", "공방증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, AttackDefenceUpSkillUoBonuses, passiveDefenceUpSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveAttackDefenceSkillUp);

        #endregion

        #region 체력 & 스태미나 증가 패시브 스킬

        var HpStaminaUpSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"HpUp",0 },
                    {"StaminaUp",0 },
                    {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"HpUp",50 },
                    {"StaminaUp",25 },
                    {"acquisitionPoints",4 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"HpUp",100 },
                    {"StaminaUp",50 },
                    {"acquisitionPoints",8 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"HpUp",150 },
                    {"StaminaUp",75 },
                    {"acquisitionPoints",12 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"HpUp",200 },
                    {"StaminaUp",100 },
                    {"acquisitionPoints",16 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"HpUp",250 },
                    {"StaminaUp",125 },
                    {"acquisitionPoints",20 }
                }
            }
        };

        //패시브 자동회복 스킬이 1이상 습득 해야지 지금 스킬을 습득 할 수 있음
        SkillData passiveHpStaminaSkillUp = new SkillData();
        passiveHpStaminaSkillUp.SkillSetting("HpStaminaUp", "체력스태미나증가", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 4, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, HpStaminaUpSkillUpBonuses, passiveAutoHpSkillUp);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveHpStaminaSkillUp);

        #endregion

        #region 크리티컬 확률 증가 패시브 스킬

        var CrictleRateSkillUpBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                     {"crictleRateUp",0 },
                     {"acquisitionPoints",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"crictleRateUp",1.5f },
                    {"acquisitionPoints",6 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"crictleRateUp",3 },
                    {"acquisitionPoints",12 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"crictleRateUp",4.5f },
                    {"acquisitionPoints",18 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"crictleRateUp",6 },
                    {"acquisitionPoints",24 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"crictleRateUp",7.5f },
                    {"acquisitionPoints",30 }
                }
            }
        };

        SkillData passiveCrictleRateSkillUp = new SkillData();
        passiveCrictleRateSkillUp.SkillSetting("CrictleRateUp", "크리티컬확률업", SkillLevel.zero, SkillPoistion.Passive, SkillEquipPosition.None, 6, 0, 0, 0, 0, 0, 0, 0
            , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, CrictleRateSkillUpBonuses, passiveCrictleDamgeSkillUp);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveCrictleRateSkillUp);

        #endregion
    }

    void _ActiveSkillSetting()
    {
        #region 다운 어택
        var DownAttackSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"damage", 0 },
                    {"attackRange", 0 },
                    {"attackMovePoint", 0},
                    {"acquisitionPoints",0 },
                    {"coolTime",0 },
                    {"StaminaConsumption",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"damage", 1.5f },
                    {"attackRange", 0.5f },
                    {"attackMovePoint", 0.5f},
                    {"acquisitionPoints",4 },
                    {"coolTime", 30 },
                    {"StaminaConsumption", 20 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"damage", 3 },
                    {"attackRange", 1 },
                    {"attackMovePoint", 1},
                    {"acquisitionPoints",8 },
                    {"coolTime", 25 },
                    {"StaminaConsumption", 25 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"damage", 4.5f },
                    {"attackRange", 1.5f },
                    {"attackMovePoint", 1.5f},
                    {"acquisitionPoints",12 },
                    {"coolTime", 20 },
                    {"StaminaConsumption",30 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"damage", 6 },
                    {"attackRange", 2 },
                    {"attackMovePoint", 2},
                    {"acquisitionPoints",16 },
                    {"coolTime", 15 },
                    {"StaminaConsumption",35 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"damage", 7.5f },
                    {"attackRange", 2.5f },
                    {"attackMovePoint", 2.5f},
                    {"acquisitionPoints",20 },
                    {"coolTime", 10 },
                    {"StaminaConsumption",40 }
                }
            }
        };

        SkillData downAttackSkillUp = new SkillData();
        downAttackSkillUp.SkillSetting("DownAttack", "다운어택", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.None, 4, 30, 0, 0, 0, 0, 0, 0, 0, 0
            , 0, 0, 0, 0, 0, 0, 0, 0, DownAttackSkillBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(downAttackSkillUp);
        #endregion

        #region 화염 인챈트 

        var fireWeaponSetSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                     {"acquisitionPoints",0 },
                     {"coolTime", 0 },
                     {"buffTime",0 },
                     {"StaminaConsumption", 0 }
                }
            },
            {
                SkillLevel.one,new Dictionary<string, float>
                {
                     {"acquisitionPoints",3 },
                     {"coolTime", 45 },
                     {"buffTime",5 },
                     {"StaminaConsumption", 5 }
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                     {"acquisitionPoints",6 },
                     {"coolTime", 41 },
                     {"buffTime",8 },
                     {"StaminaConsumption", 10 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                     {"acquisitionPoints",9 },
                     {"coolTime", 37},
                     {"buffTime",11 },
                     {"StaminaConsumption", 15 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"acquisitionPoints",12 },
                    {"coolTime", 33},
                    {"buffTime",14 },
                    {"StaminaConsumption", 20 }
                }
            },
            {
                SkillLevel.five ,new Dictionary<string, float>
                {
                    {"acquisitionPoints",15 },
                    {"coolTime",  29},
                    {"buffTime", 17 },
                    {"StaminaConsumption", 25 }
                }
            }
        };

        SkillData FireWeaponSkillUp = new SkillData();

        FireWeaponSkillUp.SkillSetting("Flame enchantment", "화염인챈트", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.None, 3, 45, 0, 5, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 3, 0, 0, fireWeaponSetSkillBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(FireWeaponSkillUp);

        #endregion

        #region 버서커 공격력 증가, 스태미나 소모 감소, 방어력 감소

        var BerserkserSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                     {"acquisitionPoints",0 },
                     {"coolTime", 0 },
                     {"buffTime",0 },
                     {"StaminaConsumption", 0 },
                     {"attackUp",0 },
                     {"defenceUp",0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                     {"acquisitionPoints",4 },
                     {"coolTime", 60 },
                     {"buffTime", 10 },
                     {"StaminaConsumption", 20 },
                     {"attackUp", 3 },
                     {"defenceUp",-2 }
                }
            },
            {
                SkillLevel.two , new Dictionary<string, float>
                {
                     {"acquisitionPoints",8 },
                     {"coolTime", 55 },
                     {"buffTime", 12 },
                     {"StaminaConsumption", 25 },
                     {"attackUp", 6 },
                     {"defenceUp",-5 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"acquisitionPoints",12 },
                    {"coolTime", 50 },
                    {"buffTime", 15 },
                    {"StaminaConsumption", 30 },
                    {"attackUp", 9 },
                    {"defenceUp",-8 }
                }
            },
            {
                SkillLevel.four ,new Dictionary<string, float>
                {
                    {"acquisitionPoints",16 },
                    {"coolTime", 45 },
                    {"buffTime", 19 },
                    {"StaminaConsumption", 35 },
                    {"attackUp", 12 },
                    {"defenceUp",-11 }
                }
            },
            {
                SkillLevel.five ,new Dictionary<string, float>
                {
                    {"acquisitionPoints",20 },
                    {"coolTime", 40 },
                    {"buffTime", 25 },
                    {"StaminaConsumption", 40 },
                    {"attackUp", 16 },
                    {"defenceUp",-15 }
                }
            }
        };

        SkillData BerserkserSkill = new SkillData();
        BerserkserSkill.SkillSetting("Berserker", "버서커", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.None, 4, 60, 0, 10, 3, -2, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 20, 0, 0, BerserkserSkillBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(BerserkserSkill);

        #endregion

        #region 실드 (데미지 흡수)

        var absoulteDamgeSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"acquisitionPoints",0 },
                    {"coolTime", 0 },
                    {"buffTime",0 },
                    {"StaminaConsumption", 0 },
                    {"DamageAbsorption", 0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"acquisitionPoints",5 },
                    {"coolTime", 60 },
                    {"buffTime",10 },
                    {"StaminaConsumption", 10 },
                    {"DamageAbsorption", 5 }
                }
            },
            {
                SkillLevel.two ,new Dictionary<string, float>
                {
                    {"acquisitionPoints",5 },
                    {"coolTime", 60 },
                    {"buffTime",12 },
                    {"StaminaConsumption", 15 },
                    {"DamageAbsorption", 5 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"acquisitionPoints",10 },
                    {"coolTime", 55 },
                    {"buffTime",14 },
                    {"StaminaConsumption", 20 },
                    {"DamageAbsorption", 10 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"acquisitionPoints",15 },
                    {"coolTime", 50 },
                    {"buffTime",16 },
                    {"StaminaConsumption", 25 },
                    {"DamageAbsorption", 15 }
                }
            },
            {
                SkillLevel.five , new Dictionary<string, float>
                {
                    {"acquisitionPoints",20 },
                    {"coolTime", 45 },
                    {"buffTime",18 },
                    {"StaminaConsumption", 30 },
                    {"DamageAbsorption", 20 }
                }
            }
        };

        SkillData AbsoulteDamgeSkill = new SkillData();

        AbsoulteDamgeSkill.SkillSetting("AbsoulteDamage", "데미지흡수", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.None, 5, 60, 0, 10, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 10, 5, 0, absoulteDamgeSkillBonuses, null);

        PlayerManager.instance.player.skill.skillDataList.Add(AbsoulteDamgeSkill);

        #endregion

        #region 다운 어택 이동 범위 증가

        var DownAttackMoveSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                     {"acquisitionPoints",0 },
                     {"attackMovePoint", 0},
                }
            },
            {
                SkillLevel.one , new Dictionary<string, float>
                {
                    {"acquisitionPoints",5 },
                    {"attackMovePoint", 0.1f},
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"acquisitionPoints",10 },
                    {"attackMovePoint", 0.2f},
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"acquisitionPoints",15 },
                    {"attackMovePoint", 0.3f},
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"acquisitionPoints",20},
                    {"attackMovePoint", 0.4f},
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"acquisitionPoints",20},
                    {"attackMovePoint", 0.5f},
                }
            }         
        };

        SkillData DownAttackMoveSkill = new SkillData();
        DownAttackMoveSkill.SkillSetting("DownAttackMoveUp", "다운어택이동거리증가", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.NotRegistration, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 5, 0, 0, 0, DownAttackMoveSkillBonuses, downAttackSkillUp);

        PlayerManager.instance.player.skill.skillDataList.Add(DownAttackMoveSkill);

        #endregion

        #region 다운 어택 공격 범위 증가

        var DownAttackRangeSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"acquisitionPoints",0 },
                    {"attackRange", 0 },
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"acquisitionPoints",5 },
                    {"attackRange", 0.2f },
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"acquisitionPoints",10 },
                    {"attackRange", 0.4f },
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"acquisitionPoints",15 },
                    {"attackRange", 0.6f },
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"acquisitionPoints",20 },
                    {"attackRange", 0.8f },
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                     {"acquisitionPoints",25 },
                    {"attackRange", 1 },
                }
            }
        };

        SkillData DownAttackRangeSkill = new SkillData();
        DownAttackRangeSkill.SkillSetting("DownAttackRanageUp", "다운어택공격범위증가", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.NotRegistration, 5, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, DownAttackRangeSkillBonuses, DownAttackMoveSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(DownAttackRangeSkill);

        #endregion

        #region 검기 슬래시

        var AttackSlashSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"damage", 0 },
                    {"acquisitionPoints",0 },
                    {"coolTime", 0 },
                    {"StaminaConsumption", 0 },
                     {"attackMovePoint", 0},
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"damage", 6 },
                    {"acquisitionPoints", 8 },
                    {"coolTime", 60 },
                    {"StaminaConsumption", 30 },
                     {"attackMovePoint", 10},
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"damage", 12 },
                    {"acquisitionPoints", 16 },
                    {"coolTime", 55 },
                    {"StaminaConsumption", 40 },
                     {"attackMovePoint", 15},
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"damage", 18 },
                    {"acquisitionPoints", 24 },
                    {"coolTime", 50 },
                    {"StaminaConsumption", 50 },
                     {"attackMovePoint", 20},
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"damage", 24 },
                    {"acquisitionPoints", 32 },
                    {"coolTime", 45 },
                    {"StaminaConsumption", 60 },
                     {"attackMovePoint", 20},
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"damage", 32 },
                    {"acquisitionPoints", 40 },
                    {"coolTime", 40 },
                    {"StaminaConsumption", 70 },
                     {"attackMovePoint", 20},
                }
            }
        };

        SkillData AttackSlashSkill = new SkillData();
        AttackSlashSkill.SkillSetting("SwordSlash", "검기슬래시", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.None, 8, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            6, 0, 10, 30, 0, 0, AttackSlashSkillBonuses, downAttackSkillUp);

        PlayerManager.instance.player.skill.skillDataList.Add(AttackSlashSkill);

        #endregion

        #region 화염 인챈트 지속시간 증가

        var FireEnchantContinueSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                     {"buffTime",0 },
                     {"acquisitionPoints",0 },
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                     {"buffTime",1 },
                     {"acquisitionPoints",2 },
                }
            },
            {
                SkillLevel.two ,new Dictionary<string, float>
                {
                     {"buffTime",2},
                     {"acquisitionPoints",4 },
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"buffTime", 3},
                    {"acquisitionPoints", 6 },
                }
            },
            {
                SkillLevel.four , new Dictionary<string, float>
                {
                    {"buffTime", 4 },
                    {"acquisitionPoints",8 },
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"buffTime", 5 },
                    {"acquisitionPoints",10 },
                }
            }
        };

        SkillData FireEnchanteContinueSkill = new SkillData();
        FireEnchanteContinueSkill.SkillSetting("FlameEnchantDuration", "화염인챈트지속", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.NotRegistration, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, FireEnchantContinueSkillBonuses, FireWeaponSkillUp);

        PlayerManager.instance.player.skill.skillDataList.Add(FireEnchanteContinueSkill);

        #endregion

        #region 버서커 지속 시간 증가

        var BeskerContinues = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero ,new Dictionary<string, float>
                {
                    {"buffTime", 0 },
                    {"acquisitionPoints", 0 },
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"buffTime", 1 },
                    {"acquisitionPoints", 2 },
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"buffTime", 2 },
                    {"acquisitionPoints", 4 },
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"buffTime", 3 },
                    {"acquisitionPoints", 6 },
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"buffTime", 4 },
                    {"acquisitionPoints", 8 },
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"buffTime", 5 },
                    {"acquisitionPoints", 10 },
                }
            }
        };

        SkillData BeskerContinuesSkill = new SkillData();
        BeskerContinuesSkill.SkillSetting("BerserkerDuration", "버서커지속시간증가", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.NotRegistration, 2, 0, 0, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, BeskerContinues, BerserkserSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(BeskerContinuesSkill);

        #endregion

        #region 실드 지속 시간 증가

        var SheildDurationSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"buffTime", 0 },
                    {"acquisitionPoints", 0 },
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"buffTime", 1 },
                    {"acquisitionPoints", 2 },
                }
            },
            {
                SkillLevel.two, new Dictionary<string, float>
                {
                    {"buffTime", 2 },
                    {"acquisitionPoints", 4 },
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"buffTime", 3 },
                    {"acquisitionPoints", 6 },
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"buffTime", 4 },
                    {"acquisitionPoints", 8 },
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"buffTime", 5 },
                    {"acquisitionPoints", 10 },
                }
            }
        };

        SkillData SheildDurationSkill = new SkillData();

        SheildDurationSkill.SkillSetting("ShieldDuration", "실드지속시간증가", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.NotRegistration, 2, 0, 0, 1, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SheildDurationSkillBonuses, AbsoulteDamgeSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(SheildDurationSkill);

        #endregion

        #region 검기 두번 나가는 확률 증가

        var TwiceShowrdSlashSkillBonuses = new Dictionary<SkillLevel, Dictionary<string, float>>()
        {
            {
                SkillLevel.zero, new Dictionary<string, float>
                {
                    {"acquisitionPoints",0 },
                    {"skillConditionsRate", 0 }
                }
            },
            {
                SkillLevel.one, new Dictionary<string, float>
                {
                    {"acquisitionPoints",5 },
                    {"skillConditionsRate", 2 }
                }
            },
            {
                SkillLevel.two ,new Dictionary<string, float>
                {
                    {"acquisitionPoints",10 },
                    {"skillConditionsRate", 4 }
                }
            },
            {
                SkillLevel.three, new Dictionary<string, float>
                {
                    {"acquisitionPoints",15 },
                    {"skillConditionsRate", 6 }
                }
            },
            {
                SkillLevel.four, new Dictionary<string, float>
                {
                    {"acquisitionPoints",20 },
                    {"skillConditionsRate", 8 }
                }
            },
            {
                SkillLevel.five, new Dictionary<string, float>
                {
                    {"acquisitionPoints",25 },
                    {"skillConditionsRate", 10 }
                }
            }
        };

        SkillData TwiceSwordSlashSkill = new SkillData();
        TwiceSwordSlashSkill.SkillSetting("TwiceSwordSlash", "두번검기슬래시", SkillLevel.zero, SkillPoistion.Active, SkillEquipPosition.NotRegistration, 5, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 2, TwiceShowrdSlashSkillBonuses, AttackSlashSkill);

        PlayerManager.instance.player.skill.skillDataList.Add(TwiceSwordSlashSkill);

        #endregion
    }
}

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
           0,0,HpLevlSkillUpBonuses, null);

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
            0,0,StminaSkillUpBonuses, null);

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
            ,0, 0, 0, 0, 0, 0, 0,0, AttackSkillUpBonuses, null);

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
            , 0, 0, 0, 0, 0, 0, 0,0,0, DefenceSkillUpBonuses, null);

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
            0, 0, 0, 0, 0, 0, 0,0,0, HpAutoRestorationSkillUpBonuses, passiveHpUpSkill);

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
            , 0, 0, 0, 0, 0, 0, 0,0,0, StaminaAutoRestorationSkillUpBonuses, passiveStaminaUpSkill);

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
            0, 0, 0, 0, 0, 0, 0, 0,0,0, CrictleDamgeUpSkillUpBonuses, passiveAttackUpSkill);

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
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0, AttackDefenceUpSkillUoBonuses, passiveDefenceUpSkill);

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
            0, 0, 0, 0, 0, 0,0,0, HpStaminaUpSkillUpBonuses, passiveAutoHpSkillUp);

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
            , 0, 0, 0, 0, 0, 0,0,0, CrictleRateSkillUpBonuses, passiveCrictleDamgeSkillUp);

        PlayerManager.instance.player.skill.skillDataList.Add(passiveCrictleRateSkillUp);

        #endregion
    }

    void _ActiveSkillSetting()
    {
        #region 다운 어택

        #endregion

        #region 화염 속성 등록

        #endregion

        #region 버서커 공격력, 공격속도가 올라가는 대신 방어력 감소

        #endregion

        #region 실드

        #endregion

        #region 다운 어택 이동 범위 증가

        #endregion

        #region 다운 어택 공격 범위 증가

        #endregion

        #region 검기 슬래시

        #endregion

        #region 화상 데미지 증가

        #endregion

        #region 버서커 지속 시간 증가

        #endregion

        #region 실드 지속 시간 증가

        #endregion

        #region 검기 두번 나가는 확률 증가

        #endregion

        #region 화상 지속시간 증가

        #endregion
    }
}

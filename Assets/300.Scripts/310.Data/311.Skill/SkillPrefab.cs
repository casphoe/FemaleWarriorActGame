using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPrefab : MonoBehaviour
{
    [SerializeField] Image imgSkill;

    //0 : 레벨 다운, 1 : 레벨 업
    [SerializeField] Button[] btnLevel;

    //0 : 이름 ,1 : 레벨, 2: 스킬 습득의 대한 텍스트, 3 : 스킬 습득 포인트량
    [SerializeField] Text[] txtData;

    public SkillPoistion poistion;

    public SkillEquipPosition equipPoistion;

    Button btnSkillRegistration;

    int childIndex = 0;
    int levelUpIndex = 0;
    int levelDownIndex = 0;

    private void Awake()
    {
        childIndex = transform.GetSiblingIndex();

        btnLevel[0].onClick.AddListener(() => LevelBtnClick(0));
        btnLevel[1].onClick.AddListener(() => LevelBtnClick(1));
        btnSkillRegistration = imgSkill.GetComponent<Button>();
    }

    private void Start()
    {
        switch(GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtData[0].text = SkillSetting.instance.skillPanelList[childIndex].nameKor;
                txtData[3].text = "스킬 습득 포인트 :";
                break;
            case LANGUAGE.ENG:
                txtData[0].text = SkillSetting.instance.skillPanelList[childIndex].nameEng;
                txtData[3].text = "Skill Acquisition Points :";
                break;
        }

        switch (poistion)
        {
            case SkillPoistion.Active:
                equipPoistion = SkillSetting.instance.skillPanelList[childIndex].equipPostion;
                txtData[4].fontSize = 10;
                if (equipPoistion == SkillEquipPosition.NotRegistration)
                {
                    btnSkillRegistration.interactable = false;
                }
                else
                {
                    btnSkillRegistration.interactable = true;
                }
                break;
        }

        SkillDataSetting();
        SkillSetting.instance.skillPanelList[childIndex].level = PlayerManager.instance.player.skill.GetSkillLevelByName(SkillSetting.instance.skillPanelList[childIndex].nameKor);
        txtData[1].text = SkillSetting.instance.skillPanelList[childIndex].level.ToString();
        txtData[2].text = SkillSetting.instance.skillPanelList[childIndex].acquisitionPoints.ToString();

        if (SkillSetting.instance.skillPanelList[childIndex].level == SkillLevel.zero)
        {
            btnLevel[0].interactable = false;
        }
        else if(SkillSetting.instance.skillPanelList[childIndex].level == SkillLevel.five)
        {
            btnLevel[1].interactable = false;
        }
        else
        {
            for(int i = 0; i < btnLevel.Length; i++)
            {
                btnLevel[i].interactable = true;
            }
        }
    }

    void SkillDataSetting()
    {
        switch (poistion)
        {
            case SkillPoistion.Passive:
                SkillPanel.instance.SkillCountTxtSetting(0, PlayerManager.instance.player.skillCount);
                switch (childIndex)
                {
                    case 0:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "체렉증가 : " + SkillSetting.instance.skillPanelList[childIndex].HpUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "HpUp : " + SkillSetting.instance.skillPanelList[childIndex].HpUp.ToString();
                                break;
                        }
                        break;
                    case 1:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "스태미나증가 : " + SkillSetting.instance.skillPanelList[childIndex].StaminaUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "StaminaUp : " + SkillSetting.instance.skillPanelList[childIndex].StaminaUp.ToString();
                                break;
                        }
                        break;
                    case 2:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "공격력증가 : " + SkillSetting.instance.skillPanelList[childIndex].attackUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "AttackUp : " + SkillSetting.instance.skillPanelList[childIndex].attackUp.ToString();
                                break;
                        }
                        break;
                    case 3:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "방어력증가 : " + SkillSetting.instance.skillPanelList[childIndex].defenceUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "DenfenceUp : " + SkillSetting.instance.skillPanelList[childIndex].defenceUp.ToString();
                                break;
                        }
                        break;
                    case 4:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "자동체력회복증가 : " + SkillSetting.instance.skillPanelList[childIndex].HpRestoration.ToString() + "\n" + "체력회복쿨타임 : " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "AutoHpRestUp : " + SkillSetting.instance.skillPanelList[childIndex].HpRestoration.ToString() + "\n" + "HpRestCoolTime : " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString();
                                break;
                        }
                        break;
                    case 5:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "자동스태미나회복증가 : " + SkillSetting.instance.skillPanelList[childIndex].StaminaRestoration.ToString() + "\n" + "스태미나회복쿨타임 : " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "AutoStaminaRestUp : " + SkillSetting.instance.skillPanelList[childIndex].StaminaRestoration.ToString() + "\n" + "StaminaRestCoolTime : " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString();
                                break;
                        }
                        break;
                    case 6:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "크리티컬 데미지 증가 : " + SkillSetting.instance.skillPanelList[childIndex].crictleDmgUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "CrictleDamageUp : " + SkillSetting.instance.skillPanelList[childIndex].crictleDmgUp.ToString();
                                break;
                        }
                        break;
                    case 7:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "공격력증가 : " + SkillSetting.instance.skillPanelList[childIndex].attackUp.ToString() + "\n" + "방어력증가 : " + SkillSetting.instance.skillPanelList[childIndex].defenceUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "AttackUp : " + SkillSetting.instance.skillPanelList[childIndex].attackUp.ToString() + "\n" + "DefenceUp : " + SkillSetting.instance.skillPanelList[childIndex].defenceUp.ToString();
                                break;
                        }
                        break;
                    case 8:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "체력증가 : " + SkillSetting.instance.skillPanelList[childIndex].HpUp.ToString() + "\n" + "스태미나증가 : " + SkillSetting.instance.skillPanelList[childIndex].StaminaUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "HpUp : " + SkillSetting.instance.skillPanelList[childIndex].HpUp.ToString() + "\n" + "StaminaUp : " + SkillSetting.instance.skillPanelList[childIndex].StaminaUp.ToString();
                                break;
                        }
                        break;
                    case 9:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "크리티컬 확률 증가 : " + SkillSetting.instance.skillPanelList[childIndex].crictleRateUp.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "CrictleRateUp : " + SkillSetting.instance.skillPanelList[childIndex].crictleRateUp.ToString();
                                break;
                        }
                        break;
                }
                break;
            case SkillPoistion.Active:
                SkillPanel.instance.SkillCountTxtSetting(1, PlayerManager.instance.player.skillCount);
                switch (childIndex)
                {
                    case 0:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "데미지 " + SkillSetting.instance.skillPanelList[childIndex].damage.ToString() + "\n" + "공격범위 " + SkillSetting.instance.skillPanelList[childIndex].attackRange.ToString() + "\n" + "이동거리 " + SkillSetting.instance.skillPanelList[childIndex].attackMovePoint.ToString()
                                    + "\n" +  "쿨타임 " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "스태미나 소모 " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "damage " + SkillSetting.instance.skillPanelList[childIndex].damage.ToString() + "\n" + "attackRange " + SkillSetting.instance.skillPanelList[childIndex].attackRange.ToString() + "\n" + "attackMovePoint " + SkillSetting.instance.skillPanelList[childIndex].attackMovePoint.ToString()
                                    + "\n" + "coolTime " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "StaminaConsumption " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                        }
                        break;
                    case 1:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "버프지속시간 " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString() + "\n"
                                    + "\n" + "쿨타임 " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "스태미나 소모 " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "BuffTime " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString() + "\n"
                                    + "\n" + "CoolTime " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "StaminaConsumption " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                        }
                        break;
                    case 2:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "공격력증가 " + SkillSetting.instance.skillPanelList[childIndex].attackUp.ToString() + "\n" + "방어력증가 " + SkillSetting.instance.skillPanelList[childIndex].defenceUp.ToString() + "\n" + "버프지속시간 " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString()
                                    + "\n" + "쿨타임 " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "스태미나 소모 " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "AttackUp " + SkillSetting.instance.skillPanelList[childIndex].attackUp.ToString() + "\n" + "DefenceUp " + SkillSetting.instance.skillPanelList[childIndex].defenceUp.ToString() + "\n" + "BuffTime " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString()
                                    + "\n" + "coolTime " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "StaminaConsumption " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                        }
                        break;
                    case 3:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "버프지속시간 " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString() + "\n" + "데미지 흡수 " + SkillSetting.instance.skillPanelList[childIndex].DamageAbsorption.ToString() + "\n"
                                     + "쿨타임 " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "스태미나 소모 " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "BuffTime " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString() + "\n" + "DamageAbsorption " + SkillSetting.instance.skillPanelList[childIndex].DamageAbsorption.ToString() + "\n"
                                      + "coolTime " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "StaminaConsumption " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                        }
                        break;
                    case 4:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "이동거리 : " + SkillSetting.instance.skillPanelList[childIndex].attackMovePoint.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "attackMovePoint : " + SkillSetting.instance.skillPanelList[childIndex].attackMovePoint.ToString();
                                break;
                        }
                        break;
                    case 5:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "공격범위 : " + SkillSetting.instance.skillPanelList[childIndex].attackRange.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "attackRange : " + SkillSetting.instance.skillPanelList[childIndex].attackRange.ToString();
                                break;
                        }
                        break;
                    case 6:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "데미지 " + SkillSetting.instance.skillPanelList[childIndex].damage.ToString() + "\n" + "이동거리 " + SkillSetting.instance.skillPanelList[childIndex].attackMovePoint.ToString()
                                    + "\n" + "쿨타임 " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "스태미나 소모 " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "damage " + SkillSetting.instance.skillPanelList[childIndex].damage.ToString() + "\n" + "attackMovePoint " + SkillSetting.instance.skillPanelList[childIndex].attackMovePoint.ToString()
                                    + "\n" + "coolTime " + SkillSetting.instance.skillPanelList[childIndex].coolTime.ToString() + "\n" + "StaminaConsumption " + SkillSetting.instance.skillPanelList[childIndex].StaminaConsumption.ToString();
                                break;
                        }
                        break;
                    case 7:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "지속시간 : " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "BuffTime : " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString();
                                break;
                        }
                        break;
                    case 8:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "지속시간 : " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "BuffTime : " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString();
                                break;
                        }
                        break;
                    case 9:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "지속시간 : " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "BuffTime : " + SkillSetting.instance.skillPanelList[childIndex].buffTime.ToString();
                                break;
                        }
                        break;
                    case 10:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txtData[4].text = "스킬조건비율 : " + SkillSetting.instance.skillPanelList[childIndex].skillConditionsRate.ToString();
                                break;
                            case LANGUAGE.ENG:
                                txtData[4].text = "SkillConditionsRate : " + SkillSetting.instance.skillPanelList[childIndex].skillConditionsRate.ToString();
                                break;
                        }
                        break;
                }
                break;
        }
    }


    void LevelBtnClick(int num)
    {
        switch(num)
        {
            case 0:
                //레벨 다운
                levelDownIndex = PlayerManager.instance.player.skill.levelDownSkill(SkillSetting.instance.skillPanelList[childIndex]);
                switch(levelDownIndex)
                {                 
                    case 1: //선행 스킬 존재
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                SkillPanel.instance.SkillRequireTextSetting(true, SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameKor + "선행 스킬이 습득 되어 있습니다", SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameKor + "스킬 레벨을 0으로 만든 후 " + SkillSetting.instance.skillPanelList[childIndex].nameKor + " 의 스킬 레벨 다운 시켜주세요");
                                break;
                            case LANGUAGE.ENG:
                                SkillPanel.instance.SkillRequireTextSetting(true, SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameEng + "Prerequisite skills have been acquired", SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameEng + "After setting the skill level to 0 " + SkillSetting.instance.skillPanelList[childIndex].nameEng + " Please lower the skill level of");
                                break;
                        }
                        break;
                    case 2: //레벨 다운
                        SkillSetting.instance.skillPanelList[childIndex].level = PlayerManager.instance.player.skill.GetSkillLevelByName(SkillSetting.instance.skillPanelList[childIndex].nameKor);
                        txtData[1].text = SkillSetting.instance.skillPanelList[childIndex].level.ToString();
                        txtData[2].text = SkillSetting.instance.skillPanelList[childIndex].acquisitionPoints.ToString();
                        if (SkillSetting.instance.skillPanelList[childIndex].level == SkillLevel.zero)
                        {
                            btnLevel[0].interactable = false;
                        }
                        btnLevel[1].interactable = true;
                        break;
                }
                break;
            case 1:
                //레벨 업
                levelUpIndex = PlayerManager.instance.player.skill.LevelUpSkill(SkillSetting.instance.skillPanelList[childIndex]);
                switch(levelUpIndex)
                {
                    case 0: //체대레벨
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                SkillPanel.instance.SkillRequireTextSetting(true, "스킬 레벨이 최대치 입니다.", SkillSetting.instance.skillPanelList[childIndex].nameKor + " 스킬 레벨을 더 이상 상승시키지 못합니다.");
                                break;
                            case LANGUAGE.ENG:
                                SkillPanel.instance.SkillRequireTextSetting(true, "The skill level is at maximum.", SkillSetting.instance.skillPanelList[childIndex].nameEng + " The skill level can no longer be increased.");
                                break;
                        }
                        break;
                    case 1: //선행 스킬 습득 못함
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                SkillPanel.instance.SkillRequireTextSetting(true, SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameKor + "선행 스킬이 습득이 안되어있습니다.", SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameKor + "스킬을 습득 한 후 " + SkillSetting.instance.skillPanelList[childIndex].nameKor + " 의 스킬 습득을 진행해주세요");
                                break;
                            case LANGUAGE.ENG:
                                SkillPanel.instance.SkillRequireTextSetting(true, SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameEng + "Prerequisite skills have not been acquired.", SkillSetting.instance.skillPanelList[childIndex].requiredSkill.nameEng + "After acquiring the skill " + SkillSetting.instance.skillPanelList[childIndex].nameEng + " Please proceed with acquiring the skills.");
                                break;
                        }
                        break;
                    case 2: //스킬 포인트 부족
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                SkillPanel.instance.SkillRequireTextSetting(true, "스킬 포인트 부족", SkillSetting.instance.skillPanelList[childIndex].nameKor + " 스킬을 습득 하는 데 " + (SkillSetting.instance.skillPanelList[childIndex].acquisitionPoints - PlayerManager.instance.player.skillCount).ToString() + " 포인트가 부족 합니다.");
                                break;
                            case LANGUAGE.ENG:
                                SkillPanel.instance.SkillRequireTextSetting(true, "lack of skill points", SkillSetting.instance.skillPanelList[childIndex].nameEng + " To acquire skills " + (SkillSetting.instance.skillPanelList[childIndex].acquisitionPoints - PlayerManager.instance.player.skillCount).ToString() + " Not enough points.");
                                break;
                        }
                        break;
                    case 3: //스킬 레벨업
                        btnLevel[0].interactable = true;
                        SkillSetting.instance.skillPanelList[childIndex].level = PlayerManager.instance.player.skill.GetSkillLevelByName(SkillSetting.instance.skillPanelList[childIndex].nameKor);
                        txtData[1].text = SkillSetting.instance.skillPanelList[childIndex].level.ToString();
                        txtData[2].text = SkillSetting.instance.skillPanelList[childIndex].acquisitionPoints.ToString();
                        if (SkillSetting.instance.skillPanelList[childIndex].level == SkillLevel.five)
                        {
                            btnLevel[1].interactable = false;
                        }
                        switch(poistion)
                        {
                            case SkillPoistion.Passive:
                                PlayerManager.instance.PassivePlayerStatSkillSetting(0, SkillSetting.instance.skillPanelList[childIndex].HpUp, SkillSetting.instance.skillPanelList[childIndex].StaminaUp,
                                    SkillSetting.instance.skillPanelList[childIndex].attackUp, SkillSetting.instance.skillPanelList[childIndex].defenceUp, SkillSetting.instance.skillPanelList[childIndex].crictleRateUp, SkillSetting.instance.skillPanelList[childIndex].crictleDmgUp, SkillSetting.instance.skillPanelList[childIndex].HpRestoration, SkillSetting.instance.skillPanelList[childIndex].StaminaRestoration);
                                break;
                        }
                        break;
                }
                break;
        }
        SkillDataSetting();
    }
}

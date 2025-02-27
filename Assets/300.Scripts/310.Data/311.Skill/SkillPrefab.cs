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

    int childIndex = 0;
    int levelUpIndex = 0;
    int levelDownIndex = 0;

    private void Awake()
    {
        childIndex = transform.GetSiblingIndex();

        btnLevel[0].onClick.AddListener(() => LevelBtnClick(0));
        btnLevel[1].onClick.AddListener(() => LevelBtnClick(1));
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
                SkillPanel.instance.SkillCountTxtSetting(0, PlayerManager.instance.player.skillCount);
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
                    case 0: //미습득 상태
                        break;
                    case 1: //선행 스킬 존재
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
                        break;
                    case 1: //선행 스킬 습득 못함
                        break;
                    case 2: //스킬 포인트 부족
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
                        break;
                }
                break;
        }
        SkillDataSetting();
    }
}

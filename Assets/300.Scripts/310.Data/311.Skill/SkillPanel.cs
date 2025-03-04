using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SkillPanel : MonoBehaviour
{
    [SerializeField] GameObject skillPage;

    [SerializeField] GameObject skillRequirePanel;

    [SerializeField] Button[] btnSkill;

    [SerializeField] GameObject[] skillPageSetting;

    [SerializeField] SkillSetting skillSetting;

    [SerializeField] Button btnCanel;

    [SerializeField] Text[] txtSkillCount;

    [SerializeField] Text[] txtSkillRequre;

    int skillSelectNum = 0;
    int skillPageSelect = 0;

    private Coroutine currentSkillPageCorute;

    public float blinkDuration = 0.5f; //깜빡임 주기

    bool[] isSkillPageSelect = new bool[2];
    bool[] isSkillSelect;

    public static SkillPanel instance;

    private void Awake()
    {
        instance = this;
        btnSkill[0].onClick.AddListener(() => OnSkillPageSelectClickEvent(0));
        btnSkill[1].onClick.AddListener(() => OnSkillPageSelectClickEvent(1));
        btnCanel.onClick.AddListener(() => OnCancelClickEvent());
    }

    private void Start()
    {
        Utils.OnOff(skillPage, false);
        for(int i = 0; i < isSkillPageSelect.Length; i++)
        {
            isSkillPageSelect[i] = false;
            Utils.OnOff(skillPageSetting[i], false);
        }
        skillSetting._SkillSetting();
    }


    private void Update()
    {
        if (PlayerManager.instance.IsDead == false)
        {
            if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.Skill]))
            {
                PlayerManager.instance.isSkillPage = !PlayerManager.instance.isSkillPage;
                if(PlayerManager.instance.isSkillPage == true)
                {
                    Utils.OnOff(skillPage, true);
                    SkillRequireTextSetting(false, "", "");
                }
                else
                {
                    Utils.OnOff(skillPage, false);
                }
            }

            if (isSkillPageSelect[0] == false && isSkillPageSelect[1] == false)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (skillPageSelect > 0)
                    {
                        skillPageSelect -= 1;
                        OnSkillPageImageClickEvent(skillPageSelect);
                    }
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (skillPageSelect < btnSkill.Length - 1)
                    {
                        skillPageSelect += 1;
                        OnSkillPageImageClickEvent(skillPageSelect);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    StopAllBinking(btnSkill, 0);
                    isSkillPageSelect[skillPageSelect] = true;
                    for(int i = 0; i < skillPageSetting.Length; i++)
                    {
                        Utils.OnOff(skillPageSetting[i], false);
                    }
                    Utils.OnOff(skillPageSetting[skillPageSelect], true);
                    if (isSkillPageSelect[0] == true) //액티브
                    {
                        skillSetting._skillDataSetting(0);
                    }
                    else if (isSkillPageSelect[1] == true) //패시브
                    {
                        skillSetting._skillDataSetting(1);
                        isSkillSelect = new bool[skillSetting.skillPanelList.Count];
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (isSkillSelect.All(value => !value)) // 모든 값이 false인지 확인
                {
                    if (isSkillPageSelect[0] == true || isSkillPageSelect[1] == true)
                    {
                        OnSkillPageImageClickEvent(skillPageSelect);
                        for(int i = 0; i < isSkillPageSelect.Length; i++)
                        {
                            isSkillPageSelect[i] = false;
                        }

                        for (int i = 0; i < skillPageSetting.Length; i++)
                        {
                            Utils.OnOff(skillPageSetting[i], false);
                        }
                    }
                }
                else
                {

                }
            }
        }
    }

    void OnCancelClickEvent()
    {
        PlayerManager.instance.isSkillPage = false;
        Utils.OnOff(skillPage, false);
    }


    void OnSkillPageSelectClickEvent(int num)
    {
        skillPageSelect = num;

        for (int i = 0; i < isSkillPageSelect.Length; i++)
        {
            isSkillPageSelect[i] = false;
        }

        for (int i = 0; i < skillPageSetting.Length; i++)
        {
            Utils.OnOff(skillPageSetting[i], false);
        }
        Utils.OnOff(skillPageSetting[skillPageSelect], true);

        isSkillPageSelect[num] = true;
        skillSetting._skillDataSetting(num);
        switch (skillPageSelect)
        {
            case 0:
                
                break;
            case 1:
                
                break;
        }
    }

    public void SkillCountTxtSetting(int num,int data)
    {
        switch(GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtSkillCount[num].text = "스킬 포인트 : " + Utils.GetThousandCommaText(data);
                break;
            case LANGUAGE.ENG:
                txtSkillCount[num].text = "Skill Point : " + Utils.GetThousandCommaText(data);
                break;
        }
    }

    void OnSkillPageImageClickEvent(int num)
    {
        StopAllBinking(btnSkill, 0);

        StartBlinking(num, btnSkill, 0);       
    }

    private void StartBlinking(int index, Button[] btn, int num)
    {
        switch (num)
        {
            case 0:
                if (currentSkillPageCorute != null)
                {
                    StopCoroutine(currentSkillPageCorute);
                }
                currentSkillPageCorute = StartCoroutine(Blink(index, btn));
                break;          
        }
    }

    private IEnumerator Blink(int index, Button[] btn)
    {
        Image buttonImage = btn[index].GetComponent<Image>();
        Color originalColor = buttonImage.color;

        while (true)
        {
            buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            yield return new WaitForSeconds(blinkDuration);

            buttonImage.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    void StopAllBinking(Button[] btn, int num)
    {
        switch (num)
        {
            case 0:
                if (currentSkillPageCorute != null)
                {
                    StopCoroutine(currentSkillPageCorute);
                }
                break;
        }

        for (int i = 0; i < btn.Length; i++)
        {
            ResetButtonAlpha(btn[i]);
        }
    }

    private void ResetButtonAlpha(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Color originalColor = buttonImage.color;
        buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    public void SkillRequireTextSetting(bool isOn,string title, string desc)
    {
        Utils.OnOff(skillRequirePanel, isOn);
        txtSkillRequre[0].text = title;
        txtSkillRequre[1].text = desc;
        StartCoroutine(RequireOff(2));
    }

    IEnumerator RequireOff(float time)
    {
        yield return new WaitForSeconds(time);
        Utils.OnOff(skillRequirePanel, false);
    }
}

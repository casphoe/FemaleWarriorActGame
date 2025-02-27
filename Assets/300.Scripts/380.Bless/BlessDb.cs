using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BlessData
{
    Gold, Exp, AttackUp, DefenceUp
}

public class BlessDb : MonoBehaviour
{
    public BlessData data;

    //0 : 이름, 1 : 설명, 2 : 가격
    [SerializeField] Text[] txtData;


    private void Awake()
    {
        OnBlessData();
    }

    void OnBlessData()
    {
        switch(data)
        {
            case BlessData.Gold:
                switch(GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtData[0].text = "Gold Buff";
                        txtData[1].text = "Increases gold acquisition by 50% for 30 minutes.";
                        break;
                    case LANGUAGE.KOR:
                        txtData[0].text = "골드 버프";
                        txtData[1].text = "골드 획득량을 30분 동안 50% 올려줍니다.";
                        break;
                }
                switch(GameManager.data.diffucity)
                {
                    case Diffucity.Easy:
                        txtData[2].text = Utils.GetThousandCommaText(600);
                        break;
                    case Diffucity.Normal:
                        txtData[2].text = Utils.GetThousandCommaText(750);
                        break;
                    case Diffucity.Hard:
                        txtData[2].text = Utils.GetThousandCommaText(900);
                        break;
                }
                break;
            case BlessData.Exp:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtData[0].text = "Exp Buff";
                        txtData[1].text = "Increases experience gain by 50% for 30 minutes.";
                        break;
                    case LANGUAGE.KOR:
                        txtData[0].text = "경험치 버프";
                        txtData[1].text = "경험치 획득량을 30분 동안 50% 올려줍니다.";
                        break;
                }
                switch (GameManager.data.diffucity)
                {
                    case Diffucity.Easy:
                        txtData[2].text = Utils.GetThousandCommaText(800);
                        break;
                    case Diffucity.Normal:
                        txtData[2].text = Utils.GetThousandCommaText(1000);
                        break;
                    case Diffucity.Hard:
                        txtData[2].text = Utils.GetThousandCommaText(1200);
                        break;
                }
                break;
            case BlessData.AttackUp:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtData[0].text = "Attack Buff";
                        txtData[1].text = "Increases attack power by 50% for 30 minutes.";
                        break;
                    case LANGUAGE.KOR:
                        txtData[0].text = "공격력 버프";
                        txtData[1].text = "공격력을 30분 동안 50% 올려줍니다.";
                        break;
                }
                switch (GameManager.data.diffucity)
                {
                    case Diffucity.Easy:
                        txtData[2].text = Utils.GetThousandCommaText(1200);
                        break;
                    case Diffucity.Normal:
                        txtData[2].text = Utils.GetThousandCommaText(1800);
                        break;
                    case Diffucity.Hard:
                        txtData[2].text = Utils.GetThousandCommaText(2400);
                        break;
                }
                break;
            case BlessData.DefenceUp:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtData[0].text = "Defense Buff";
                        txtData[1].text = "Increases defense by 50% for 30 minutes.";
                        break;
                    case LANGUAGE.KOR:
                        txtData[0].text = "방어력 버프";
                        txtData[1].text = "방어력을 30분 동안 50% 올려줍니다.";
                        break;
                }
                switch (GameManager.data.diffucity)
                {
                    case Diffucity.Easy:
                        txtData[2].text = Utils.GetThousandCommaText(1200);
                        break;
                    case Diffucity.Normal:
                        txtData[2].text = Utils.GetThousandCommaText(1800);
                        break;
                    case Diffucity.Hard:
                        txtData[2].text = Utils.GetThousandCommaText(2400);
                        break;
                }
                break;
        }
    }
}

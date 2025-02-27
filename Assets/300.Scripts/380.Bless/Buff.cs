using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Buff : MonoBehaviour
{
    [SerializeField] Button[] btnBuff;

    [SerializeField] Button[] btnBlessShop;

    [SerializeField] GameObject[] purchaseObject;

    [SerializeField] Text txtBlessPurchaseData;

    [SerializeField]
    Text[] txtBlessPurchase;

    [SerializeField] int selectNum = -1;

    [SerializeField] GameObject panelBlessPurchase;

    [SerializeField] Text[] txtPurchase;

    [SerializeField] GameObject buffCharcterObject;

    private void Awake()
    {
        btnBuff[0].onClick.AddListener(() => OnBlessSelectClick(0));
        btnBuff[1].onClick.AddListener(() => OnBlessSelectClick(1));
        btnBuff[2].onClick.AddListener(() => OnBlessSelectClick(2));
        btnBuff[3].onClick.AddListener(() => OnBlessSelectClick(3));

        btnBlessShop[0].onClick.AddListener(() => OnBlessShopClick(0));
        btnBlessShop[1].onClick.AddListener(() => OnBlessShopClick(1));
        btnBlessShop[2].onClick.AddListener(() => OnBlessShopClick(2));
    }

    private void OnEnable()
    {
        OnBuffPurchaseData();
        selectNum = -1;
        for (int i = 1; i < 3; i++)
        {
            btnBlessShop[i].interactable = false;
        }
        txtBlessPurchaseData.text = "";
    }

    void OnBlessSelectClick(int num)
    {
        selectNum = num;
        for (int i = 1; i < 3; i++)
        {
            btnBlessShop[i].interactable = true;
        }     
        switch (selectNum)
        {
            case 0:
                switch(GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtBlessPurchaseData.text = "The purchase price of the gold acquisition buff is " + txtBlessPurchase[selectNum].text + " won. Do you want to purchase it?";
                        break;
                    case LANGUAGE.KOR:
                        txtBlessPurchaseData.text = "골드 획득량 버프 구매 가격은 " + txtBlessPurchase[selectNum].text + "원 입니다. 구매하시겠습니까?";
                        break;
                }
                break;
            case 1:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtBlessPurchaseData.text = "The purchase price of the experience gain buff is " + txtBlessPurchase[selectNum].text + " won. Do you want to purchase it?";
                        break;
                    case LANGUAGE.KOR:
                        txtBlessPurchaseData.text = "경험치 획득량 버프 구매 가격은 " + txtBlessPurchase[selectNum].text + "원 입니다. 구매하시겠습니까?";
                        break;
                }
                break;
            case 2:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtBlessPurchaseData.text = "The purchase price of the attack power increase buff is " + txtBlessPurchase[selectNum].text + " won. Do you want to purchase it?";
                        break;
                    case LANGUAGE.KOR:
                        txtBlessPurchaseData.text = "공격력 증가 버프 구매 가격은 " + txtBlessPurchase[selectNum].text + "원 입니다. 구매하시겠습니까?";
                        break;
                }
                break;
            case 3:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.ENG:
                        txtBlessPurchaseData.text = "The purchase price of the defense increase buff is " + txtBlessPurchase[selectNum].text + " won. Do you want to purchase it?";
                        break;
                    case LANGUAGE.KOR:
                        txtBlessPurchaseData.text = "방어력 증가 버프 구매 가격은 " + txtBlessPurchase[selectNum].text + "원 입니다. 구매하시겠습니까?";
                        break;
                }
                break;
        }
    }

    void OnBlessShopClick(int num)
    {
        switch(num)
        {
            case 0:
                Utils.OnOff(gameObject, false);
                break;
            case 1:
                if (PlayerManager.instance.player.money >= Utils.GetStringToInt(txtBlessPurchase[selectNum].text))
                {
                    PlayerManager.instance.player.money -= Utils.GetStringToInt(txtBlessPurchase[selectNum].text);
                    GameCanvas.instance.MoneySetting();
                    txtBlessPurchaseData.text = "";
                    PlayerManager.instance.player.isBuff[selectNum] = true;
                    PlayerManager.instance.player.buffRemainTime[selectNum] = 60 * 30;
                    OnBuffPurchaseData();
                    switch(GameManager.data.lanauge)
                    {
                        case LANGUAGE.KOR:
                            switch(selectNum)
                            {
                                case 0:
                                    StartCoroutine(blessPurchaseCorotue("축복 구매 성공", "골드 획득량 증가 버프 구매가 성공했습니다 버프 지속시간은 30분입니다."));
                                    break;
                                case 1:
                                    StartCoroutine(blessPurchaseCorotue("축복 구매 성공", "경험치 획득량 증가 버프 구매가 성공했습니다 버프 지속시간은 30분입니다."));
                                    break;
                                case 2:
                                    StartCoroutine(blessPurchaseCorotue("축복 구매 성공", "공격력 증가 버프 구매가 성공했습니다 버프 지속시간은 30분입니다."));
                                    break;
                                case 3:
                                    StartCoroutine(blessPurchaseCorotue("축복 구매 성공", "방어력 증가 버프 구매가 성공했습니다 버프 지속시간은 30분입니다."));
                                    break;
                            }
                            break;
                        case LANGUAGE.ENG:
                            switch (selectNum)
                            {
                                case 0:
                                    StartCoroutine(blessPurchaseCorotue("blessing purchase success", "The purchase of the gold acquisition increase buff was successful. The buff lasts for 30 minutes."));
                                    break;
                                case 1:
                                    StartCoroutine(blessPurchaseCorotue("blessing purchase success", "The purchase of the experience gain increase buff was successful. The buff lasts for 30 minutes."));
                                    break;
                                case 2:
                                    StartCoroutine(blessPurchaseCorotue("blessing purchase success", "The purchase of the attack power increase buff was successful. The buff lasts for 30 minutes."));
                                    break;
                                case 3:
                                    StartCoroutine(blessPurchaseCorotue("blessing purchase success", "The purchase of the defense increase buff was successful. The buff lasts for 30 minutes."));
                                    break;
                            }
                            break;
                    }
                    OnCharcterPurchaseBuffOn(selectNum);
                }
                else
                {
                    switch (GameManager.data.lanauge)
                    {
                        case LANGUAGE.KOR:
                            StartCoroutine(blessPurchaseCorotue("축복 구매 실패", "골드가 " + (Utils.GetStringToInt(txtBlessPurchase[selectNum].text) - PlayerManager.instance.player.money) + " 원이 부족합니다." ));
                            break;
                        case LANGUAGE.ENG:
                            StartCoroutine(blessPurchaseCorotue("Blessing Purchase Failed", (Utils.GetStringToInt(txtBlessPurchase[selectNum].text) - PlayerManager.instance.player.money) + " won short of gold"));
                            break;
                    }
                }            
                break;
            case 2:
                txtBlessPurchaseData.text = "";
                break;
        }
    }

    void OnCharcterPurchaseBuffOn(int num)
    {
        Utils.OnOff(buffCharcterObject.transform.GetChild(num).gameObject, true);
    }

    IEnumerator blessPurchaseCorotue(string tilte, string desc)
    {
        Utils.OnOff(panelBlessPurchase, true);
        txtPurchase[0].text = tilte;
        txtPurchase[1].text = desc;
        yield return new WaitForSeconds(2);
        Utils.OnOff(panelBlessPurchase, false);
    }

    void OnBuffPurchaseData()
    {
        if (PlayerManager.instance.player.isBuff[0] == true)
        {
            btnBuff[0].interactable = false;
            Utils.OnOff(purchaseObject[0], true);
        }
        else
        {
            btnBuff[0].interactable = true;
            Utils.OnOff(purchaseObject[0], false);
        }

        if (PlayerManager.instance.player.isBuff[1] == true)
        {
            btnBuff[1].interactable = false;
            Utils.OnOff(purchaseObject[1], true);
        }
        else
        {
            btnBuff[1].interactable = true;
            Utils.OnOff(purchaseObject[1], false);
        }

        if (PlayerManager.instance.player.isBuff[2] == true)
        {
            btnBuff[2].interactable = false;
            Utils.OnOff(purchaseObject[2], true);
        }
        else
        {
            btnBuff[2].interactable = true;
            Utils.OnOff(purchaseObject[2], false);
        }

        if (PlayerManager.instance.player.isBuff[3] == true)
        {
            btnBuff[3].interactable = false;
            Utils.OnOff(purchaseObject[3], true);
        }
        else
        {
            btnBuff[3].interactable = true;
            Utils.OnOff(purchaseObject[3], false);
        }
    }
}

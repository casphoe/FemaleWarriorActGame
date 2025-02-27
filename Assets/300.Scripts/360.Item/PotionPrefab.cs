using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class PotionPrefabData : InfiniteScrollData
{
    public int index = 0;
    public int number = 0;
}

public class PotionPrefab : InfiniteScrollItem
{
    public int listIndex = 0;

    public Image img;
    //0: 이름 , 1 : 설명,2 : 구매가격, 3 : 구매 개수, 4 : 총금액
    public Text[] txt;

    //0 : down, 1 : up
    public Button[] btnCount;

    //0 : 구매, 1 : 판매
    public int potionShopNum = -1;

    private void Awake()
    {
        btnCount[0].onClick.AddListener(() => OnCountClick(0));
        btnCount[1].onClick.AddListener(() => OnCountClick(1));
    }

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        PotionPrefabData itemData = (PotionPrefabData)scrollData;

        listIndex = itemData.index;       
        switch (potionShopNum)
        {
            case 0: //구매
                img.sprite = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].itemSpr;
                PotionHaveCountSetting();
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txt[0].text = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].nameKor;
                        txt[1].text = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].descriptionKor;
                        break;
                    case LANGUAGE.ENG:
                        txt[0].text = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].name;
                        txt[1].text = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].description;
                        break;
                }
                txt[2].text = Utils.GetThousandCommaText(PotionPurchaseList.instance.itemPurchasePotionList[listIndex].purchase);
                txt[3].text = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count.ToString();
                txt[4].text = Utils.GetThousandCommaText(PotionPurchaseList.instance.itemPurchasePotionList[listIndex].totalData);
                if (PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount >= PlayerManager.instance.player.maxHpPotionCount)
                {
                    btnCount[1].interactable = false;
                }
                else
                {
                    btnCount[1].interactable = true;
                }

                if(PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count > 0)
                {
                    btnCount[0].interactable = true;
                }
                else
                {
                    btnCount[0].interactable = false;
                }
                break;
            case 1: //판매
                img.sprite = PotionPurchaseList.instance.itemSellPotionList[listIndex].itemSpr;
                PotionSellHaveCountSetting();
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txt[0].text = PotionPurchaseList.instance.itemSellPotionList[listIndex].nameKor;
                        txt[1].text = PotionPurchaseList.instance.itemSellPotionList[listIndex].descriptionKor;
                        break;
                    case LANGUAGE.ENG:
                        txt[0].text = PotionPurchaseList.instance.itemSellPotionList[listIndex].name;
                        txt[1].text = PotionPurchaseList.instance.itemSellPotionList[listIndex].description;
                        break;
                }
                txt[2].text = Utils.GetThousandCommaText(PotionPurchaseList.instance.itemSellPotionList[listIndex].purchase);
                txt[3].text = PotionPurchaseList.instance.itemSellPotionList[listIndex].Count.ToString();
                txt[4].text = Utils.GetThousandCommaText(PotionPurchaseList.instance.itemSellPotionList[listIndex].totalData);
                if (PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount > 0 && PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount > PotionPurchaseList.instance.itemSellPotionList[listIndex].Count)
                {
                    btnCount[1].interactable = true;
                }
                else
                {
                    btnCount[1].interactable = false;
                }

                if (PotionPurchaseList.instance.itemSellPotionList[listIndex].Count > 0)
                {
                    btnCount[0].interactable = true;
                }
                else
                {
                    btnCount[0].interactable = false;
                }
                break;
        }       
    }   

    void PotionHaveCountSetting()
    {
        switch(listIndex)
        {
            case 0:
                PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("체력포션");
                break;
            case 1:
                PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("중간체력포션");
                break;
            case 2:
                PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("상급체력포션");
                break;
            case 3:
                PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("스태미나포션");
                break;
            case 4:
                PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("중간스태미나포션");
                break;
            case 5:
                PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("상급스태미나포션");
                break;
        }
    }

    void PotionSellHaveCountSetting()
    {
        switch (listIndex)
        {
            case 0:
                PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("체력포션");
                break;
            case 1:
                PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("중간체력포션");
                break;
            case 2:
                PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("상급체력포션");
                break;
            case 3:
                PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("스태미나포션");
                break;
            case 4:
                PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("중간스태미나포션");
                break;
            case 5:
                PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("상급스태미나포션");
                break;
        }
    }

    void OnCountClick(int num)
    {
        switch (potionShopNum)
        {
            case 0:
                switch (num)
                {
                    //구매
                    case 0:
                        if (PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count > 1)
                        {
                            btnCount[1].interactable = true;
                            PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count -= 1;
                            PotionPurchaseList.instance.itemPurchasePotionList[listIndex].totalData = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].purchase * PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                            PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count = 0;
                            PotionPurchaseList.instance.itemPurchasePotionList[listIndex].totalData = 0;
                        }
                        break;
                    case 1:
                        if ((PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count + PotionPurchaseList.instance.itemPurchasePotionList[listIndex].haveCount) < PlayerManager.instance.player.maxHpPotionCount)
                        {
                            btnCount[0].interactable = true;
                            PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count += 1;
                        }
                        else
                        {
                            btnCount[1].interactable = false;
                        }
                        PotionPurchaseList.instance.itemPurchasePotionList[listIndex].totalData = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].purchase * PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count;
                        break;
                }
                PotionPurchaseList.instance.purchaseTotal = PotionPurchaseList.instance.itemPurchasePotionList[0].totalData + PotionPurchaseList.instance.itemPurchasePotionList[1].totalData + PotionPurchaseList.instance.itemPurchasePotionList[2].totalData + PotionPurchaseList.instance.itemPurchasePotionList[3].totalData + PotionPurchaseList.instance.itemPurchasePotionList[4].totalData + PotionPurchaseList.instance.itemPurchasePotionList[5].totalData;
                PotionPurchaseList.instance.PurchaseTotalDataSetting();
                txt[3].text = PotionPurchaseList.instance.itemPurchasePotionList[listIndex].Count.ToString();
                txt[4].text = Utils.GetThousandCommaText(PotionPurchaseList.instance.itemPurchasePotionList[listIndex].totalData);
                break;
                //판매
            case 1:
                 switch(num)
                {
                    case 0:
                        if (PotionPurchaseList.instance.itemSellPotionList[listIndex].Count > 1 && PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount > 1)
                        {
                            btnCount[1].interactable = true;
                            PotionPurchaseList.instance.itemSellPotionList[listIndex].Count -= 1;
                            PotionPurchaseList.instance.itemSellPotionList[listIndex].totalData = PotionPurchaseList.instance.itemSellPotionList[listIndex].purchase * PotionPurchaseList.instance.itemSellPotionList[listIndex].Count;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                            PotionPurchaseList.instance.itemSellPotionList[listIndex].Count = 0;
                            PotionPurchaseList.instance.itemSellPotionList[listIndex].totalData = 0;
                        }
                        break;
                    case 1:
                        if (PotionPurchaseList.instance.itemSellPotionList[listIndex].Count < PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount && PotionPurchaseList.instance.itemSellPotionList[listIndex].haveCount > 0)
                        {
                            btnCount[0].interactable = true;
                            PotionPurchaseList.instance.itemSellPotionList[listIndex].Count += 1;
                        }
                        else
                        {
                            btnCount[1].interactable = false;
                        }
                        PotionPurchaseList.instance.itemSellPotionList[listIndex].totalData = PotionPurchaseList.instance.itemSellPotionList[listIndex].purchase * PotionPurchaseList.instance.itemSellPotionList[listIndex].Count;
                        break;
                }
                PotionPurchaseList.instance.sellTotal = PotionPurchaseList.instance.itemSellPotionList[0].totalData + PotionPurchaseList.instance.itemSellPotionList[1].totalData + PotionPurchaseList.instance.itemSellPotionList[2].totalData + PotionPurchaseList.instance.itemSellPotionList[3].totalData + PotionPurchaseList.instance.itemSellPotionList[4].totalData + PotionPurchaseList.instance.itemSellPotionList[5].totalData;
                PotionPurchaseList.instance.SellTotalDataSetting();
                txt[3].text = PotionPurchaseList.instance.itemSellPotionList[listIndex].Count.ToString();
                txt[4].text = Utils.GetThousandCommaText(PotionPurchaseList.instance.itemSellPotionList[listIndex].totalData);             
                break;
        }
    }
}

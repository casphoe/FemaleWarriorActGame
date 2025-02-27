using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionPurchaseList : MonoBehaviour
{
    public static PotionPurchaseList instance;

    public int indexNumber = -1;
    public int selectNumber = -1;

    [SerializeField] PotionList dataList;
    [SerializeField] PotionSellPopup dataSellList;

    public List<ItemData> itemPurchasePotionList = new List<ItemData>();

    public List<ItemData> itemSellPotionList = new List<ItemData>();

    public Sprite[] potionSpr;

    public int purchaseTotal;
    public int sellTotal;

    [SerializeField] Text txtPurchaseTotal;
    [SerializeField] Text txtSellTotal;

    [SerializeField] Button[] btnPurcahse;

    [SerializeField] Button[] btnSell;

    private void Awake()
    {
        instance = this;
        purchaseTotal = 0;
        sellTotal = 0;
        btnPurcahse[0].onClick.AddListener(() => OnBuyClickEvent(0));
        btnPurcahse[1].onClick.AddListener(() => OnBuyClickEvent(1));

        btnSell[0].onClick.AddListener(() => OnSellClickEvent(0));
        btnSell[1].onClick.AddListener(() => OnSellClickEvent(1));
    }

    public void PurchaseTotalDataSetting()
    {
        switch(GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtPurchaseTotal.text = "총 구매 금액은 :  " + Utils.GetThousandCommaText(purchaseTotal) + " 원 입니다.";
                break;
            case LANGUAGE.ENG:
                txtPurchaseTotal.text = "total purchase amount :  " + Utils.GetThousandCommaText(purchaseTotal) + " is won.";
                break;
        }
    }

    public void SellTotalDataSetting()
    {
        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtSellTotal.text = "총 판매 금액은 :  " + Utils.GetThousandCommaText(sellTotal) + " 원 입니다.";
                break;
            case LANGUAGE.ENG:
                txtSellTotal.text = "total sell amount :  " + Utils.GetThousandCommaText(sellTotal) + " is won.";
                break;
        }
    }

    void OnBuyClickEvent(int num)
    {
        switch(num)
        {
            case 0:
                if(PlayerManager.instance.player.money >= purchaseTotal)
                {
                    //구매완료
                    for(int i = 0; i < itemPurchasePotionList.Count; i++)
                    {
                        if (itemPurchasePotionList[i].Count > 0)
                        {
                            switch (i)
                            {
                                case 0:
                                    PlayerManager.instance.player.inventory.AddNewItemToInventory("LessHpPotion", "체력포션", ItemDb.HpPotion, DataDb.None, 0.15f, 0, 0, 0, 0, 0, 0, 0, 0, itemPurchasePotionList[0].Count, 0, 0, 0, 0, itemPurchasePotionList[0].purchase);
                                    break;
                                case 1:
                                    PlayerManager.instance.player.inventory.AddNewItemToInventory("MeduimHpPotion", "중간체력포션", ItemDb.HpPotion, DataDb.None, 0.30f, 0, 0, 0, 0, 0, 0, 0, 0, itemPurchasePotionList[1].Count, 0, 0, 0, 0, itemPurchasePotionList[1].purchase);
                                    break;
                                case 2:
                                    PlayerManager.instance.player.inventory.AddNewItemToInventory("HighHpPotion", "상급체력포션", ItemDb.HpPotion, DataDb.None, 0.70f, 0, 0, 0, 0, 0, 0, 0, 0, itemPurchasePotionList[2].Count, 0, 0, 0, 0, itemPurchasePotionList[2].purchase);
                                    break;
                                case 3:
                                    PlayerManager.instance.player.inventory.AddNewItemToInventory("LessStaminaPotion", "스태미나포션", ItemDb.StaminaPotion, DataDb.None, 0, 0.15f, 0, 0, 0, 0, 0, 0, 0, itemPurchasePotionList[3].Count, 0, 0, 0, 0, itemPurchasePotionList[3].purchase);
                                    break;
                                case 4:
                                    PlayerManager.instance.player.inventory.AddNewItemToInventory("MeduimStaminaPotion", "중간스태미나포션", ItemDb.StaminaPotion, DataDb.None, 0, 0.30f, 0, 0, 0, 0, 0, 0, 0, itemPurchasePotionList[4].Count, 0, 0, 0, 0, itemPurchasePotionList[4].purchase);
                                    break;
                                case 5:
                                    PlayerManager.instance.player.inventory.AddNewItemToInventory("HighStaminaPotion", "상급스태미나포션", ItemDb.StaminaPotion, DataDb.None, 0, 0.70f, 0, 0, 0, 0, 0, 0, 0, itemPurchasePotionList[5].Count, 0, 0, 0, 0, itemPurchasePotionList[5].purchase);
                                    break;
                            }
                        }
                    }
                    switch (GameManager.data.lanauge)
                    {
                        case LANGUAGE.KOR:
                            ShopPanel.instance.ShopTextSetting("구매 성공", "총 " + Utils.GetThousandCommaText(purchaseTotal) + " 원이 결제 되었습니다.");
                            break;
                        case LANGUAGE.ENG:
                            ShopPanel.instance.ShopTextSetting("Purchase Success", "A total of " + Utils.GetThousandCommaText(purchaseTotal) + " won was paid.");
                            break;
                    }
                    PlayerManager.instance.player.money -= purchaseTotal;
                    GameCanvas.instance.MoneySetting();
                    GameCanvas.instance.PotionSetting();
                    purchaseTotal = 0;
                    PurchaseTotalDataSetting();
                    for (int i = 0; i < itemPurchasePotionList.Count; i++)
                    {
                        itemPurchasePotionList[i].Count = 0;
                        itemPurchasePotionList[i].totalData = 0;
                    }
                    dataList.PurchaseLoadList();
                }
                else
                {
                    //돈이 부족하다는 글이 나와야함
                    switch(GameManager.data.lanauge)
                    {
                        case LANGUAGE.KOR:
                            ShopPanel.instance.ShopTextSetting("구매 실패", "돈이 " + Utils.GetThousandCommaText(purchaseTotal - PlayerManager.instance.player.money) + "원 부족합니다.");
                            break;
                        case LANGUAGE.ENG:
                            ShopPanel.instance.ShopTextSetting("Purchase Failed", "I'm " + Utils.GetThousandCommaText(purchaseTotal - PlayerManager.instance.player.money) + " won short");
                            break;
                    }
                }
                break;
            case 1: // 구매 취소
                purchaseTotal = 0;
                PurchaseTotalDataSetting();
                for(int i = 0; i < itemPurchasePotionList.Count; i++)
                {
                    itemPurchasePotionList[i].Count = 0;
                    itemPurchasePotionList[i].totalData = 0;
                }
                dataList.PurchaseLoadList();
                break;
        }
    }

    public void OnPurchaseData()
    {
        var PurchaseContents = new ItemData[]
        {
            new ItemData()
            {
                db = ItemDb.HpPotion,
                name = "LessHpPotion",
                nameKor = "체력포션",
                description = "Restores 15% of current health.",
                descriptionKor = "현재 체력에서 15% 만큼 회복합니다.",
                purchase = 150,
                hpRestoration = 0.15f,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[0],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.HpPotion,
                name = "MeduimHpPotion",
                nameKor = "중간체력포션",
                description = "Restores 30% of current health.",
                descriptionKor = "현재 체력에서 30% 만큼 회복합니다.",
                purchase = 400,
                hpRestoration = 0.30f,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                 itemSpr = potionSpr[1],
                 Count = 0,
                 totalData = 0,
                 itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.HpPotion,
                name = "HighHpPotion",
                nameKor = "상급체력포션",
                description = "Restores 70% of current health.",
                descriptionKor = "현재 체력에서 70% 만큼 회복합니다.",
                purchase = 1000,
                hpRestoration = 0.70f,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[2],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.StaminaPotion,
                name = "LessStaminaPotion",
                nameKor = "스태미나포션",
                description = "Restores 15% of current stamina.",
                descriptionKor = "현재 스태미나에서 15% 만큼 회복합니다.",
                purchase = 130,
                hpRestoration = 0,
                stminaRestoration = 0.15f,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[3],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.StaminaPotion,
                name = "MeduimStaminaPotion",
                nameKor = "중간스태미나포션",
                description = "Restores 30% of current stamina.",
                descriptionKor = "현재 스태미나에서 30% 만큼 회복합니다.",
                purchase = 300,
                hpRestoration = 0,
                stminaRestoration = 0.30f,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[4],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.StaminaPotion,
                name = "HighStaminaPotion",
                nameKor = "상급스태미나포션",
                description = "Restores 70% of current stamina.",
                descriptionKor = "현재 스태미나에서 70% 만큼 회복합니다.",
                purchase = 700,
                hpRestoration = 0,
                stminaRestoration = 0.70f,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[5],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
        };
        if(itemPurchasePotionList.Count == 0)
        {
            itemPurchasePotionList.AddRange(PurchaseContents);
        }
        dataList.PurchaseLoadList();
    }

    void OnSellClickEvent(int num)
    {
        switch (num)
        {
            //판매완료
            case 0:
                for (int i = 0; i < itemSellPotionList.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            PlayerManager.instance.player.inventory.RemoveItemByName("체력포션", itemSellPotionList[0].Count);
                            break;
                        case 1:
                            PlayerManager.instance.player.inventory.RemoveItemByName("중간체력포션", itemSellPotionList[1].Count);
                            break;
                        case 2:
                            PlayerManager.instance.player.inventory.RemoveItemByName("상급체력포션", itemSellPotionList[2].Count);
                            break;
                        case 3:
                            PlayerManager.instance.player.inventory.RemoveItemByName("스태미나포션", itemSellPotionList[3].Count);
                            break;
                        case 4:
                            PlayerManager.instance.player.inventory.RemoveItemByName("중간스태미나포션", itemSellPotionList[4].Count);
                            break;
                        case 5:
                            PlayerManager.instance.player.inventory.RemoveItemByName("상급스태미나포션", itemSellPotionList[5].Count);
                            break;
                    }
                }

                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        ShopPanel.instance.ShopTextSetting("판매 성공", "총 " + Utils.GetThousandCommaText(sellTotal) + " 원이 판매 되었습니다.");
                        break;
                    case LANGUAGE.ENG:
                        ShopPanel.instance.ShopTextSetting("Sales Success", "A total of " + Utils.GetThousandCommaText(sellTotal) + " won was sold.");
                        break;
                }

                PlayerManager.instance.player.money += sellTotal;
                GameCanvas.instance.MoneySetting();
                GameCanvas.instance.PotionSetting();
                sellTotal = 0;
                SellTotalDataSetting();
                for (int i = 0; i < itemSellPotionList.Count; i++)
                {
                    itemSellPotionList[i].Count = 0;
                    itemSellPotionList[i].totalData = 0;
                }
                dataSellList.SellLoadList();
                break;
            case 1: //판매 취소
                sellTotal = 0;
                SellTotalDataSetting();
                for (int i = 0; i < itemSellPotionList.Count; i++)
                {
                    itemSellPotionList[i].Count = 0;
                    itemSellPotionList[i].totalData = 0;
                }
                dataSellList.SellLoadList();
                break;
        }
    }

    public void OnSellData()
    {
        var SellContent = new ItemData[]
        {
             new ItemData()
            {
                db = ItemDb.HpPotion,
                name = "LessHpPotion",
                nameKor = "체력포션",
                description = "Restores 15% of current health.",
                descriptionKor = "현재 체력에서 15% 만큼 회복합니다.",
                purchase = 75,
                hpRestoration = 0.15f,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[0],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
              new ItemData()
            {
                db = ItemDb.HpPotion,
                name = "MeduimHpPotion",
                nameKor = "중간체력포션",
                description = "Restores 30% of current health.",
                descriptionKor = "현재 체력에서 30% 만큼 회복합니다.",
                purchase = 200,
                hpRestoration = 0.30f,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                 itemSpr = potionSpr[1],
                 Count = 0,
                 totalData = 0,
                 itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.HpPotion,
                name = "HighHpPotion",
                nameKor = "상급체력포션",
                description = "Restores 70% of current health.",
                descriptionKor = "현재 체력에서 70% 만큼 회복합니다.",
                purchase = 500,
                hpRestoration = 0.70f,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[2],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.StaminaPotion,
                name = "LessStaminaPotion",
                nameKor = "스태미나포션",
                description = "Restores 15% of current stamina.",
                descriptionKor = "현재 스태미나에서 15% 만큼 회복합니다.",
                purchase = 65,
                hpRestoration = 0,
                stminaRestoration = 0.15f,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[3],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.StaminaPotion,
                name = "MeduimStaminaPotion",
                nameKor = "중간스태미나포션",
                description = "Restores 30% of current stamina.",
                descriptionKor = "현재 스태미나에서 30% 만큼 회복합니다.",
                purchase = 150,
                hpRestoration = 0,
                stminaRestoration = 0.30f,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[4],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
            new ItemData()
            {
                db = ItemDb.StaminaPotion,
                name = "HighStaminaPotion",
                nameKor = "상급스태미나포션",
                description = "Restores 70% of current stamina.",
                descriptionKor = "현재 스태미나에서 70% 만큼 회복합니다.",
                purchase = 350,
                hpRestoration = 0,
                stminaRestoration = 0.70f,
                defenceUp = 0,
                attackUp  = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = potionSpr[5],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 0
            },
        };
        if(itemSellPotionList.Count == 0)
        {
            itemSellPotionList.AddRange(SellContent);
        }
        dataSellList.SellLoadList();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class itemPrefabData : InfiniteScrollData
{
    public int index = 0;
    public int number = 0;
}

public class itemPrefab : InfiniteScrollItem
{
    public int listIndex = 0;

    public Button[] btnCount;
    //0: 가격,1 : 구매 카운트, 2 : 총가격
    public Text[] txt;
    public Image img;

    public Image btnImage;

    //0 : 구매 , 1 : 판매
    public int itemShopNum = -1;

    string basedWeaponName = string.Empty;
    string basedArmorName = string.Empty;
    string basedAccelyName = string.Empty;

    int weaponBaseCost;
    int weaponSellPurchae;

    int armorBaseCost;
    int armorSellPurchase;

    int accelyBaseCost;
    int accelySellPurchase;

    Sprite itemWeaponSpr;
    Sprite itemArmorSpr;
    Sprite itemAccelySpr;

    private void Awake()
    {
        btnCount[0].onClick.AddListener(() => OnCountClick(0));
        btnCount[1].onClick.AddListener(() => OnCountClick(1));
    }

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        itemPrefabData itemData = (itemPrefabData)scrollData;

        listIndex = itemData.index;

        switch(itemShopNum)
        {
            case 0: //구매
                switch(PlayerManager.instance.itemShopNum)
                {
                    case 0: //무기                       
                        img.sprite = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].itemSpr;
                        switch(GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txt[0].text = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].nameKor;
                                break;
                            case LANGUAGE.ENG:
                                txt[0].text = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].name;
                                break;
                        }
                        txt[1].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].purchase);
                        txt[2].text = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count.ToString();
                        txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].totalData);

                        if(EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].haveCount > 10)
                        {
                            btnCount[1].interactable = false;
                        }
                        else
                        {
                            btnCount[1].interactable = true;
                        }


                        if (EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count > 0)
                        {
                            btnCount[0].interactable = true;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                        }
                        break;
                    case 1: //방어구
                        img.sprite = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].itemSpr;
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txt[0].text = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].nameKor;
                                break;
                            case LANGUAGE.ENG:
                                txt[0].text = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].name;
                                break;
                        }
                        txt[1].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorPurchaseList[listIndex].purchase);
                        txt[2].text = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count.ToString();
                        txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorPurchaseList[listIndex].totalData);

                        if (EquipmentList.instance.equipmentArmorPurchaseList[listIndex].haveCount > 10)
                        {
                            btnCount[1].interactable = false;
                        }
                        else
                        {
                            btnCount[1].interactable = true;
                        }


                        if (EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count > 0)
                        {
                            btnCount[0].interactable = true;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                        }
                        break;
                    case 2: //액세서리
                        img.sprite = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].itemSpr;
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txt[0].text = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].nameKor;
                                break;
                            case LANGUAGE.ENG:
                                txt[0].text = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].name;
                                break;
                        }
                        txt[1].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].purchase);
                        txt[2].text = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count.ToString();
                        txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].totalData);

                        if (EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].haveCount > 10)
                        {
                            btnCount[1].interactable = false;
                        }
                        else
                        {
                            btnCount[1].interactable = true;
                        }


                        if (EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count > 0)
                        {
                            btnCount[0].interactable = true;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                        }
                        break;
                }
                if (EquipmentList.instance.purchaseIndexNumber == listIndex)
                {
                    Utils.ImageColorChange(btnImage, new Color(200 / 255f, 200 / 255f, 200 / 255f, 1));
                }
                else
                {
                    Utils.ImageColorChange(btnImage, new Color(1, 1, 1, 0));
                }
                break;
            case 1: //판매
                switch(PlayerManager.instance.itemShopNum)
                {
                    case 0:                      
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txt[0].text = EquipmentList.instance.equipmentWeaponSellList[listIndex].nameKor;
                                break;
                            case LANGUAGE.ENG:
                                txt[0].text = EquipmentList.instance.equipmentWeaponSellList[listIndex].name;
                                break;
                        }
                        EquipmentList.instance.equipmentWeaponSellList[listIndex].UpdateEquippCount();
                        weaponBaseCost = EquipmentList.instance.equipmentWeaponSellList[listIndex].purchase / 2;
                        weaponSellPurchae = weaponBaseCost + (int)(weaponBaseCost * 0.2f * EquipmentList.instance.equipmentWeaponSellList[listIndex].itemEnforce);
                        txt[1].text = Utils.GetThousandCommaText(weaponSellPurchae);
                        txt[2].text = EquipmentList.instance.equipmentWeaponSellList[listIndex].Count.ToString();
                        EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData = weaponSellPurchae * EquipmentList.instance.equipmentWeaponSellList[listIndex].Count;
                        txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData);
                        basedWeaponName = GetBaseName(EquipmentList.instance.equipmentWeaponSellList[listIndex].nameKor);
                        GetWeaponSpriteBasedOnName(basedWeaponName);
                        img.sprite = itemWeaponSpr;
                        if (EquipmentList.instance.equipmentWeaponSellList[listIndex].itemEnforce > 0)
                        {
                            Utils.OnOff(txt[4].gameObject, true);
                        }
                        else
                        {
                            Utils.OnOff(txt[4].gameObject, false);
                        }

                        txt[4].text = EquipmentList.instance.equipmentWeaponSellList[listIndex].itemEnforce.ToString();

                        if (EquipmentList.instance.equipmentWeaponSellList[listIndex].haveCount > 0 && EquipmentList.instance.equipmentWeaponSellList[listIndex].haveCount > EquipmentList.instance.equipmentWeaponSellList[listIndex].Count && EquipmentList.instance.equipmentWeaponSellList[listIndex].unequippedCount > EquipmentList.instance.equipmentWeaponSellList[listIndex].Count)
                        {
                            btnCount[1].interactable = true;
                        }
                        else
                        {
                            btnCount[1].interactable = false;
                        }


                        if (EquipmentList.instance.equipmentWeaponSellList[listIndex].Count > 0)
                        {
                            btnCount[0].interactable = true;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                        }
                        break;
                    case 1:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txt[0].text = EquipmentList.instance.equipmentArmorSellList[listIndex].nameKor;
                                break;
                            case LANGUAGE.ENG:
                                txt[0].text = EquipmentList.instance.equipmentArmorSellList[listIndex].name;
                                break;
                        }
                        EquipmentList.instance.equipmentArmorSellList[listIndex].UpdateEquippCount();
                        armorBaseCost = EquipmentList.instance.equipmentArmorSellList[listIndex].purchase / 2;
                        armorSellPurchase = armorBaseCost + (int)(armorBaseCost * 0.2f * EquipmentList.instance.equipmentArmorSellList[listIndex].itemEnforce);
                        EquipmentList.instance.equipmentArmorSellList[listIndex].totalData = armorSellPurchase * EquipmentList.instance.equipmentArmorSellList[listIndex].Count;
                        txt[1].text = Utils.GetThousandCommaText(armorSellPurchase);
                        txt[2].text = EquipmentList.instance.equipmentArmorSellList[listIndex].Count.ToString();
                        txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorSellList[listIndex].totalData);
                        basedArmorName = GetBaseName(EquipmentList.instance.equipmentArmorSellList[listIndex].nameKor);
                        GetArmorSpirteBasedOnName(basedArmorName);
                        img.sprite = itemArmorSpr;

                        if (EquipmentList.instance.equipmentArmorSellList[listIndex].itemEnforce > 0)
                        {
                            Utils.OnOff(txt[4].gameObject, true);
                        }
                        else
                        {
                            Utils.OnOff(txt[4].gameObject, false);
                        }

                        txt[4].text = EquipmentList.instance.equipmentArmorSellList[listIndex].itemEnforce.ToString();

                        if (EquipmentList.instance.equipmentArmorSellList[listIndex].haveCount > 0 && EquipmentList.instance.equipmentArmorSellList[listIndex].haveCount > EquipmentList.instance.equipmentArmorSellList[listIndex].Count && EquipmentList.instance.equipmentArmorSellList[listIndex].unequippedCount > EquipmentList.instance.equipmentArmorSellList[listIndex].Count)
                        {
                            btnCount[1].interactable = true;
                        }
                        else
                        {
                            btnCount[1].interactable = false;
                        }


                        if (EquipmentList.instance.equipmentArmorSellList[listIndex].Count > 0)
                        {
                            btnCount[0].interactable = true;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                        }
                        break;
                    case 2:
                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                txt[0].text = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].nameKor;
                                break;
                            case LANGUAGE.ENG:
                                txt[0].text = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].name;
                                break;
                        }
                        EquipmentList.instance.equipmentAccessoriesSellList[listIndex].UpdateEquippCount();
                        accelyBaseCost = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].purchase / 2;
                        accelySellPurchase = accelyBaseCost + (int)(accelyBaseCost * 0.2f * EquipmentList.instance.equipmentAccessoriesSellList[listIndex].itemEnforce);
                        EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData = armorSellPurchase * EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count;
                        txt[1].text = Utils.GetThousandCommaText(accelySellPurchase);
                        txt[2].text = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count.ToString();
                        txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData);
                        basedAccelyName = GetBaseName(EquipmentList.instance.equipmentAccessoriesSellList[listIndex].nameKor);
                        GetAccelySpriteBasedOnName(basedAccelyName);
                        img.sprite = itemAccelySpr;

                        if (EquipmentList.instance.equipmentAccessoriesSellList[listIndex].itemEnforce > 0)
                        {
                            Utils.OnOff(txt[4].gameObject, true);
                        }
                        else
                        {
                            Utils.OnOff(txt[4].gameObject, false);
                        }

                        txt[4].text = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].itemEnforce.ToString();

                        if (EquipmentList.instance.equipmentAccessoriesSellList[listIndex].haveCount > 0 && EquipmentList.instance.equipmentAccessoriesSellList[listIndex].haveCount > EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count && EquipmentList.instance.equipmentAccessoriesSellList[listIndex].unequippedCount > EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count)
                        {
                            btnCount[1].interactable = true;
                        }
                        else
                        {
                            btnCount[1].interactable = false;
                        }


                        if (EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count > 0)
                        {
                            btnCount[0].interactable = true;
                        }
                        else
                        {
                            btnCount[0].interactable = false;
                        }
                        break;
                }
                break;
        }
    }

    void OnCountClick(int num)
    {
        switch(itemShopNum) // 0 : 구매, 1 : 판매
        {
            case 0: //구매
                switch(num)
                {
                    case 0: //0: 다운
                        switch(PlayerManager.instance.itemShopNum)
                        {
                            //무기
                            case 0:
                                if (EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count > 1)
                                {
                                    btnCount[1].interactable = true;
                                    EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count -= 1;
                                }
                                else
                                {
                                    btnCount[0].interactable = false;
                                    EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count = 0;
                                    EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].totalData = 0;
                                }
                                EquipmentList.instance.weaponPurchaseTotal = 0;
                                EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].totalData = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].purchase * EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentWeaponPurchaseList.Count; i++)
                                {
                                    EquipmentList.instance.weaponPurchaseTotal += EquipmentList.instance.equipmentWeaponPurchaseList[i].totalData;
                                }
                                EquipmentList.instance.PurchaseTotalDataSetting(0);
                                txt[2].text = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].totalData);
                                break;
                            //방어구
                            case 1:
                                if (EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count > 1)
                                {
                                    btnCount[1].interactable = true;
                                    EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count -= 1;
                                }
                                else
                                {
                                    btnCount[0].interactable = false;
                                    EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count = 0;
                                    EquipmentList.instance.equipmentArmorPurchaseList[listIndex].totalData = 0;
                                }
                                EquipmentList.instance.armorPurchaseTotal = 0;
                                EquipmentList.instance.equipmentArmorPurchaseList[listIndex].totalData = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].purchase * EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentArmorPurchaseList.Count; i++)
                                {
                                    EquipmentList.instance.armorPurchaseTotal += EquipmentList.instance.equipmentArmorPurchaseList[i].totalData;
                                }
                                EquipmentList.instance.PurchaseTotalDataSetting(1);
                                txt[2].text = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorPurchaseList[listIndex].totalData);
                                break;
                            //액세서리
                            case 2:
                                if (EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count > 1)
                                {
                                    btnCount[1].interactable = true;
                                    EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count -= 1;
                                }
                                else
                                {
                                    btnCount[0].interactable = false;
                                    EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count = 0;
                                    EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].totalData = 0;
                                }
                                EquipmentList.instance.accessoriesPurchaseTotal = 0;
                                EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].totalData = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].purchase * EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentAccessoriesPurchaseList.Count; i++)
                                {
                                    EquipmentList.instance.accessoriesPurchaseTotal += EquipmentList.instance.equipmentAccessoriesPurchaseList[i].totalData;
                                }
                                EquipmentList.instance.PurchaseTotalDataSetting(2);
                                txt[2].text = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].totalData);
                                break;
                        }
                        break;
                    case 1: //Up
                        switch (PlayerManager.instance.itemShopNum)
                        {
                            //무기
                            case 0:
                                if((EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count + EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].haveCount) < 10)
                                {
                                    btnCount[0].interactable = true;
                                    EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count += 1;

                                }
                                else
                                {
                                    btnCount[1].interactable = false;
                                }
                                EquipmentList.instance.weaponPurchaseTotal = 0;
                                EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].totalData = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].purchase * EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentWeaponPurchaseList.Count; i++)
                                {
                                    EquipmentList.instance.weaponPurchaseTotal += EquipmentList.instance.equipmentWeaponPurchaseList[i].totalData;
                                }
                                EquipmentList.instance.PurchaseTotalDataSetting(0);
                                txt[2].text = EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponPurchaseList[listIndex].totalData);
                                break;
                            //방어구
                            case 1:
                                if ((EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count + EquipmentList.instance.equipmentArmorPurchaseList[listIndex].haveCount) < 10)
                                {
                                    btnCount[0].interactable = true;
                                    EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count += 1;

                                }
                                else
                                {
                                    btnCount[1].interactable = false;
                                }
                                EquipmentList.instance.armorPurchaseTotal = 0;
                                EquipmentList.instance.equipmentArmorPurchaseList[listIndex].totalData = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].purchase * EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentArmorPurchaseList.Count; i++)
                                {
                                    EquipmentList.instance.armorPurchaseTotal += EquipmentList.instance.equipmentArmorPurchaseList[i].totalData;
                                }
                                EquipmentList.instance.PurchaseTotalDataSetting(1);
                                txt[2].text = EquipmentList.instance.equipmentArmorPurchaseList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorPurchaseList[listIndex].totalData);
                                break;
                            //액세서리
                            case 2:
                                if ((EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count + EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].haveCount) < 6)
                                {
                                    btnCount[0].interactable = true;
                                    EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count += 1;

                                }
                                else
                                {
                                    btnCount[1].interactable = false;
                                }
                                EquipmentList.instance.accessoriesPurchaseTotal = 0;
                                EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].totalData = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].purchase * EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentAccessoriesPurchaseList.Count; i++)
                                {
                                    EquipmentList.instance.accessoriesPurchaseTotal += EquipmentList.instance.equipmentAccessoriesPurchaseList[i].totalData;
                                }
                                EquipmentList.instance.PurchaseTotalDataSetting(2);
                                txt[2].text = EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesPurchaseList[listIndex].totalData);
                                break;
                        }
                        break;
                }
                break;
            case 1: //판매
                switch(num)
                {
                    case 0: //다운
                        switch (PlayerManager.instance.itemShopNum)
                        {
                            case 0: //무기
                                EquipmentList.instance.equipmentWeaponSellList[listIndex].UpdateEquippCount();
                                if (EquipmentList.instance.equipmentWeaponSellList[listIndex].Count > 1 && EquipmentList.instance.equipmentWeaponSellList[listIndex].haveCount > 1)
                                {
                                    btnCount[1].interactable = true;
                                    EquipmentList.instance.equipmentWeaponSellList[listIndex].Count -= 1;
                                }
                                else
                                {
                                    btnCount[0].interactable = false;
                                    EquipmentList.instance.equipmentWeaponSellList[listIndex].Count = 0;
                                    EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData = 0;
                                }

                                EquipmentList.instance.weaponSellTotal = 0;

                                weaponBaseCost = EquipmentList.instance.equipmentWeaponSellList[listIndex].purchase / 2;
                                weaponSellPurchae = weaponBaseCost + (int)(weaponBaseCost * 0.2f * EquipmentList.instance.equipmentWeaponSellList[listIndex].itemEnforce);

                                EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData = weaponSellPurchae * EquipmentList.instance.equipmentWeaponSellList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentWeaponSellList.Count; i++)
                                {
                                    EquipmentList.instance.weaponSellTotal += EquipmentList.instance.equipmentWeaponSellList[i].totalData;
                                }
                                EquipmentList.instance.SellTotalDataSetting(0);
                                txt[2].text = EquipmentList.instance.equipmentWeaponSellList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData);
                                break;
                            case 1: //방어구
                                EquipmentList.instance.equipmentArmorSellList[listIndex].UpdateEquippCount();
                                if (EquipmentList.instance.equipmentArmorSellList[listIndex].Count > 1 && EquipmentList.instance.equipmentArmorSellList[listIndex].haveCount > 1)
                                {
                                    btnCount[1].interactable = true;
                                    EquipmentList.instance.equipmentArmorSellList[listIndex].Count -= 1;
                                }
                                else
                                {
                                    btnCount[0].interactable = false;
                                    EquipmentList.instance.equipmentArmorSellList[listIndex].Count = 0;
                                    EquipmentList.instance.equipmentArmorSellList[listIndex].totalData = 0;
                                }

                                EquipmentList.instance.armorSellTotal = 0;

                                armorBaseCost = EquipmentList.instance.equipmentArmorSellList[listIndex].purchase / 2;
                                armorSellPurchase = armorBaseCost + (int)(armorBaseCost * 0.2f * EquipmentList.instance.equipmentArmorSellList[listIndex].itemEnforce);

                                EquipmentList.instance.equipmentArmorSellList[listIndex].totalData = armorSellPurchase * EquipmentList.instance.equipmentArmorSellList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentArmorSellList.Count; i++)
                                {
                                    EquipmentList.instance.armorSellTotal += EquipmentList.instance.equipmentArmorSellList[i].totalData;
                                }
                                EquipmentList.instance.SellTotalDataSetting(1);
                                txt[2].text = EquipmentList.instance.equipmentArmorSellList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorSellList[listIndex].totalData);
                                break;
                            case 2: //액세서리
                                EquipmentList.instance.equipmentAccessoriesSellList[listIndex].UpdateEquippCount();
                                if (EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count > 1 && EquipmentList.instance.equipmentAccessoriesSellList[listIndex].haveCount > 1)
                                {
                                    btnCount[1].interactable = true;
                                    EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count -= 1;
                                }
                                else
                                {
                                    btnCount[0].interactable = false;
                                    EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count = 0;
                                    EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData = 0;
                                }

                                EquipmentList.instance.accessoriesSellTotal = 0;

                                accelyBaseCost = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].purchase / 2;
                                accelySellPurchase = accelyBaseCost + (int)(accelyBaseCost * 0.2f * EquipmentList.instance.equipmentAccessoriesSellList[listIndex].itemEnforce);

                                EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData = accelySellPurchase * EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentAccessoriesSellList.Count; i++)
                                {
                                    EquipmentList.instance.accessoriesSellTotal += EquipmentList.instance.equipmentAccessoriesSellList[i].totalData;
                                }
                                EquipmentList.instance.SellTotalDataSetting(2);
                                txt[2].text = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData);
                                break;
                        }
                        break;
                    case 1: //Up
                        switch (PlayerManager.instance.itemShopNum)
                        {
                            case 0:
                                EquipmentList.instance.equipmentWeaponSellList[listIndex].UpdateEquippCount();
                                if (EquipmentList.instance.equipmentWeaponSellList[listIndex].Count < EquipmentList.instance.equipmentWeaponSellList[listIndex].haveCount && EquipmentList.instance.equipmentWeaponSellList[listIndex].Count < EquipmentList.instance.equipmentWeaponSellList[listIndex].unequippedCount && EquipmentList.instance.equipmentWeaponSellList[listIndex].haveCount > 0 && EquipmentList.instance.equipmentWeaponSellList[listIndex].unequippedCount > 0)
                                {
                                    btnCount[0].interactable = true;
                                    EquipmentList.instance.equipmentWeaponSellList[listIndex].Count += 1;
                                    if(EquipmentList.instance.equipmentWeaponSellList[listIndex].Count == EquipmentList.instance.equipmentWeaponSellList[listIndex].haveCount)
                                    {
                                        btnCount[1].interactable = false;
                                    }
                                }
                                else
                                {
                                    btnCount[1].interactable = false;
                                }
                                EquipmentList.instance.weaponSellTotal = 0;
                                weaponBaseCost = EquipmentList.instance.equipmentWeaponSellList[listIndex].purchase / 2;
                                weaponSellPurchae = weaponBaseCost + (int)(weaponBaseCost * 0.2f * EquipmentList.instance.equipmentWeaponSellList[listIndex].itemEnforce);
                                EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData = weaponSellPurchae * EquipmentList.instance.equipmentWeaponSellList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentWeaponSellList.Count; i++)
                                {
                                    EquipmentList.instance.weaponSellTotal += EquipmentList.instance.equipmentWeaponSellList[i].totalData;
                                }
                                EquipmentList.instance.SellTotalDataSetting(0);
                                txt[2].text = EquipmentList.instance.equipmentWeaponSellList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentWeaponSellList[listIndex].totalData);
                                break;
                            case 1:
                                EquipmentList.instance.equipmentArmorSellList[listIndex].UpdateEquippCount();
                                if (EquipmentList.instance.equipmentArmorSellList[listIndex].Count < EquipmentList.instance.equipmentArmorSellList[listIndex].haveCount && EquipmentList.instance.equipmentArmorSellList[listIndex].Count < EquipmentList.instance.equipmentArmorSellList[listIndex].unequippedCount && EquipmentList.instance.equipmentArmorSellList[listIndex].haveCount > 0 && EquipmentList.instance.equipmentArmorSellList[listIndex].unequippedCount > 0)
                                {
                                    btnCount[0].interactable = true;
                                    EquipmentList.instance.equipmentArmorSellList[listIndex].Count += 1;
                                    if (EquipmentList.instance.equipmentArmorSellList[listIndex].Count == EquipmentList.instance.equipmentArmorSellList[listIndex].haveCount)
                                    {
                                        btnCount[1].interactable = false;
                                    }
                                }
                                else
                                {
                                    btnCount[1].interactable = false;
                                }
                                EquipmentList.instance.armorSellTotal = 0;
                                armorBaseCost = EquipmentList.instance.equipmentArmorSellList[listIndex].purchase / 2;
                                armorSellPurchase = armorBaseCost + (int)(armorBaseCost * 0.2f * EquipmentList.instance.equipmentArmorSellList[listIndex].itemEnforce);
                                EquipmentList.instance.equipmentArmorSellList[listIndex].totalData = armorSellPurchase * EquipmentList.instance.equipmentArmorSellList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentArmorSellList.Count; i++)
                                {
                                    EquipmentList.instance.armorSellTotal += EquipmentList.instance.equipmentArmorSellList[i].totalData;
                                }
                                EquipmentList.instance.SellTotalDataSetting(1);
                                txt[2].text = EquipmentList.instance.equipmentArmorSellList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentArmorSellList[listIndex].totalData);
                                break;
                            case 2:
                                EquipmentList.instance.equipmentAccessoriesSellList[listIndex].UpdateEquippCount();
                                if (EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count < EquipmentList.instance.equipmentAccessoriesSellList[listIndex].haveCount && EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count < EquipmentList.instance.equipmentAccessoriesSellList[listIndex].unequippedCount && EquipmentList.instance.equipmentAccessoriesSellList[listIndex].haveCount > 0 && EquipmentList.instance.equipmentAccessoriesSellList[listIndex].unequippedCount > 0)
                                {
                                    btnCount[0].interactable = true;
                                    EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count += 1;
                                    if (EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count == EquipmentList.instance.equipmentAccessoriesSellList[listIndex].haveCount)
                                    {
                                        btnCount[1].interactable = false;
                                    }
                                }
                                else
                                {
                                    btnCount[1].interactable = false;
                                }
                                EquipmentList.instance.accessoriesSellTotal = 0;
                                accelyBaseCost = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].purchase / 2;
                                accelySellPurchase = accelyBaseCost + (int)(accelyBaseCost * 0.2f * EquipmentList.instance.equipmentAccessoriesSellList[listIndex].itemEnforce);
                                EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData = accelySellPurchase * EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count;
                                for (int i = 0; i < EquipmentList.instance.equipmentAccessoriesSellList.Count; i++)
                                {
                                    EquipmentList.instance.accessoriesSellTotal += EquipmentList.instance.equipmentAccessoriesSellList[i].totalData;
                                }
                                EquipmentList.instance.SellTotalDataSetting(2);
                                txt[2].text = EquipmentList.instance.equipmentAccessoriesSellList[listIndex].Count.ToString();
                                txt[3].text = Utils.GetThousandCommaText(EquipmentList.instance.equipmentAccessoriesSellList[listIndex].totalData);
                                break;
                        }
                        break;
                }
                break;
        }
    }

    // 강화 단계 제거한 기본 이름 반환
    private string GetBaseName(string itemName)
    {
        int parenIndex = itemName.IndexOf('(');
        return parenIndex > -1 ? itemName.Substring(0, parenIndex) : itemName;
    }

    private Sprite GetWeaponSpriteBasedOnName(string itemName)
    {
        switch (itemName)
        {
            case "부려진검":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[0].itemSpr;
                break;
            case "녹슨대거":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[1].itemSpr;
                break;
            case "대거":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[2].itemSpr;
                break;
            case "철대거":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[3].itemSpr;
                break;
            case "시미터":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[4].itemSpr;
                break;
            case "철검":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[5].itemSpr;
                break;
            case "강철검":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[6].itemSpr;
                break;
            case "기사검":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[7].itemSpr;
                break;
            case "플랑베르주":
                itemWeaponSpr = EquipmentList.instance.equipmentWeaponPurchaseList[8].itemSpr;
                break;
        }

        return itemWeaponSpr;
    }

    private Sprite GetArmorSpirteBasedOnName(string itemName)
    {
        switch (itemName)
        {
            case "천옷":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[0].itemSpr;
                break;
            case "천망토":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[1].itemSpr;
                break;
            case "천헬멧":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[2].itemSpr;
                break;
            case "천장갑":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[3].itemSpr;
                break;
            case "천신발":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[4].itemSpr;
                break;
            case "천바지":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[5].itemSpr;
                break;
            case "가죽옷":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[6].itemSpr;
                break;
            case "가죽망토":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[7].itemSpr;
                break;
            case "가죽헬멧":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[8].itemSpr;
                break;
            case "가죽장갑":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[9].itemSpr;
                break;
            case "가죽신발":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[10].itemSpr;
                break;
            case "가죽바지":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[11].itemSpr;
                break;
            case "철갑옷":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[12].itemSpr;
                break;
            case "철망토":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[13].itemSpr;
                break;
            case "철헬멧":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[14].itemSpr;
                break;
            case "철장갑":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[15].itemSpr;
                break;
            case "철부츠":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[16].itemSpr;
                break;
            case "철바지":
                itemArmorSpr = EquipmentList.instance.equipmentArmorPurchaseList[17].itemSpr;
                break;
        }
        return itemArmorSpr;
    }

    private Sprite GetAccelySpriteBasedOnName(string itemName)
    {
        switch (itemName)
        {
            case "체력증가반지":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[0].itemSpr;
                break;
            case "스태미나증가반지":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[1].itemSpr;
                break;
            case "공격력증가반지":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[2].itemSpr;
                break;
            case "크리티컬증가반지":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[3].itemSpr;
                break;
            case "경험치증가반지":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[4].itemSpr;
                break;
            case "방어력증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[5].itemSpr;
                break;
            case "공방증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[6].itemSpr;
                break;
            case "골드량증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[7].itemSpr;
                break;
            case "운증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[8].itemSpr;
                break;
            case "경험치골드량증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[9].itemSpr;
                break;
        }

        return itemAccelySpr;
    }

    public void OnClick()
    {
        switch(itemShopNum)
        {
            case 0:
                OnSelect();
                break;
        }      
    }
}

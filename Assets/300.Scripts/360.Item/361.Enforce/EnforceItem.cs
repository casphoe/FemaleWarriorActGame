using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class EnforceItemData : InfiniteScrollData
{
    public int index = 0;
    public int number = 0;
}

public class EnforceItem : InfiniteScrollItem
{
    public int listIndex = 0;

    public Image img;

    public Image btnImage;

    string basedWeaponName = string.Empty;
    string basedArmorName = string.Empty;
    string basedAccelyName = string.Empty;

    Sprite itemWeaponSpr;
    Sprite itemArmorSpr;
    Sprite itemAccelySpr;

    // 0 : 이름, 1 : 개수, 2: 강화 단계 , 3: 강화 비용
    public Text[] txt;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        EnforceItemData enforceData = (EnforceItemData)scrollData;

        listIndex = enforceData.index;

        switch(PlayerManager.instance.itemShopNum)
        {
            case 0:
                switch(GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txt[0].text = EnforceList.instance.enforceWeaponItems[listIndex].nameKor;
                        txt[1].text = "개수 : " + EnforceList.instance.enforceWeaponItems[listIndex].haveCount.ToString();
                        txt[2].text = "강화 : " + EnforceList.instance.enforceWeaponItems[listIndex].itemEnforce.ToString();
                        txt[3].text = "강화 비용 : " + Utils.GetThousandCommaText(Mathf.CeilToInt(EnforceList.instance.enforceWeaponItems[listIndex].purchase * (1 + (0.2f * EnforceList.instance.enforceWeaponItems[listIndex].itemEnforce))));
                        break;
                    case LANGUAGE.ENG:
                        txt[0].text = EnforceList.instance.enforceWeaponItems[listIndex].name;
                        txt[1].text = "Count : " + EnforceList.instance.enforceWeaponItems[listIndex].haveCount.ToString();
                        txt[2].text = "Enhance : " + EnforceList.instance.enforceWeaponItems[listIndex].itemEnforce.ToString();
                        txt[3].text = "Enhance Cost : " + Utils.GetThousandCommaText(Mathf.CeilToInt(EnforceList.instance.enforceWeaponItems[listIndex].purchase * (1 + (0.2f * EnforceList.instance.enforceWeaponItems[listIndex].itemEnforce))));
                        break;
                }
                basedWeaponName = GetBaseName(EnforceList.instance.enforceWeaponItems[listIndex].nameKor);
                GetWeaponSpriteBasedOnName(basedWeaponName);
                img.sprite = itemWeaponSpr;
                break;
            case 1:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txt[0].text = EnforceList.instance.enforceArmorItems[listIndex].nameKor;
                        txt[1].text = "개수 : " + EnforceList.instance.enforceArmorItems[listIndex].haveCount.ToString();
                        txt[2].text = "강화 : " + EnforceList.instance.enforceArmorItems[listIndex].itemEnforce.ToString();
                        txt[3].text = "강화 비용 : " + Utils.GetThousandCommaText(Mathf.CeilToInt(EnforceList.instance.enforceArmorItems[listIndex].purchase * (1 + (0.2f * EnforceList.instance.enforceArmorItems[listIndex].itemEnforce))));
                        break;
                    case LANGUAGE.ENG:
                        txt[0].text = EnforceList.instance.enforceArmorItems[listIndex].name;
                        txt[1].text = "Count : " + EnforceList.instance.enforceArmorItems[listIndex].haveCount.ToString();
                        txt[2].text = "Enhance : " + EnforceList.instance.enforceArmorItems[listIndex].itemEnforce.ToString();
                        txt[3].text = "Enhance Cost : " + Utils.GetThousandCommaText(Mathf.CeilToInt(EnforceList.instance.enforceArmorItems[listIndex].purchase * (1 + (0.2f * EnforceList.instance.enforceArmorItems[listIndex].itemEnforce))));
                        break;
                }
                basedArmorName = GetBaseName(EnforceList.instance.enforceArmorItems[listIndex].nameKor);
                GetArmorSpirteBasedOnName(basedArmorName);
                img.sprite = itemArmorSpr;
                break;
            case 2:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txt[0].text = EnforceList.instance.enforceAccelyItems[listIndex].nameKor;
                        txt[1].text = "개수 : " + EnforceList.instance.enforceAccelyItems[listIndex].haveCount.ToString();
                        txt[2].text = "강화 : " + EnforceList.instance.enforceAccelyItems[listIndex].itemEnforce.ToString();
                        txt[3].text = "강화 비용 : " + Utils.GetThousandCommaText(Mathf.CeilToInt(EnforceList.instance.enforceAccelyItems[listIndex].purchase * (1 + (0.2f * EnforceList.instance.enforceAccelyItems[listIndex].itemEnforce))));
                        break;
                    case LANGUAGE.ENG:
                        txt[0].text = EnforceList.instance.enforceAccelyItems[listIndex].name;
                        txt[1].text = "Count : " + EnforceList.instance.enforceAccelyItems[listIndex].haveCount.ToString();
                        txt[2].text = "Enhance : " + EnforceList.instance.enforceAccelyItems[listIndex].itemEnforce.ToString();
                        txt[3].text = "Enhance Cost : " + Utils.GetThousandCommaText(Mathf.CeilToInt(EnforceList.instance.enforceAccelyItems[listIndex].purchase * (1 + (0.2f * EnforceList.instance.enforceAccelyItems[listIndex].itemEnforce))));
                        break;
                }
                basedAccelyName = GetBaseName(EnforceList.instance.enforceAccelyItems[listIndex].nameKor);
                GetAccelySpriteBasedOnName(basedAccelyName);
                img.sprite = itemAccelySpr;
                break;
        }

        if(EnforceList.instance.selectIndex == listIndex)
        {
            Utils.ImageColorChange(btnImage, new Color(200 / 255f, 200 / 255f, 200 / 255f, 1));
        }
        else
        {
            Utils.ImageColorChange(btnImage, new Color(1, 1, 1, 0));
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
        switch(itemName)
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
        switch(itemName)
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
        switch(itemName)
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
            case "방어력증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[5].itemSpr;
                break;
            case "공방증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[6].itemSpr;
                break;
            case "운증가목걸이":
                itemAccelySpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[8].itemSpr;
                break;
        }

        return itemAccelySpr;
    }

    public void OnClick()
    {
        OnSelect();
    }
}

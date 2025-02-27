using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class EquipItemData : InfiniteScrollData
{
    public int index = 0;
    public int number = 0;
}

public class EquipItem : InfiniteScrollItem
{
    public Text[] txt;

    public Image img;

    private string nameStr = string.Empty;

    private Sprite itemSpr;

    public int listIndex = 0;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        EquipItemData itemData = (EquipItemData)scrollData;

        listIndex = itemData.index;

        if (EquipmentPanel.instance.equipItemList[listIndex].nameKor == "장착해제")
        {
            Utils.OnOff(img.gameObject, false);

            switch(GameManager.data.lanauge)
            {
                case LANGUAGE.KOR:
                    txt[0].text = "장착해제";
                    break;
                case LANGUAGE.ENG:
                    txt[0].text = "Unequipped";
                    break;
            }
            Utils.OnOff(txt[1].gameObject, false);
        }
        else
        {
            Utils.OnOff(img.gameObject, true);
            Utils.OnOff(txt[1].gameObject, true);
            switch (GameManager.data.lanauge)
            {
                case LANGUAGE.KOR:
                    txt[0].text = EquipmentPanel.instance.equipItemList[listIndex].nameKor;
                    
                    break;
                case LANGUAGE.ENG:
                    txt[0].text = EquipmentPanel.instance.equipItemList[listIndex].name;                   
                    break;
            }
            txt[1].text = EquipmentPanel.instance.equipItemList[listIndex].unequippedCount.ToString();
            nameStr = GetBaseName(EquipmentPanel.instance.equipItemList[listIndex].nameKor);
            img.sprite = GetItemSpriteBasedOnName(nameStr);
        }
    }

    private string GetBaseName(string itemName)
    {
        int parenIndex = itemName.IndexOf('(');
        return parenIndex > -1 ? itemName.Substring(0, parenIndex) : itemName;
    }

    private Sprite GetItemSpriteBasedOnName(string itemName)
    {
        switch (nameStr)
        {
            case "체력포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[0];
                break;
            case "중간체력포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[1];
                break;
            case "상급체력포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[2];
                break;
            case "스태미나포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[3];
                break;
            case "중간스태미나포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[4];
                break;
            case "상급스태미나포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[5];
                break;
            case "부려진검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[0];
                break;
            case "녹슨대거":
                itemSpr = InventoryPanel.instance.weaponSpirteList[1];
                break;
            case "대거":
                itemSpr = InventoryPanel.instance.weaponSpirteList[2];
                break;
            case "철대거":
                itemSpr = InventoryPanel.instance.weaponSpirteList[3];
                break;
            case "시미터":
                itemSpr = InventoryPanel.instance.weaponSpirteList[4];
                break;
            case "철검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[5];
                break;
            case "강철검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[6];
                break;
            case "기사검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[7];
                break;
            case "플랑베르주":
                itemSpr = InventoryPanel.instance.weaponSpirteList[8];
                break;
            case "천옷":
                itemSpr = InventoryPanel.instance.aromrSpriteList[0];
                break;
            case "천망토":
                itemSpr = InventoryPanel.instance.aromrSpriteList[1];
                break;
            case "천헬멧":
                itemSpr = InventoryPanel.instance.aromrSpriteList[2];
                break;
            case "천장갑":
                itemSpr = InventoryPanel.instance.aromrSpriteList[3];
                break;
            case "천신발":
                itemSpr = InventoryPanel.instance.aromrSpriteList[4];
                break;
            case "천바지":
                itemSpr = InventoryPanel.instance.aromrSpriteList[5];
                break;
            case "가죽옷":
                itemSpr = InventoryPanel.instance.aromrSpriteList[6];
                break;
            case "가죽망토":
                itemSpr = InventoryPanel.instance.aromrSpriteList[7];
                break;
            case "가죽헬멧":
                itemSpr = InventoryPanel.instance.aromrSpriteList[8];
                break;
            case "가죽장갑":
                itemSpr = InventoryPanel.instance.aromrSpriteList[9];
                break;
            case "가죽신발":
                itemSpr = InventoryPanel.instance.aromrSpriteList[10];
                break;
            case "가죽바지":
                itemSpr = InventoryPanel.instance.aromrSpriteList[11];
                break;
            case "철갑옷":
                itemSpr = InventoryPanel.instance.aromrSpriteList[12];
                break;
            case "철망토":
                itemSpr = InventoryPanel.instance.aromrSpriteList[13];
                break;
            case "철헬멧":
                itemSpr = InventoryPanel.instance.aromrSpriteList[14];
                break;
            case "철장갑":
                itemSpr = InventoryPanel.instance.aromrSpriteList[15];
                break;
            case "철부츠":
                itemSpr = InventoryPanel.instance.aromrSpriteList[16];
                break;
            case "철바지":
                itemSpr = InventoryPanel.instance.aromrSpriteList[17];
                break;
            case "체력증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[0].itemSpr;
                break;
            case "스태미나증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[1].itemSpr;
                break;
            case "공격력증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[2].itemSpr;
                break;
            case "크리티컬증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[3].itemSpr;
                break;
            case "경험치증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[4].itemSpr;
                break;
            case "방어력증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[5].itemSpr;
                break;
            case "공방증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[6].itemSpr;
                break;
            case "골드량증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[7].itemSpr;
                break;
            case "운증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[8].itemSpr;
                break;
            case "경험치골드량증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[9].itemSpr;
                break;
        }

        return itemSpr;
    }

    public void OnClick()
    {
        OnSelect();
    }
}

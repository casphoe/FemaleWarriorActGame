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

        if (EquipmentPanel.instance.equipItemList[listIndex].nameKor == "��������")
        {
            Utils.OnOff(img.gameObject, false);

            switch(GameManager.data.lanauge)
            {
                case LANGUAGE.KOR:
                    txt[0].text = "��������";
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
            case "ü������":
                itemSpr = InventoryPanel.instance.PotionSpriteList[0];
                break;
            case "�߰�ü������":
                itemSpr = InventoryPanel.instance.PotionSpriteList[1];
                break;
            case "���ü������":
                itemSpr = InventoryPanel.instance.PotionSpriteList[2];
                break;
            case "���¹̳�����":
                itemSpr = InventoryPanel.instance.PotionSpriteList[3];
                break;
            case "�߰����¹̳�����":
                itemSpr = InventoryPanel.instance.PotionSpriteList[4];
                break;
            case "��޽��¹̳�����":
                itemSpr = InventoryPanel.instance.PotionSpriteList[5];
                break;
            case "�η�����":
                itemSpr = InventoryPanel.instance.weaponSpirteList[0];
                break;
            case "�콼���":
                itemSpr = InventoryPanel.instance.weaponSpirteList[1];
                break;
            case "���":
                itemSpr = InventoryPanel.instance.weaponSpirteList[2];
                break;
            case "ö���":
                itemSpr = InventoryPanel.instance.weaponSpirteList[3];
                break;
            case "�ù���":
                itemSpr = InventoryPanel.instance.weaponSpirteList[4];
                break;
            case "ö��":
                itemSpr = InventoryPanel.instance.weaponSpirteList[5];
                break;
            case "��ö��":
                itemSpr = InventoryPanel.instance.weaponSpirteList[6];
                break;
            case "����":
                itemSpr = InventoryPanel.instance.weaponSpirteList[7];
                break;
            case "�ö�������":
                itemSpr = InventoryPanel.instance.weaponSpirteList[8];
                break;
            case "õ��":
                itemSpr = InventoryPanel.instance.aromrSpriteList[0];
                break;
            case "õ����":
                itemSpr = InventoryPanel.instance.aromrSpriteList[1];
                break;
            case "õ���":
                itemSpr = InventoryPanel.instance.aromrSpriteList[2];
                break;
            case "õ�尩":
                itemSpr = InventoryPanel.instance.aromrSpriteList[3];
                break;
            case "õ�Ź�":
                itemSpr = InventoryPanel.instance.aromrSpriteList[4];
                break;
            case "õ����":
                itemSpr = InventoryPanel.instance.aromrSpriteList[5];
                break;
            case "���׿�":
                itemSpr = InventoryPanel.instance.aromrSpriteList[6];
                break;
            case "���׸���":
                itemSpr = InventoryPanel.instance.aromrSpriteList[7];
                break;
            case "�������":
                itemSpr = InventoryPanel.instance.aromrSpriteList[8];
                break;
            case "�����尩":
                itemSpr = InventoryPanel.instance.aromrSpriteList[9];
                break;
            case "���׽Ź�":
                itemSpr = InventoryPanel.instance.aromrSpriteList[10];
                break;
            case "���׹���":
                itemSpr = InventoryPanel.instance.aromrSpriteList[11];
                break;
            case "ö����":
                itemSpr = InventoryPanel.instance.aromrSpriteList[12];
                break;
            case "ö����":
                itemSpr = InventoryPanel.instance.aromrSpriteList[13];
                break;
            case "ö���":
                itemSpr = InventoryPanel.instance.aromrSpriteList[14];
                break;
            case "ö�尩":
                itemSpr = InventoryPanel.instance.aromrSpriteList[15];
                break;
            case "ö����":
                itemSpr = InventoryPanel.instance.aromrSpriteList[16];
                break;
            case "ö����":
                itemSpr = InventoryPanel.instance.aromrSpriteList[17];
                break;
            case "ü����������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[0].itemSpr;
                break;
            case "���¹̳���������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[1].itemSpr;
                break;
            case "���ݷ���������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[2].itemSpr;
                break;
            case "ũ��Ƽ����������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[3].itemSpr;
                break;
            case "����ġ��������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[4].itemSpr;
                break;
            case "�������������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[5].itemSpr;
                break;
            case "�������������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[6].itemSpr;
                break;
            case "��差���������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[7].itemSpr;
                break;
            case "�����������":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[8].itemSpr;
                break;
            case "����ġ��差���������":
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

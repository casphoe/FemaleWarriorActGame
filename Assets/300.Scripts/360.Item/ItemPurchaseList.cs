using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class ItemPurchaseList : MonoBehaviour
{
    public InfiniteScroll itemPurchaseScollList;
    public GameObject itemListContent;

    private int index = 0;
    private int selectIndex = 0;

    private List<itemPrefabData> dataList = new List<itemPrefabData>();


    private void Awake()
    {
        itemPurchaseScollList.AddSelectCallback((data) =>
        {
            EquipmentList.instance.purchaseIndexNumber = ((itemPrefabData)data).index;
            selectIndex = EquipmentList.instance.purchaseIndexNumber;
            ColorChange();
            PlayerDataSetting();
        });
    }

    public void AllDefaultColorChange()
    {
        EquipmentList.instance.purchaseIndexNumber = -1;
        selectIndex = -1;
        for (int i = 0; i < itemListContent.transform.childCount; i++)
        {
            Image img = itemListContent.transform.GetChild(i).GetComponent<Image>();
            if(img.color != new Color(1,1,1,0))
            {
                Utils.ImageColorChange(img, new Color(1, 1, 1, 0));
            }
        }
        EquipmentList.instance.AllDefaultPlayerDataSetting();
    }

    void ColorChange()
    {
        for (int i = 0; i < itemListContent.transform.childCount; i++)
        {
            Image img = itemListContent.transform.GetChild(i).GetComponent<Image>();
            itemPrefab dataItem = itemListContent.transform.GetChild(i).GetComponent<itemPrefab>();
            if(dataItem.listIndex == EquipmentList.instance.purchaseIndexNumber)
            {
                Utils.ImageColorChange(img, new Color(200 / 255f, 200 / 255f, 200 / 255f, 1));
            }
            else
            {
                Utils.ImageColorChange(img, new Color(1, 1, 1, 0));
            }
        }
    }

    void PlayerDataSetting()
    {
        switch (PlayerManager.instance.itemShopNum)
        {
            //무기
            case 0:
                EquipmentList.instance.EquipPlayerDataSetting(EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].HpUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].StaminaUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].attackUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].defenceUp,
                    EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].crictleRateUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].crictleDmgUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].lukUp
                    , EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].expUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].moneyUp, EquipmentList.instance.equipmentWeaponPurchaseList[selectIndex].itemMaxEnforce);
                break;
            //방어구
            case 1:
                EquipmentList.instance.EquipPlayerDataSetting(EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].HpUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].StaminaUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].attackUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].defenceUp,
                    EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].crictleRateUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].crictleDmgUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].lukUp
                    , EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].expUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].moneyUp, EquipmentList.instance.equipmentArmorPurchaseList[selectIndex].itemMaxEnforce);
                break;
            //액세서리
            case 2:
                EquipmentList.instance.EquipPlayerDataSetting(EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].HpUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].StaminaUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].attackUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].defenceUp,
                    EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].crictleRateUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].crictleDmgUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].lukUp
                    , EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].expUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].moneyUp, EquipmentList.instance.equipmentAccessoriesPurchaseList[selectIndex].itemMaxEnforce);
                break;
        }
    }

    void itemPurchaseListClear()
    {
        dataList.Clear();
        itemPurchaseScollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
        AllDefaultColorChange();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) // 이 부분 확인할 것
        {
            itemPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void itemWeaponPurchaseLoadList()
    {
        itemPurchaseListClear();
        if(dataList.Count != EquipmentList.instance.equipmentWeaponPurchaseList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentList.instance.equipmentWeaponPurchaseList.Count);
            for(int i = 0; i < difference; i++)
            {
                WeaponPurchaseInsertData();
            }
        }
        AllUpdate();
    }

    public void itemArmorPurchaseLoadList()
    {
        itemPurchaseListClear();
        if(dataList.Count != EquipmentList.instance.equipmentArmorPurchaseList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentList.instance.equipmentArmorPurchaseList.Count);
            for (int i = 0; i < difference; i++)
            {
                ArmorPurchaseInsertData();
            }
        }
        AllUpdate();
    }

    public void itemAccesoryPurchaseLoadList()
    {
        itemPurchaseListClear();
        if (dataList.Count != EquipmentList.instance.equipmentAccessoriesPurchaseList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentList.instance.equipmentAccessoriesPurchaseList.Count);
            for (int i = 0; i < difference; i++)
            {
                AccessoriesPurchaseInsertData();
            }
        }
        AllUpdate();
    }

    void WeaponPurchaseInsertData()
    {
        itemPrefabData data = new itemPrefabData();
        data.index = index++;
        data.number = itemPurchaseScollList.GetItemCount() + 1;
        dataList.Add(data);
        itemPurchaseScollList.InsertData(data);
    }

    void ArmorPurchaseInsertData()
    {
        itemPrefabData data = new itemPrefabData();
        data.index = index++;
        data.number = itemPurchaseScollList.GetItemCount() + 1;
        dataList.Add(data);
        itemPurchaseScollList.InsertData(data);
    }

    void AccessoriesPurchaseInsertData()
    {
        itemPrefabData data = new itemPrefabData();
        data.index = index++;
        data.number = itemPurchaseScollList.GetItemCount() + 1;
        dataList.Add(data);
        itemPurchaseScollList.InsertData(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        itemPurchaseScollList.UpdateAllData();
    }

}

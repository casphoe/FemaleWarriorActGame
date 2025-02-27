using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class itemSellList : MonoBehaviour
{
    public InfiniteScroll itemSellScrollList;
    public GameObject itemListContent;

    private int index = 0;
    private int selectIndex = 0;

    private List<itemPrefabData> dataList = new List<itemPrefabData>();

    private void Awake()
    {
        itemSellScrollList.AddSelectCallback((data) =>
        {
            EquipmentList.instance.sellIndexNumber = ((itemPrefabData)data).index;
            selectIndex = EquipmentList.instance.sellIndexNumber;
        });
    }

    void itemSellListClear()
    {
        dataList.Clear();
        itemSellScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
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

    public void itemWeaponSellLoadList()
    {
        itemSellListClear();
        if(dataList.Count != EquipmentList.instance.equipmentWeaponSellList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentList.instance.equipmentWeaponSellList.Count);
            for(int i = 0; i < difference; i++)
            {
                WeaponSellInsertData();
            }
        }
        AllUpdate();
    }

    public void itemArmorSellLoadList()
    {
        itemSellListClear();
        if (dataList.Count != EquipmentList.instance.equipmentArmorSellList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentList.instance.equipmentArmorSellList.Count);
            for (int i = 0; i < difference; i++)
            {
                ArmorSellInsertData();
            }
        }
        AllUpdate();
    }

    public void itemAccelySellLoadList()
    {
        itemSellListClear();
        if (dataList.Count != EquipmentList.instance.equipmentAccessoriesSellList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentList.instance.equipmentAccessoriesSellList.Count);
            for (int i = 0; i < difference; i++)
            {
                AccelySellInsertData();
            }
        }
        AllUpdate();
    }

    void ArmorSellInsertData()
    {
        itemPrefabData data = new itemPrefabData();
        data.index = index++;
        data.number = itemSellScrollList.GetItemCount() + 1;
        dataList.Add(data);
        itemSellScrollList.InsertData(data);
    }

    void AccelySellInsertData()
    {
        itemPrefabData data = new itemPrefabData();
        data.index = index++;
        data.number = itemSellScrollList.GetItemCount() + 1;
        dataList.Add(data);
        itemSellScrollList.InsertData(data);
    }

    void WeaponSellInsertData()
    {
        itemPrefabData data = new itemPrefabData();
        data.index = index++;
        data.number = itemSellScrollList.GetItemCount() + 1;
        dataList.Add(data);
        itemSellScrollList.InsertData(data);
    }


    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        itemSellScrollList.UpdateAllData();
    }
}

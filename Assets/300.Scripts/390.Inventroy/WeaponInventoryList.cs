using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
public class WeaponInventoryList : MonoBehaviour
{
    public InfiniteScroll weaponInventoryScrollList;
    public GameObject weaponInvetoryListContent;

    private List<InventoryItemPrefabData> dataList = new List<InventoryItemPrefabData>();

    public int index = 0;
    public int selectIndex = 0;

    private void Awake()
    {
        weaponInventoryScrollList.AddSelectCallback((data) =>
        {
            InventoryPanel.instance.selectWeaponIndex = ((InventoryItemPrefabData)data).index;
            selectIndex = InventoryPanel.instance.selectItemIndex;
        });
    }

    void weaponListClear()
    {
        dataList.Clear();
        weaponInventoryScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) 
        {
            InventoryItemPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void WeaponInventoryLoadList()
    {
        weaponListClear();
        if (dataList.Count != InventoryList.instance.weaponInventoryList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - InventoryList.instance.weaponInventoryList.Count);
            for (int i = 0; i < difference; i++)
            {
                WeaponInventoryInsertData();
            }
        }
        AllUpdate();
    }

    void WeaponInventoryInsertData()
    {
        InventoryItemPrefabData data = new InventoryItemPrefabData();
        data.index = index++;
        data.number = weaponInventoryScrollList.GetItemCount() + 1;
        dataList.Add(data);
        weaponInventoryScrollList.InsertData(data);
    }

    void AllUpdate()
    {
        weaponInventoryScrollList.UpdateAllData();
    }
}

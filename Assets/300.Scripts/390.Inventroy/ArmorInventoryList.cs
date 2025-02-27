using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class ArmorInventoryList : MonoBehaviour
{
    public InfiniteScroll armorInventoryScrollList;
    public GameObject armorInventoryListContent;

    private List<InventoryItemPrefabData> dataList = new List<InventoryItemPrefabData>();

    public int index = 0;
    public int selectIndex = 0;

    private void Awake()
    {
        armorInventoryScrollList.AddSelectCallback((data) =>
        {
            InventoryPanel.instance.selectArmorIndex = ((InventoryItemPrefabData)data).index;
            selectIndex = InventoryPanel.instance.selectItemIndex;
        });
    }

    void ArmorListClear()
    {
        dataList.Clear();
        armorInventoryScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) // 이 부분 확인할 것
        {
            InventoryItemPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void ArmorInventoryLoadList()
    {
        ArmorListClear();
        if (dataList.Count != InventoryList.instance.armorInventoryList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - InventoryList.instance.armorInventoryList.Count);
            for (int i = 0; i < difference; i++)
            {
                ArmorInventoryInsertData();
            }
        }
        AllUpdate();
    }

    void ArmorInventoryInsertData()
    {
        InventoryItemPrefabData data = new InventoryItemPrefabData();
        data.index = index++;
        data.number = armorInventoryScrollList.GetItemCount() + 1;
        dataList.Add(data);
        armorInventoryScrollList.InsertData(data);
    }

    void AllUpdate()
    {
        armorInventoryScrollList.UpdateAllData();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class EquipmentPanelList : MonoBehaviour
{
    public InfiniteScroll EquipmentPanelScrollList;
    public GameObject EquipInventoryListContent;

    private List<EquipItemData> dataList = new List<EquipItemData>();

    public int index = 0;
    public int selectIndex = 0;

    private void Awake()
    {
        EquipmentPanelScrollList.AddSelectCallback((data) =>
        {
            EquipmentPanel.instance.selectEquip = ((EquipItemData)data).index;
            selectIndex = EquipmentPanel.instance.selectEquip;
        });
    }

    public void EquipListClear()
    {
        dataList.Clear();
        EquipmentPanelScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++)
        {
            EquipItemData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void EquipInventoryLoadList()
    {
        EquipListClear();
        if (dataList.Count != EquipmentPanel.instance.equipItemList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EquipmentPanel.instance.equipItemList.Count);
            for (int i = 0; i < difference; i++)
            {
                EquipInventoryInsertData();
            }
        }
        AllUpdate();
    }

    void EquipInventoryInsertData()
    {
        EquipItemData data = new EquipItemData();
        data.index = index++;
        data.number = EquipmentPanelScrollList.GetItemCount() + 1;
        dataList.Add(data);
        EquipmentPanelScrollList.InsertData(data);
    }

    public void AllUpdate()
    {
        EquipmentPanelScrollList.UpdateAllData();
    }
}

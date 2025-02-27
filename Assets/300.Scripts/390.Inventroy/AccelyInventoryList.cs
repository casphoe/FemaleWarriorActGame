using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class AccelyInventoryList : MonoBehaviour
{
    public InfiniteScroll accelyInventoryScrollList;

    public GameObject accelyInventoryListContent;

    private List<InventoryItemPrefabData> dataList = new List<InventoryItemPrefabData>();

    public int index = 0;
    public int selectIndex = 0;

    private void Awake()
    {
        accelyInventoryScrollList.AddSelectCallback((data) =>
        {
            InventoryPanel.instance.selectAccelyIndex = ((InventoryItemPrefabData)data).index;
            selectIndex = InventoryPanel.instance.selectItemIndex;
        });
    }

    void AccelyListClear()
    {
        dataList.Clear();
        accelyInventoryScrollList.ClearData();
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

    public void AccelyInvetoryLoadList()
    {
        AccelyListClear();
        if (dataList.Count != InventoryList.instance.accelyInventoryList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - InventoryList.instance.accelyInventoryList.Count);
            for (int i = 0; i < difference; i++)
            {
                AccelyInventoryInsertData();
            }
        }
        AllUpdate();
    }

    void AccelyInventoryInsertData()
    {
        InventoryItemPrefabData data = new InventoryItemPrefabData();
        data.index = index++;
        data.number = accelyInventoryScrollList.GetItemCount() + 1;
        dataList.Add(data);
        accelyInventoryScrollList.InsertData(data);
    }

    void AllUpdate()
    {
        accelyInventoryScrollList.UpdateAllData();
    }
}

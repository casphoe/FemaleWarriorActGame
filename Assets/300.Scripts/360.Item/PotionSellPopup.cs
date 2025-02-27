using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSellPopup : MonoBehaviour
{
    public InfiniteScroll SellPotionScrollList;

    public GameObject PotionListContent;

    private int index = 0;

    int numberIndex = 0;

    private List<PotionPrefabData> dataList = new List<PotionPrefabData>();

    private void Awake()
    {
        SellPotionScrollList.AddSelectCallback((data) =>
        {
            PotionPurchaseList.instance.indexNumber = ((PotionPrefabData)data).index;
        });
    }

    public void SellLoadList()
    {
        SellListClear();
        if (dataList.Count != PotionPurchaseList.instance.itemSellPotionList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - PotionPurchaseList.instance.itemSellPotionList.Count);
            for (int i = 0; i < difference; i++)
            {
                SellInsertData();
            }
        }
        SellPotionAllUpdate();
    }

    void SellInsertData()
    {
        PotionPrefabData data = new PotionPrefabData();
        data.index = index++;
        data.number = SellPotionScrollList.GetItemCount() + 1;
        dataList.Add(data);
        SellPotionScrollList.InsertData(data);
    }

    void SellListClear()
    {
        dataList.Clear();
        SellPotionScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }


    public void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) // 이 부분 확인할 것
        {
            PotionPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    private void OnEnable()
    {
        SellPotionAllUpdate();
    }

    void SellPotionAllUpdate()
    {
        SellPotionScrollList.UpdateAllData();
    }
}

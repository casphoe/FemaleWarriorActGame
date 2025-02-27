using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class PotionList : MonoBehaviour
{
    public InfiniteScroll PurchasePotionScrollList;

    public GameObject PotionListContent;

    private int index = 0;

    int numberIndex = 0;

    private List<PotionPrefabData> dataList = new List<PotionPrefabData>();

    private void Awake()
    {
        PurchasePotionScrollList.AddSelectCallback((data) =>
        {
            PotionPurchaseList.instance.indexNumber = ((PotionPrefabData)data).index;
        });
    }

    void PurchaseListClear()
    {
        dataList.Clear();
        PurchasePotionScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }


    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) // 이 부분 확인할 것
        {
            PotionPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void PurchaseLoadList()
    {
        PurchaseListClear();
        if(dataList.Count != PotionPurchaseList.instance.itemPurchasePotionList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - PotionPurchaseList.instance.itemPurchasePotionList.Count);
            for(int i = 0; i < difference; i++)
            {
                PurchaseInsertData();
            }
        }
        PurchaseAllUpdate();
    }

    void PurchaseInsertData()
    {
        PotionPrefabData data = new PotionPrefabData();
        data.index = index++;
        data.number = PurchasePotionScrollList.GetItemCount() + 1;
        dataList.Add(data);
        PurchasePotionScrollList.InsertData(data);
    }

    private void OnEnable()
    {
        PurchaseAllUpdate();
    }

    void PurchaseAllUpdate()
    {
        PurchasePotionScrollList.UpdateAllData();
    }

}

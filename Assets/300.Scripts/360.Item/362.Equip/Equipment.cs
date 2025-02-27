using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] EquipmentPanelList dataList;

    public static Equipment instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnEquipData()
    {
        dataList.EquipInventoryLoadList();
    }

    public void OnEquipDataClear()
    {
        dataList.EquipListClear();
        dataList.AllUpdate();
    }
}

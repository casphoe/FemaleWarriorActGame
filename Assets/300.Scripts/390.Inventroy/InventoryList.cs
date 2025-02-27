using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    public static InventoryList instance;


    public List<ItemData> itemInventoryList = new List<ItemData>();

    public List<ItemData> weaponInventoryList = new List<ItemData>();

    public List<ItemData> armorInventoryList = new List<ItemData>();

    public List<ItemData> accelyInventoryList = new List<ItemData>();

    [SerializeField] ItemInventoryList dataList;
    [SerializeField] WeaponInventoryList dataWeaponList;
    [SerializeField] ArmorInventoryList dataArmorList;
    [SerializeField] AccelyInventoryList dataAccelyList;

    private void Awake()
    {
        instance = this;
    }

    public void OnItemInventroyData()
    {
        PlayerManager.instance.player.inventory.InventoryFillerItems(itemInventoryList, 0);
        dataList.itemInventoryLoadList();
    }

    public void OnWeaponInventoryData()
    {
        PlayerManager.instance.player.inventory.InventoryFillerItems(weaponInventoryList, 1);
        dataWeaponList.WeaponInventoryLoadList();
    }

    public void OnArmorInventoryData()
    {
        PlayerManager.instance.player.inventory.InventoryFillerItems(armorInventoryList, 2);
        dataArmorList.ArmorInventoryLoadList();
    }

    public void OnAccelyInventoryData()
    {
        PlayerManager.instance.player.inventory.InventoryFillerItems(accelyInventoryList, 3);
        dataAccelyList.AccelyInvetoryLoadList();
    }

    public void OnItemUiSelect(int num)
    {
        dataList.OnListSlot(num);
    }
}

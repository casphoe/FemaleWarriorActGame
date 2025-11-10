using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SerializeField]
public class Inventory 
{
    public List<InventoryData> inventoryDataList = new List<InventoryData>();

    // 데이터 추가 메서드
    public void AddItemToInventory(int slotNum, string formattedDateTime, ItemData itemdata)
    {  
        InventoryData newInventoryData = new InventoryData
        {
            slotNum = slotNum,
            slotDateTime = formattedDateTime,
            item = itemdata
        };
        inventoryDataList.Add(newInventoryData);       
    }

    public void AddNewItemToInventory(string itemNamgEng,string itemName, ItemDb itemdb,DataDb particular, float hpRestoration, float staminaRestoration, float defenceUp, float attackUp, float crictleDup,float crictleRup, int expUp, int moneyUp, int luckUp,
        int haveCount, int currentEnforce, int maxEnforce, float hpUp, float stUP, int productPrice)
    {

        // 아이템 중복 여부 확인
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item.nameKor == itemName)
            {
                inventoryData.item.haveCount += haveCount; // 개수 증가
                inventoryData.item.UpdateEquippCount();
                Debug.Log(itemName + "의 개수가 증가했습니다. 현재 개수: " + inventoryData.item.haveCount);
                return;
            }
        }

        ItemData newItemData = new ItemData();
        newItemData.ItemSetting(itemNamgEng, itemName,itemdb,particular,hpRestoration, staminaRestoration, defenceUp, attackUp, crictleDup, crictleRup, expUp, moneyUp, luckUp, haveCount, currentEnforce, maxEnforce, hpUp, stUP, productPrice
            );

        //현재날짜를 원하는 형식으로 변경 (T가 포함되지 않도록)
        string formattedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        //인벤토리에 추가
        AddItemToInventory(inventoryDataList.Count + 1, formattedDateTime, newItemData);

        if(newItemData.db == ItemDb.Weapon  || newItemData.db == ItemDb.Armor || newItemData.db == ItemDb.Accely)
        {
            EnforceList.instance.AddToEnforceList();
        }
        Debug.Log("새 아이템이 인벤토리에 추가되었습니다.");
    }
    // 인벤토리에서 동일한 강화 등급의 아이템 찾기
    public ItemData FindItemByNameAndEnforceLevel(string nameKor, int enforceLevel)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item != null &&
                inventoryData.item.nameKor == nameKor &&
                inventoryData.item.itemEnforce == enforceLevel)
            {
                return inventoryData.item;
            }
        }
        return null; // 해당 아이템을 찾지 못한 경우
    }

    // 이름으로 아이템을 찾고, 판매 가능한 개수를 반환하는 함수
    public int GetSellableItemCount(string itemName)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item.nameKor == itemName)
            {
                return inventoryData.item.unequippedCount; // 미장착된 개수 반환
            }
        }

        Debug.Log(itemName + " 인벤토리에서 찾을 수 없습니다.");
        return 0;
    }

    public float EatHpPotion(string itemName)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item.nameKor == itemName)
            {
                return inventoryData.item.hpRestoration;
            }
        }
        return 0;
    }

    public float EatStaminaPotion(string itemName)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item.nameKor == itemName)
            {
                return inventoryData.item.stminaRestoration;
            }
        }
        return 0;
    }

    // 모든 아이템을 반환하는 메서드
    public List<ItemData> GetAllItems()
    {
        List<ItemData> allItems = new List<ItemData>();
        foreach (var inventoryData in inventoryDataList)
        {
            allItems.Add(inventoryData.item);  // inventoryData의 item을 모두 반환
        }
        return allItems;
    }

    //이름으로 아이템을 찾아 개수를 반환하는 함수
    public int GetItemCountByName(string itemName)
    {
        //아이템 이름으로 검색하여 찾기
        foreach(var inventroyData in inventoryDataList)
        {
            if(inventroyData.item.nameKor == itemName)
            {
                // 해당 아이템의 개수를 반환
                return inventroyData.item.haveCount;
            }
        }

        //아이템이 존재하지 않으면 0으로 반환
        return 0;
    }

    // 이름으로 아이템을 찾아 haveCount 값을 빼는 함수
    public bool RemoveItemByName(string itemName, int countToRemove)
    {
        for (int i = 0; i < inventoryDataList.Count; i++)
        {          
            if (inventoryDataList[i].item.nameKor == itemName)
            {
                if (inventoryDataList[i].item.haveCount >= countToRemove)
                {
                    inventoryDataList[i].item.haveCount -= countToRemove;
                    inventoryDataList[i].item.unequippedCount -= countToRemove;
                    Debug.Log(itemName + "의 개수를 " + countToRemove + "만큼 뺐습니다. 현재 개수: " + inventoryDataList[i].item.haveCount);

                    // 개수가 0이 되면 인벤토리에서 삭제
                    if (inventoryDataList[i].item.haveCount <= 0)
                    {
                        inventoryDataList.RemoveAt(i);
                        ReorderInventorySlots();
                        Debug.Log(itemName + "의 개수가 0이 되어 인벤토리에서 삭제되었습니다.");
                    }
                    return true;
                }
                else
                {
                    Debug.Log("아이템 개수가 부족합니다. 현재 개수: " + inventoryDataList[i].item.haveCount);
                    return false;
                }
            }
        }
        Debug.Log(itemName + "을(를) 찾을 수 없습니다.");
        return false;
    }

    private void ReorderInventorySlots()
    {
        for (int i = 0; i < inventoryDataList.Count; i++)
        {
            inventoryDataList[i].slotNum = i + 1;
        }
        Debug.Log("인벤토리 슬롯 번호가 재정렬되었습니다.");
    }
    //인벤토리에서 판매 할 아이템 가져오는 함수
    public void FillerItems(List<ItemData> item, int num)
    {
        item.Clear();
        foreach (InventoryData inventoryData in inventoryDataList)
        {
           if(inventoryData.item != null && inventoryData.item.unequippedCount > 0)
            {
                switch(num)
                {
                    case 0:
                        if(inventoryData.item.db == ItemDb.Weapon)
                        {
                            item.Add(inventoryData.item);
                        }
                        break;
                    case 1:
                        if (inventoryData.item.db == ItemDb.Armor)
                        {
                            item.Add(inventoryData.item);
                        }
                        break;
                    case 2:
                        if (inventoryData.item.db == ItemDb.Accely)
                        {
                            item.Add(inventoryData.item);
                        }
                        break;
                }
            }
        }
    }

    //장비창의 대한 인벤토리 작업
    public void FillerParticularityItems(List<ItemData> item, int num)
    {
        item.Clear();

        List<InventoryData> filteredList = new List<InventoryData>();

        ItemData newItemData = new ItemData();
        newItemData.ItemSetting("Unequipped", "장착해제", ItemDb.None, DataDb.None, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            );
        // 장착 해제 아이템을 InventoryData로 감싸서 filteredList에 추가
        InventoryData unequippedInventoryData = new InventoryData
        {
            item = newItemData
        };

        filteredList.Add(unequippedInventoryData);

        foreach (InventoryData inventoryData in inventoryDataList)
        {
            inventoryData.item.UpdateEquippCount();
            if (inventoryData.item != null && inventoryData.item.unequippedCount > 0)
            {
                //nventoryData.item.dataDb 값이 주어진 num 값과 일치하는 지 판별하고 그에 따라서 리스트에 아이템 추가
                bool shouldAdd = false;

                switch (num)
                {
                    case 0: shouldAdd = inventoryData.item.dataDb == DataDb.Head; break;
                    case 1: shouldAdd = inventoryData.item.dataDb == DataDb.Necklace; break;
                    case 2: shouldAdd = inventoryData.item.dataDb == DataDb.Cloack; break;
                    case 3: shouldAdd = inventoryData.item.dataDb == DataDb.Cheast; break;
                    case 4: shouldAdd = inventoryData.item.dataDb == DataDb.Hand; break;
                    case 5: shouldAdd = inventoryData.item.dataDb == DataDb.Ring; break;
                    case 6: shouldAdd = inventoryData.item.dataDb == DataDb.Pants; break;
                    case 7: shouldAdd = inventoryData.item.dataDb == DataDb.Weapon; break;
                    case 8: shouldAdd = inventoryData.item.dataDb == DataDb.Boots; break;
                }
                if (shouldAdd)
                {
                    filteredList.Add(inventoryData);
                }
            }
        }

        // 첫 번째 아이템을 저장 (첫번째 아이템은 장착 해제)
        InventoryData firstItem = filteredList[0];

        // 장착된 아이템이 먼저 오도록 정렬 1번째 인덱스 부터 정렬
        filteredList = filteredList.Skip(1).OrderByDescending(inv => inv.item.equip == Equip.Equip).ThenByDescending(inv => DateTime.TryParse
        (inv.slotDateTime, out var date) ? date : DateTime.MinValue).ToList();

        //첫번째 아이템 유지
        item.Add(firstItem.item);
        // 정렬된 나머지 아이템 추가
        item.AddRange(filteredList.Select(inv => inv.item));     
    }

    //인벤토리 아이템 장착
    public void InventoryEquipItem(string itemName)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            //아이템 이름이 같고,미장착 상태이며, 미장착 개수가 1이상일 경우
            if (inventoryData.item.nameKor == itemName &&  inventoryData.item.unequippedCount > 0)
            {               
                inventoryData.item.equippedCount = 1;               

                inventoryData.item.UpdateEquippCount();              

                // 장착 개수가 1 이상이면 장착 상태로 변경
                if (inventoryData.item.equippedCount > 0)
                {
                    inventoryData.item.equip = Equip.Equip;
                }             
                return;
            }
        }
    }

    //해당 아이템의 대한 스텟데이터 가져오는 함수
    public ItemData InventoryItemStat(string itemName)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item.nameKor == itemName)
            {
                return inventoryData.item; // 해당 아이템 데이터 반환
            }
        }
        return null;
    }

    //장착된 아이템 장착 제거
    public void RemoveEquipItem(string itemName)
    {
        foreach (var inventoryData in inventoryDataList)
        {
            if (inventoryData.item.nameKor == itemName && inventoryData.item.equippedCount > 0)
            {
                inventoryData.item.equippedCount--;               
                inventoryData.item.UpdateEquippCount();

                if (inventoryData.item.equippedCount == 0)
                {
                    inventoryData.item.equip = Equip.None;
                }
            }
        }
    }

    public void InventoryFillerItems(List<ItemData> item, int num)
    {
        item.Clear();

        List<InventoryData> filteredList = new List<InventoryData>();

        foreach (InventoryData inventoryData in inventoryDataList)
        {
            if (inventoryData.item != null)
            {
                switch(num)
                {
                    case 0:
                        if (inventoryData.item.db == ItemDb.HpPotion || inventoryData.item.db == ItemDb.StaminaPotion)
                        {
                            filteredList.Add(inventoryData);
                        }
                        break;
                    case 1:
                        if (inventoryData.item.db == ItemDb.Weapon)
                        {
                            filteredList.Add(inventoryData);
                        }
                        break;
                    case 2:
                        if (inventoryData.item.db == ItemDb.Armor)
                        {
                            filteredList.Add(inventoryData);
                        }
                        break;
                    case 3:
                        if (inventoryData.item.db == ItemDb.Accely)
                        {
                            filteredList.Add(inventoryData);
                        }
                        break;
                }
            }
        }

        switch(InventoryPanel.instance.selectOrder)
        {
            case 0:
                // 오름차순 정렬 (가장 오래된 것이 마지막에 오도록)
                filteredList = filteredList.OrderBy(data => DateTime.Parse(data.slotDateTime)).ToList();
                break;
            case 1:
                // 내림차순 정렬 (가장 최근 것이 마지막에 오도록)
                filteredList = filteredList.OrderByDescending(data => DateTime.Parse(data.slotDateTime)).ToList();
                break;
        }

        if(num == 0)
        {
            // 정렬된 아이템을 최종 리스트에 추가
            foreach (var inventoryData in filteredList)
            {
                item.Add(inventoryData.item);
            }
        }
        else
        {
            // 최종 리스트 추가 (장착 여부에 따라 분리)
            Dictionary<Guid, ItemData> itemDictionary = new Dictionary<Guid, ItemData>();

            foreach (var inventoryData in filteredList)
            {
                ItemData originalItem = inventoryData.item;
                originalItem.UpdateEquippCount();

                // 미장착 상태의 아이템 추가
                if (originalItem.unequippedCount > 0)
                {
                    ItemData unequippedItem = (ItemData)originalItem.Clone();
                    unequippedItem.haveCount = originalItem.unequippedCount;
                    unequippedItem.equip = Equip.None;
                    item.Add(unequippedItem);
                }

                // 장착 상태의 아이템 추가
                if (originalItem.equippedCount > 0)
                {
                    ItemData equippedItem = (ItemData)originalItem.Clone();
                    equippedItem.haveCount = originalItem.equippedCount;
                    equippedItem.equip = Equip.Equip;
                    item.Add(equippedItem);
                }
            }
        }
    }

    public void Clear()
    {
        inventoryDataList.Clear();
        // 필요하면 여기서 슬롯 재정렬/세이브 트리거 등 수행
    }
}

[SerializeField]
public class InventoryData
{
    public int slotNum = 0;

    public string slotDateTime;

    public ItemData item;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class EnforceItemList : MonoBehaviour
{
    public InfiniteScroll itemEnforceScollList;
    public GameObject enforceListContent;

    private int index = 0;
    private int selectIndex = 0;

    private List<EnforceItemData> dataList = new List<EnforceItemData>();

    private void Awake()
    {
        itemEnforceScollList.AddSelectCallback((data) =>
        {
            EnforceList.instance.selectIndex = ((EnforceItemData)data).index;
            selectIndex = EnforceList.instance.selectIndex;
            ColorChange();
            EnforceItemDataSetting();
            switch(PlayerManager.instance.itemShopNum)
            {
                case 0:
                    EnforceList.instance.OnEnforceTextSetting(EnforceList.instance.enforceWeaponItems[selectIndex].nameKor + " 아이템 강화 확률이 " + EnforceList.instance.enhanceRate + "% 입니다. 강화하시겠습니까?",
                        EnforceList.instance.enforceWeaponItems[selectIndex].name + " Item Enhance Rate " + EnforceList.instance.enhanceRate + "% Would you like to Enhance it");
                    break;
                case 1:
                    EnforceList.instance.OnEnforceTextSetting(EnforceList.instance.enforceArmorItems[selectIndex].nameKor + " 아이템 강화 확률이 " + EnforceList.instance.enhanceRate + "% 입니다. 강화하시겠습니까?",
                       EnforceList.instance.enforceArmorItems[selectIndex].name + " Item Enhance Rate " + EnforceList.instance.enhanceRate + "% Would you like to Enhance it");
                    break;
                case 2:
                    EnforceList.instance.OnEnforceTextSetting(EnforceList.instance.enforceAccelyItems[selectIndex].nameKor + " 아이템 강화 확률이 " + EnforceList.instance.enhanceRate + "% 입니다. 강화하시겠습니까?",
                       EnforceList.instance.enforceAccelyItems[selectIndex].name + " Item Enhance Rate " + EnforceList.instance.enhanceRate + "% Would you like to Enhance it");
                    break;
            }
        });
    }

    public void AllDefualtColorChange()
    {
        EnforceList.instance.selectIndex = -1;
        selectIndex = -1;
        for(int i = 0; i < enforceListContent.transform.childCount; i++)
        {
            Image img = enforceListContent.transform.GetChild(i).GetComponent<Image>();
            if (img.color != new Color(1, 1, 1, 0))
            {
                Utils.ImageColorChange(img, new Color(1, 1, 1, 0));
            }
        }
        EnforceList.instance.AllDefaultEnforceSetting();
    }

    void ColorChange()
    {
        for (int i = 0; i < enforceListContent.transform.childCount; i++)
        {
            Image img = enforceListContent.transform.GetChild(i).GetComponent<Image>();
            EnforceItem dataItem = enforceListContent.transform.GetChild(i).GetComponent<EnforceItem>();
            if(dataItem.listIndex == EnforceList.instance.selectIndex)
            {
                Utils.ImageColorChange(img, new Color(200 / 255f, 200 / 255f, 200 / 255f, 1));
            }
            else
            {
                Utils.ImageColorChange(img, new Color(1, 1, 1, 0));
            }
        }
    }

    void EnforceItemDataSetting()
    {
        switch (PlayerManager.instance.itemShopNum)
        {
            case 0:
                EnforceList.instance.EnforceItemDataSetting(EnforceList.instance.enforceWeaponItems[selectIndex].HpUp, EnforceList.instance.enforceWeaponItems[selectIndex].StaminaUp, EnforceList.instance.enforceWeaponItems[selectIndex].attackUp,
                    EnforceList.instance.enforceWeaponItems[selectIndex].defenceUp, EnforceList.instance.enforceWeaponItems[selectIndex].crictleRateUp, EnforceList.instance.enforceWeaponItems[selectIndex].crictleDmgUp, EnforceList.instance.enforceWeaponItems[selectIndex].lukUp,
                    EnforceList.instance.enforceWeaponItems[selectIndex].itemEnforce, EnforceList.instance.enforceWeaponItems[selectIndex].itemMaxEnforce, ItemDb.Weapon);
                break;
            case 1:
                EnforceList.instance.EnforceItemDataSetting(EnforceList.instance.enforceArmorItems[selectIndex].HpUp, EnforceList.instance.enforceArmorItems[selectIndex].StaminaUp, EnforceList.instance.enforceArmorItems[selectIndex].attackUp,
                    EnforceList.instance.enforceArmorItems[selectIndex].defenceUp, EnforceList.instance.enforceArmorItems[selectIndex].crictleRateUp, EnforceList.instance.enforceArmorItems[selectIndex].crictleDmgUp, EnforceList.instance.enforceArmorItems[selectIndex].lukUp,
                    EnforceList.instance.enforceArmorItems[selectIndex].itemEnforce, EnforceList.instance.enforceArmorItems[selectIndex].itemMaxEnforce, ItemDb.Armor);
                break;
            case 2:
                EnforceList.instance.EnforceItemDataSetting(EnforceList.instance.enforceAccelyItems[selectIndex].HpUp, EnforceList.instance.enforceAccelyItems[selectIndex].StaminaUp, EnforceList.instance.enforceAccelyItems[selectIndex].attackUp,
                    EnforceList.instance.enforceAccelyItems[selectIndex].defenceUp, EnforceList.instance.enforceAccelyItems[selectIndex].crictleRateUp, EnforceList.instance.enforceAccelyItems[selectIndex].crictleDmgUp, EnforceList.instance.enforceAccelyItems[selectIndex].lukUp,
                    EnforceList.instance.enforceAccelyItems[selectIndex].itemEnforce, EnforceList.instance.enforceAccelyItems[selectIndex].itemMaxEnforce, ItemDb.Accely);
                break;
        }
    }

    void itemEnforceListClear()
    {
        dataList.Clear();
        itemEnforceScollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
        AllDefualtColorChange();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) 
        {
            EnforceItemData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void enforceWeaponLoadList()
    {
        itemEnforceListClear();
        if(dataList.Count != EnforceList.instance.enforceWeaponItems.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EnforceList.instance.enforceWeaponItems.Count);
            for(int i = 0; i < difference; i++)
            {
                EnforceWeaponInsertData();
            }
        }
        AllUpdate();
    }

    public void enforceArmorLoadList()
    {
        itemEnforceListClear();
        if(dataList.Count != EnforceList.instance.enforceArmorItems.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EnforceList.instance.enforceArmorItems.Count);
            for(int i = 0; i < difference; i++)
            {
                EnforceArmorInsertData();
            }
        }
        AllUpdate();
    }

    public void enforceAccelyLoadList()
    {
        itemEnforceListClear();
        if(dataList.Count != EnforceList.instance.enforceAccelyItems.Count)
        {
            int difference = Mathf.Abs(dataList.Count - EnforceList.instance.enforceAccelyItems.Count);
            for(int i = 0; i < difference; i++)
            {
                EnforceAccelyInsertData();
            }
        }
        AllUpdate();
    }

    void EnforceWeaponInsertData()
    {
        EnforceItemData data = new EnforceItemData();
        data.index = index++;
        data.number = itemEnforceScollList.GetItemCount() + 1;
        dataList.Add(data);
        itemEnforceScollList.InsertData(data);
    }

    void EnforceArmorInsertData()
    {
        EnforceItemData data = new EnforceItemData();
        data.index = index++;
        data.number = itemEnforceScollList.GetItemCount() + 1;
        dataList.Add(data);
        itemEnforceScollList.InsertData(data);
    }

    void EnforceAccelyInsertData()
    {
        EnforceItemData data = new EnforceItemData();
        data.index = index++;
        data.number = itemEnforceScollList.GetItemCount() + 1;
        dataList.Add(data);
        itemEnforceScollList.InsertData(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        itemEnforceScollList.UpdateAllData();
    }
}

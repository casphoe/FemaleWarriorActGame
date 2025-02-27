using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnforceList : MonoBehaviour
{
    public static EnforceList instance;

    public int selectIndex = -1;

    //무기 강화 아이템을 딕셔너리로 구현
    public Dictionary<int, List<ItemData>> enforcedItemsByLevel = new Dictionary<int, List<ItemData>>();

    public List<ItemData> enforceWeaponItems = new List<ItemData>();

    public List<ItemData> enforceArmorItems = new List<ItemData>();

    public List<ItemData> enforceAccelyItems = new List<ItemData>();

    // 0 :체력, 1 : 스태미나, 2 : 공격력, 3 : 방어력 , 4 : 크리티컬 확율, 5 : 크리티컬 데미지, 6 : 운, 7: 강화 확률, 8 : 현재 강화 등급, 9: 최대 강화할 수 있는 등급
    public Text[] txtPlayerData;

    // 0: 강화 시도, 강화 시도 x
    [SerializeField] Button[] btnEnforce;

    [SerializeField] Text txtEnfroce;

    [SerializeField] EnforceItemList enforceDataList;

    Enforce enforce;

    string selectName = string.Empty;

    public int enhanceRate;

    private void Awake()
    {
        instance = this;

        btnEnforce[0].onClick.AddListener(() => OnButtonEnforceClick(0));
        btnEnforce[1].onClick.AddListener(() => OnButtonEnforceClick(1));
        enhanceRate = 0;
        enforce = new Enforce();
    }

    public void OnEnforceTextSetting(string data, string data2)
    {
        switch(GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtEnfroce.text = data;
                break;
            case LANGUAGE.ENG:
                txtEnfroce.text = data2;
                break;
        }      
    }

    void OnButtonEnforceClick(int num)
    {
        switch(num)
        {
            //강화 시도
            case 0:
                selectName = string.Empty;
                switch (PlayerManager.instance.itemShopNum)
                {
                    case 0: //무기                                          
                        int weaponResult = enforce.TryReinforce(enforceWeaponItems[selectIndex]);
                        selectName = enforceWeaponItems[selectIndex].nameKor;
                        if (weaponResult == 0) //성공
                        {
                            switch(GameManager.data.lanauge)
                            {
                                case LANGUAGE.KOR:
                                    ShopPanel.instance.ShopTextSetting("강화 성공", enforceWeaponItems[selectIndex].nameKor + " 아이템 강화가 성공 했습니다.");
                                    break;
                                case LANGUAGE.ENG:
                                    ShopPanel.instance.ShopTextSetting("ReinForce Success", enforceWeaponItems[selectIndex].name + " Item Enhance Success");
                                    break;
                            }                           
                            ShowItemsByType(ItemDb.Weapon);
                            WeaponEnforceShow();
                        }
                        else
                        {
                            if(weaponResult == 1) //강화 실패
                            {
                                switch(GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", enforceWeaponItems[selectIndex].nameKor + " 아이템 강화가 실패 했습니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", enforceWeaponItems[selectIndex].name + " Item Enhance Failled");
                                        break;
                                }
                            }
                            else if(weaponResult == 2) //최고 단계
                            {
                                switch(GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", enforceWeaponItems[selectIndex].nameKor + " 아이템 강화가 최고 단계 입니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", enforceWeaponItems[selectIndex].name + " Item Enhance Level Max");
                                        break;
                                }
                            }
                            else if(weaponResult == 3) //강화 확률 못가지고 옴
                            {

                            }
                            else
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", "돈이 " + Utils.GetThousandCommaText(weaponResult) + "원 부족합니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", "I'm " + Utils.GetThousandCommaText(weaponResult) + " won short");
                                        break;
                                }
                            }
                        }                      
                        break;
                    case 1: //방어구                      
                        int armorResult = enforce.TryReinforce(enforceArmorItems[selectIndex]);
                        selectName = enforceArmorItems[selectIndex].nameKor;
                        if (armorResult == 0) //성공
                        {
                            switch (GameManager.data.lanauge)
                            {
                                case LANGUAGE.KOR:
                                    ShopPanel.instance.ShopTextSetting("강화 성공", enforceArmorItems[selectIndex].nameKor + " 아이템 강화가 성공 했습니다.");
                                    break;
                                case LANGUAGE.ENG:
                                    ShopPanel.instance.ShopTextSetting("ReinForce Success", enforceArmorItems[selectIndex].name + " Item Enhance Success");
                                    break;
                            }
                            ShowItemsByType(ItemDb.Armor);
                            ArmorEnforceShow();
                        }
                        else
                        {
                            if (armorResult == 1) //강화 실패
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", enforceArmorItems[selectIndex].nameKor + " 아이템 강화가 실패 했습니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", enforceArmorItems[selectIndex].name + " Item Enhance Failled");
                                        break;
                                }
                            }
                            else if (armorResult == 2) //최고 단계
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", enforceArmorItems[selectIndex].nameKor + " 아이템 강화가 최고 단계 입니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", enforceArmorItems[selectIndex].name + " Item Enhance Level Max");
                                        break;
                                }
                            }
                            else if (armorResult == 3) //강화 확률 못가지고 옴
                            {

                            }
                            else
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", "돈이 " + Utils.GetThousandCommaText(armorResult) + "원 부족합니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", "I'm " + Utils.GetThousandCommaText(armorResult) + " won short");
                                        break;
                                }
                            }
                        }                     
                        break;
                    case 2: //액세서리                       
                        int accelyResult = enforce.TryReinforce(enforceAccelyItems[selectIndex]);
                        selectName = enforceAccelyItems[selectIndex].nameKor;
                        if (accelyResult == 0) //성공
                        {
                            switch (GameManager.data.lanauge)
                            {
                                case LANGUAGE.KOR:
                                    ShopPanel.instance.ShopTextSetting("강화 성공", enforceAccelyItems[selectIndex].nameKor + " 아이템 강화가 성공 했습니다.");
                                    break;
                                case LANGUAGE.ENG:
                                    ShopPanel.instance.ShopTextSetting("ReinForce Success", enforceAccelyItems[selectIndex].name + " Item Enhance Success");
                                    break;
                            }
                            ShowItemsByType(ItemDb.Accely);
                            AccelyEnforceShow();
                        }
                        else
                        {
                            if (accelyResult == 1) //강화 실패
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", enforceAccelyItems[selectIndex].nameKor + " 아이템 강화가 실패 했습니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", enforceAccelyItems[selectIndex].name + " Item Enhance Failled");
                                        break;
                                }
                            }
                            else if (accelyResult == 2) //최고 단계
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", enforceAccelyItems[selectIndex].nameKor + " 아이템 강화가 최고 단계 입니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", enforceAccelyItems[selectIndex].name + " Item Enhance Level Max");
                                        break;
                                }
                            }
                            else if (accelyResult == 3) //강화 확률 못가지고 옴
                            {

                            }
                            else
                            {
                                switch (GameManager.data.lanauge)
                                {
                                    case LANGUAGE.KOR:
                                        ShopPanel.instance.ShopTextSetting("강화 실패", "돈이 " + Utils.GetThousandCommaText(accelyResult) + "원 부족합니다.");
                                        break;
                                    case LANGUAGE.ENG:
                                        ShopPanel.instance.ShopTextSetting("ReinForce Failled", "I'm " + Utils.GetThousandCommaText(accelyResult) + " won short");
                                        break;
                                }
                            }
                        }                        
                        break;
                }
                txtEnfroce.text = "";
                break;
                //강화 시도x
            case 1:
                AllDefaultEnforceSetting();
                txtEnfroce.text = "";
                enforceDataList.AllDefualtColorChange();
                break;
        }
    }

    public void AddToEnforceList()
    {
        enforcedItemsByLevel.Clear();

        // 인벤토리에서 모든 아이템 가져오기
        List<ItemData> allItems = PlayerManager.instance.player.inventory.GetAllItems();

        // 강화 레벨 기준으로 정렬
        allItems = allItems
            .Where(item => item.itemMaxEnforce > 0 && item.haveCount > 0) // 조건 필터링
            .OrderBy(item => item.itemEnforce) // 강화 등급 기준으로 정렬
            .ToList();


        //// 모든 아이템을 강화 레벨에 따라 딕셔너리에 추가
        foreach (var item in allItems)
        {
            if (item.itemMaxEnforce > 0 && item.haveCount > 0 && item.itemEnforce < item.itemMaxEnforce)
            {
                // 강화 레벨이 딕셔너리에 존재하는지 확인 없으면 만듬
                if (!enforcedItemsByLevel.ContainsKey(item.itemEnforce))
                {
                    enforcedItemsByLevel[item.itemEnforce] = new List<ItemData>();
                }

                // Add the item to the list for its enforcement level
                enforcedItemsByLevel[item.itemEnforce].Add(item);
                Debug.Log(item.nameKor + " added to enforce list at level " + item.itemEnforce);
            }
        }
    }

    public void ShowItemsByType(ItemDb itemType)
    {
        if (enforcedItemsByLevel.Count == 0)
        {
            Debug.Log("enforcedItemsByLevel is empty.");
        }
        else
        {
            switch(itemType)
            {
                case ItemDb.Weapon:
                    enforceWeaponItems.Clear(); // 기존 데이터를 초기화
                    foreach (var kvp in enforcedItemsByLevel)
                    {
                        enforceWeaponItems.AddRange(kvp.Value.Where(item => item.db == ItemDb.Weapon)); // 무기 타입만 추가
                    }
                    break;
                case ItemDb.Armor:
                    enforceArmorItems.Clear();
                    foreach (var kvp in enforcedItemsByLevel)
                    {
                        enforceArmorItems.AddRange(kvp.Value.Where(item => item.db == ItemDb.Armor)); // 방어구  타입만 추가                      
                    }
                    break;
                case ItemDb.Accely:
                    enforceAccelyItems.Clear();
                    foreach (var kvp in enforcedItemsByLevel)
                    {
                        enforceAccelyItems.AddRange(kvp.Value.Where(item => item.db == ItemDb.Accely)); // 액세서리 타입만 추가
                    }
                    break;
            }
        }
    }

    public void WeaponEnforceShow()
    {
        enforceDataList.enforceWeaponLoadList();
    }

    public void ArmorEnforceShow()
    {
        enforceDataList.enforceArmorLoadList();
    }

    public void AccelyEnforceShow()
    {
        enforceDataList.enforceAccelyLoadList();
    }


    public void AllDefaultEnforceSetting()
    {
        for(int i = 0; i < txtPlayerData.Length; i++)
        {
            txtPlayerData[i].text = "";
        }
    }

    public void EnforceItemDataSetting(float hp,float stamina,float attack, float defence, float ciritleRate, float ciritleDmg, int luck, int current, int max , ItemDb db)
    {
        txtPlayerData[0].text = Utils.GetThousandCommaText(hp) + " + " + Utils.GetThousandCommaText(hp * 0.1f) + " = " + Utils.GetThousandCommaText(Mathf.Round((hp + (hp * 0.1f)) * 10) / 10f);

        txtPlayerData[1].text = Utils.GetThousandCommaText(stamina) + " + " + Utils.GetThousandCommaText(stamina * 0.1f) + " = " + Utils.GetThousandCommaText(Mathf.Round((stamina + (stamina * 0.1f)) * 10) / 10f);

        txtPlayerData[2].text = Utils.GetThousandCommaText(attack) + " + " + Utils.GetThousandCommaText(attack * 0.1f) + " = " + Utils.GetThousandCommaText(Mathf.Round((attack + (attack * 0.1f)) * 10) / 10f);

        txtPlayerData[3].text = Utils.GetThousandCommaText(defence) + " + " + Utils.GetThousandCommaText(defence * 0.1f) + " = " + Utils.GetThousandCommaText(Mathf.Round((defence + (defence * 0.1f)) * 10) / 10f);

        txtPlayerData[4].text = Utils.GetThousandCommaText(ciritleRate) + " + " + Utils.GetThousandCommaText(ciritleRate * 0.1f) + " = " + Utils.GetThousandCommaText(Mathf.Round((ciritleRate + (ciritleRate * 0.1f)) * 10) / 10f);

        txtPlayerData[5].text = Utils.GetThousandCommaText(ciritleDmg) + " + " + Utils.GetThousandCommaText(ciritleDmg * 0.1f) + " = " + Utils.GetThousandCommaText(Mathf.Round((ciritleDmg + (ciritleDmg * 0.1f)) * 10) / 10f);

        if(luck > 0)
        {
            txtPlayerData[6].text = Utils.GetThousandCommaText(luck) + " + " + Utils.GetThousandCommaText(1) + " = " + Utils.GetThousandCommaText(luck + 1);
        }
        else if(luck < 0)
        {
            txtPlayerData[6].text = Utils.GetThousandCommaText(luck) + " - " + Utils.GetThousandCommaText(1) + " = " + Utils.GetThousandCommaText(luck - 1);
        }
        else
        {
            txtPlayerData[6].text = Utils.GetThousandCommaText(luck) + " + " + Utils.GetThousandCommaText(0) + " = " + Utils.GetThousandCommaText(luck + 0);
        }

        if(db == ItemDb.Weapon || db == ItemDb.Armor)
        {
            enhanceRate = enforce.enforceRates.TryGetValue(current, out int successRate) ? successRate : 0;
        }
        else
        {
            enhanceRate = enforce.accelyEnforceRates.TryGetValue(current, out int successRate) ? successRate : 0;
        }

        txtPlayerData[7].text = enhanceRate.ToString();
        txtPlayerData[8].text = current.ToString();
        txtPlayerData[9].text = max.ToString();
    }
}

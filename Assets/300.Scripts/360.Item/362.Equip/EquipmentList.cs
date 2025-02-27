using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentList : MonoBehaviour
{
    public static EquipmentList instance;

    public int purchaseTotal = 0;
    public int sellTotal = 0;

    public int weaponPurchaseTotal = 0;
    public int armorPurchaseTotal = 0;
    public int accessoriesPurchaseTotal = 0;

    public int weaponSellTotal = 0;
    public int armorSellTotal = 0;
    public int accessoriesSellTotal = 0;

    public List<ItemData> equipmentWeaponPurchaseList = new List<ItemData>();

    public List<ItemData> equipmentArmorPurchaseList = new List<ItemData>();

    public List<ItemData> equipmentAccessoriesPurchaseList = new List<ItemData>();

    public List<ItemData> equipmentWeaponSellList = new List<ItemData>();

    public List<ItemData> equipmentArmorSellList = new List<ItemData>();

    public List<ItemData> equipmentAccessoriesSellList = new List<ItemData>();

    [SerializeField] Text txtPurchaseTotal;
    [SerializeField] Text txtSellTotal;

    [SerializeField] Button[] btnPurchase;

    [SerializeField] Button[] btnSell;


    [SerializeField] Sprite[] weaponSpr;

    [SerializeField] Sprite[] armorSpr;

    [SerializeField] Sprite[] accessoriesSpr;

    [SerializeField] ItemPurchaseList purchaseList;

    [SerializeField] itemSellList sellList;

    // 0 :체력, 1 : 스태미나, 2 : 공격력, 3 : 방어력 , 4 : 크리티컬 확율, 5 : 크리티컬 데미지, 6 : 운, 7 : 아이템 무게, 8 : 경험치 증가, 9 : 돈 증가
    public Text[] txtPlayerData;

    public int purchaseIndexNumber = -1;
    public int sellIndexNumber = -1;

    private void Awake()
    {
        instance = this;
        purchaseTotal = 0;
        sellTotal = 0;

        btnPurchase[0].onClick.AddListener(() => OnPurchaseOnClick(0));
        btnPurchase[1].onClick.AddListener(() => OnPurchaseOnClick(1));

        btnSell[0].onClick.AddListener(() => OnSellOnClick(0));
        btnSell[1].onClick.AddListener(() => OnSellOnClick(1));
    }

    public void PurchaseTotalDataSetting(int num)
    {
        switch(num)
        {
            case 0:
                purchaseTotal = weaponPurchaseTotal;
                break;
            case 1:
                purchaseTotal = armorPurchaseTotal;
                break;
            case 2:
                purchaseTotal = accessoriesPurchaseTotal;
                break;
        }

        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtPurchaseTotal.text = "총 구매 금액은 :  " + Utils.GetThousandCommaText(purchaseTotal) + " 원 입니다.";
                break;
            case LANGUAGE.ENG:
                txtPurchaseTotal.text = "total purchase amount :  " + Utils.GetThousandCommaText(purchaseTotal) + " is won.";
                break;
        }
    }

    public void SellTotalDataSetting(int num)
    {
        switch (num)
        {
            case 0:
                sellTotal = weaponSellTotal;
                break;
            case 1:
                sellTotal = armorSellTotal;
                break;
            case 2:
                sellTotal = accessoriesSellTotal;
                break;
        }


        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtSellTotal.text = "총 판매 금액은 :  " + Utils.GetThousandCommaText(sellTotal) + " 원 입니다.";
                break;
            case LANGUAGE.ENG:
                txtSellTotal.text = "total sell amount :  " + Utils.GetThousandCommaText(sellTotal) + " is won.";
                break;
        }
    }
    //무기 아이템 구매
    public void OnWeaponPurchaseData()
    {
        equipmentWeaponPurchaseList.Clear();
        var WeaponPurchaseContents = new ItemData[]
        {
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "brokensword",
                nameKor = "부려진검",                
                purchase = 140,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 2,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = weaponSpr[0],
                Count = 0,               
                totalData = 0,
                itemEnforce = 0, //현재 강화
                itemMaxEnforce = 9, //최대 강화할수 있는,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("부려진검"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "RustyDagger",
                nameKor = "녹슨대거",              
                purchase = 360,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 3,
                crictleDmgUp = 4,
                crictleRateUp = 2,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,               
                itemSpr = weaponSpr[1],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("녹슨대거"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "Dagger",
                nameKor = "대거",             
                purchase = 600,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 5,
                crictleDmgUp = 4,
                crictleRateUp = 4,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,              
                itemSpr = weaponSpr[2],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("대거"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "IronDagger",
                nameKor = "철대거",
                purchase = 1000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 8,
                crictleDmgUp = 7,
                crictleRateUp = 4,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,             
                itemSpr = weaponSpr[3],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("철대거"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "scimitar",
                nameKor = "시미터",
                purchase = 1400,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 10,
                crictleDmgUp = 15,
                crictleRateUp = 6,
                expUp = 0,
                moneyUp = 0,
                lukUp = 4,             
                itemSpr = weaponSpr[4],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("시미터"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "IronSword",
                nameKor = "철검",
                purchase = 1200,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 14,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,              
                itemSpr = weaponSpr[5],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("철검"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "SteelSword",
                nameKor = "강철검",
                purchase = 1700,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 20,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,              
                itemSpr = weaponSpr[6],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("강철검"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "Knight's Sword",
                nameKor = "기사검",
                purchase = 3000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 32,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,              
                itemSpr = weaponSpr[7],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("기사검"),
            },
            new ItemData()
            {
                db = ItemDb.Weapon,
                name = "Flamberge",
                nameKor = "플랑베르주",
                purchase = 5000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 30,
                crictleDmgUp = 12,
                crictleRateUp = 6,
                expUp = 0,
                moneyUp = 0,
                lukUp = 6,          
                itemSpr = weaponSpr[8],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9,
                haveCount = PlayerManager.instance.player.inventory.GetItemCountByName("플랑베르주"),
            },
        };

        if(equipmentWeaponPurchaseList.Count == 0)
        {
            equipmentWeaponPurchaseList.AddRange(WeaponPurchaseContents);
        }
        purchaseList.itemWeaponPurchaseLoadList();
    }
    //갑옷 아이템 구매
    public void OnArmorPurchaseData()
    {
        equipmentArmorPurchaseList.Clear();
        var ArmorPurchaseContnets = new ItemData[]
        {
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "ClothClothes",
                nameKor = "천옷",
                purchase = 200,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 2,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = 1,              
                itemSpr = armorSpr[0],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "Cloak",
                nameKor = "천망토",
                purchase = 150,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 1,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = 1,               
                itemSpr = armorSpr[1],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "ClothHelmet",
                nameKor = "천헬멧",
                purchase = 150,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 1,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = 1,               
                itemSpr = armorSpr[2],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "ClothGloves",
                nameKor = "천장갑",
                purchase = 150,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 1,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = 1,             
                itemSpr = armorSpr[3],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "ColthShoes",
                nameKor = "천신발",
                purchase = 150,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 1,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = 1,              
                itemSpr = armorSpr[4],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "ColthPants",
                nameKor = "천바지",
                purchase = 150,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 1,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = 1,                
                itemSpr = armorSpr[5],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "LeatherClothes",
                nameKor = "가죽옷",
                purchase = 500,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 6,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,               
                itemSpr = armorSpr[6],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "LeatherCloak",
                nameKor = "가죽망토",
                purchase = 350,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 4,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,                
                itemSpr = armorSpr[7],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "LeatherHelmet",
                nameKor = "가죽헬멧",
                purchase = 350,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 4,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,               
                itemSpr = armorSpr[8],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "LeatherGloves",
                nameKor = "가죽장갑",
                purchase = 350,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 4,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,                
                itemSpr = armorSpr[9],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "LeatherShoes",
                nameKor = "가죽신발",
                purchase = 350,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 4,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,               
                itemSpr = armorSpr[10],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "LeatherPants",
                nameKor = "가죽바지",
                purchase = 350,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 4,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,              
                itemSpr = armorSpr[11],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "IronArmor",
                nameKor = "철갑옷",
                purchase = 1000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 12,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = -0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = -1,               
                itemSpr = armorSpr[12],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "IronCloak",
                nameKor = "철망토",
                purchase = 800,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 6,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = -0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = -1,                
                itemSpr = armorSpr[13],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "IronHelmet",
                nameKor = "철헬멧",
                purchase = 800,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 6,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = -0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = -1,               
                itemSpr = armorSpr[14],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "IronGloves",
                nameKor = "철장갑",
                purchase = 800,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 6,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = -0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = -1,                
                itemSpr = armorSpr[15],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "IronBoots",
                nameKor = "철부츠",
                purchase = 800,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 6,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = -0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = -1,               
                itemSpr = armorSpr[16],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
            new ItemData()
            {
                db = ItemDb.Armor,
                name = "IronPants",
                nameKor = "철바지",
                purchase = 800,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 6,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = -0.2f,
                expUp = 0,
                moneyUp = 0,
                lukUp = -1,              
                itemSpr = armorSpr[17],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                itemMaxEnforce = 9
            },
        };

        if(equipmentArmorPurchaseList.Count == 0)
        {
            equipmentArmorPurchaseList.AddRange(ArmorPurchaseContnets);
        }
        purchaseList.itemArmorPurchaseLoadList();
    }
    //액서세리 아이템 구매
    public void OnAccessoriesPurchaseData()
    {
        equipmentAccessoriesPurchaseList.Clear();
        var AccessoriesPurchaseContents = new ItemData[]
        {
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "HpIncreaseRing",
                nameKor = "체력증가반지",
                purchase = 1000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[0],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 100,
                StaminaUp = 0,
                itemMaxEnforce = 3,                
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "StaminaIncreaseRing",
                nameKor = "스태미나증가반지",
                purchase = 1000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[1],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,                
                StaminaUp = 100,
                itemMaxEnforce = 3
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "AttackIncreaseRing",
                nameKor = "공격력증가반지",
                purchase = 2000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 10,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[2],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,
                StaminaUp = 0,                
                itemMaxEnforce = 3
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "CriticalIncreaseRing",
                nameKor =  "크리티컬증가반지",
                purchase = 3000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 15,
                crictleRateUp = 5,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[3],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,              
                StaminaUp = 0,
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "ExpIncreaseRing",
                nameKor =  "경험치증가반지",
                purchase = 5000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 50,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[4],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,              
                StaminaUp = 0,
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "DefenseIncreaseNecklace",
                nameKor =  "방어력증가목걸이",
                purchase = 2000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 10,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[5],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,
                StaminaUp = 0,                
                itemMaxEnforce = 3
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "Attack and defense increase necklace",
                nameKor =  "공방증가목걸이",
                purchase = 6000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 10,
                attackUp = 10,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 0,
                lukUp = 0,
                itemSpr = accessoriesSpr[6],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,
                StaminaUp = 0,              
                itemMaxEnforce = 3
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "Gold quantity increase necklace",
                nameKor =  "골드량증가목걸이",
                purchase = 5000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 0,
                moneyUp = 50,
                lukUp = 0,
                itemSpr = accessoriesSpr[7],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,              
                StaminaUp = 0
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "Luk increase necklace",
                nameKor =  "운증가목걸이",
                purchase = 7000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 50,
                crictleRateUp = 5,
                expUp = 0,
                moneyUp = 0,
                lukUp = 10,
                itemSpr = accessoriesSpr[8],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,
                StaminaUp = 0,               
                itemMaxEnforce = 3
            },
            new ItemData()
            {
                db = ItemDb.Accely,
                name = "Experience gold amount increase necklace",
                nameKor =  "경험치골드량증가목걸이",
                purchase = 12000,
                hpRestoration = 0,
                stminaRestoration = 0,
                defenceUp = 0,
                attackUp = 0,
                crictleDmgUp = 0,
                crictleRateUp = 0,
                expUp = 50,
                moneyUp = 50,
                lukUp = 0,
                itemSpr = accessoriesSpr[9],
                Count = 0,
                totalData = 0,
                itemEnforce = 0,
                HpUp = 0,
                StaminaUp = 0,              
                itemMaxEnforce = 0
            },
        };

        if(equipmentAccessoriesPurchaseList.Count == 0)
        {
            equipmentAccessoriesPurchaseList.AddRange(AccessoriesPurchaseContents);
        }
        purchaseList.itemAccesoryPurchaseLoadList();
    }
    //무기 아이템 판매
    public void OnWeaponSellData()
    {
        PlayerManager.instance.player.inventory.FillerItems(equipmentWeaponSellList, 0);
        sellList.itemWeaponSellLoadList();
    }

    public void OnArmorSellData()
    {
        PlayerManager.instance.player.inventory.FillerItems(equipmentArmorSellList, 1);
        sellList.itemArmorSellLoadList();
    }

    public void OnAccelySellData()
    {
        PlayerManager.instance.player.inventory.FillerItems(equipmentAccessoriesSellList, 2);
        sellList.itemAccelySellLoadList();
    }

    public void EquipPlayerDataSetting(float hp, float stmina, float attack, float defense, float critcleRate, float critcleDmg, int luk, int expUp, int moneyUp, int maxEnforce)
    {
        txtPlayerData[0].text = Utils.GetThousandCommaText(PlayerManager.instance.player.hp) + " + " + Utils.GetThousandCommaText(hp) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.hp + hp);

        txtPlayerData[1].text = Utils.GetThousandCommaText(PlayerManager.instance.player.stamina) + " + " + Utils.GetThousandCommaText(stmina) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.stamina + stmina);

        txtPlayerData[2].text = Utils.GetThousandCommaText(PlayerManager.instance.player.attack) + " + " + Utils.GetThousandCommaText(attack) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.attack + attack);

        txtPlayerData[3].text = Utils.GetThousandCommaText(PlayerManager.instance.player.defense) + " + " + Utils.GetThousandCommaText(defense) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.defense + defense);

        txtPlayerData[4].text = Utils.GetThousandCommaText(PlayerManager.instance.player.critcleRate) + " + " + Utils.GetThousandCommaText(critcleRate) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.critcleRate + critcleRate);

        txtPlayerData[5].text = Utils.GetThousandCommaText(PlayerManager.instance.player.critcleDmg) + " + " + Utils.GetThousandCommaText(critcleDmg) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.critcleDmg + critcleDmg);

        txtPlayerData[6].text = Utils.GetThousandCommaText(PlayerManager.instance.player.luk) + " + " + Utils.GetThousandCommaText(luk) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.luk + luk);
       
        txtPlayerData[7].text = Utils.GetThousandCommaText(PlayerManager.instance.player.expUp) + " + " + Utils.GetThousandCommaText(expUp) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.expUp + expUp);

        txtPlayerData[8].text = Utils.GetThousandCommaText(PlayerManager.instance.player.moneyUp) + " + " + Utils.GetThousandCommaText(moneyUp) + " = " + Utils.GetThousandCommaText(PlayerManager.instance.player.moneyUp + moneyUp);

        txtPlayerData[9].text = maxEnforce.ToString();
    }

    public void AllDefaultPlayerDataSetting()
    {
        txtPlayerData[0].text = Utils.GetThousandCommaText(PlayerManager.instance.player.hp);

        txtPlayerData[1].text = Utils.GetThousandCommaText(PlayerManager.instance.player.stamina);

        txtPlayerData[2].text = Utils.GetThousandCommaText(PlayerManager.instance.player.attack);

        txtPlayerData[3].text = Utils.GetThousandCommaText(PlayerManager.instance.player.defense);

        txtPlayerData[4].text = Utils.GetThousandCommaText(PlayerManager.instance.player.critcleRate);

        txtPlayerData[5].text = Utils.GetThousandCommaText(PlayerManager.instance.player.critcleDmg);

        txtPlayerData[6].text = Utils.GetThousandCommaText(PlayerManager.instance.player.luk);
        
        txtPlayerData[7].text = Utils.GetThousandCommaText(PlayerManager.instance.player.expUp);

        txtPlayerData[8].text = Utils.GetThousandCommaText(PlayerManager.instance.player.moneyUp);

        txtPlayerData[9].text = "";
    }
    

    void OnPurchaseOnClick(int num)
    {
        switch(num)
        {
            case 0:
                if(PlayerManager.instance.player.money >= purchaseTotal)
                {
                    switch(PlayerManager.instance.itemShopNum)
                    {
                        case 0:                         
                            for(int i = 0; i < equipmentWeaponPurchaseList.Count; i++)
                            {
                                if (equipmentWeaponPurchaseList[i].Count > 0)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("brokensword", "부려진검", ItemDb.Weapon,DataDb.Weapon, 0, 0, 0, 2, 0, 0, 0, 0, 0, equipmentWeaponPurchaseList[0].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[0].purchase);
                                            break;
                                        case 1:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("RustyDagger", "녹슨대거", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 3, 4, 2, 0,0, 0, equipmentWeaponPurchaseList[1].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[1].purchase);
                                            break;
                                        case 2:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Dagger", "대거", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 5, 4, 4, 0, 0, 0, equipmentWeaponPurchaseList[2].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[2].purchase);
                                            break;
                                        case 3:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronDagger", "철대거", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 8, 7, 4, 0, 0, 0, equipmentWeaponPurchaseList[3].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[3].purchase);
                                            break;
                                        case 4:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("scimitar", "시미터", ItemDb.Weapon, DataDb.Weapon, 4, 4, 4, 10, 15, 6, 4, 4, 4, equipmentWeaponPurchaseList[4].Count, 4, 9, 4, 4, equipmentWeaponPurchaseList[4].purchase);
                                            break;
                                        case 5:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronSword", "철검", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 14, 0, 0, 0, 0, 0, equipmentWeaponPurchaseList[5].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[5].purchase);
                                            break;
                                        case 6:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("SteelSword", "강철검", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 20, 0, 0, 0, 0, 0, equipmentWeaponPurchaseList[6].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[6].purchase);
                                            break;
                                        case 7:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Knight's Sword", "기사검", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 32, 0, 0, 0, 0, 0, equipmentWeaponPurchaseList[7].Count, 0, 9, 0, 0, equipmentWeaponPurchaseList[7].purchase);
                                            break;
                                        case 8:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Flamberge", "플랑베르주", ItemDb.Weapon, DataDb.Weapon, 0, 0, 0, 30, 12, 6, 0, 0, 6, equipmentWeaponPurchaseList[8].Count, 0, 9, 0, 0 , equipmentWeaponPurchaseList[8].purchase);
                                            break;
                                    }
                                }
                            }
                            switch (GameManager.data.lanauge)
                            {
                                case LANGUAGE.KOR:
                                    ShopPanel.instance.ShopTextSetting("구매 성공", "총 " + Utils.GetThousandCommaText(purchaseTotal) + " 원이 결제 되었습니다.");
                                    break;
                                case LANGUAGE.ENG:
                                    ShopPanel.instance.ShopTextSetting("Purchase Success", "A total of " + Utils.GetThousandCommaText(purchaseTotal) + " won was paid.");
                                    break;
                            }
                            PlayerManager.instance.player.money -= purchaseTotal;
                            GameCanvas.instance.MoneySetting();
                            weaponPurchaseTotal = 0;
                            PurchaseTotalDataSetting(0);
                            for (int i = 0; i < equipmentWeaponPurchaseList.Count; i++)
                            {
                                equipmentWeaponPurchaseList[i].Count = 0;
                                equipmentWeaponPurchaseList[i].totalData = 0;
                            }
                            OnWeaponPurchaseData();
                            break;
                        case 1:
                            for (int i = 0; i < equipmentArmorPurchaseList.Count; i++)
                            {
                                if (equipmentArmorPurchaseList[i].Count > 0)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("ClothClothes", "천옷", ItemDb.Armor, DataDb.Cheast, 0, 0, 2, 0, 0, 0.2f, 0, 0, 1, equipmentArmorPurchaseList[0].Count, 0, 9, 0, 0 , equipmentArmorPurchaseList[0].purchase);
                                            break;
                                        case 1:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Cloak", "천망토", ItemDb.Armor, DataDb.Cloack, 0, 0, 1, 0, 0, 0.2f, 0, 0, 1, equipmentArmorPurchaseList[1].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[1].purchase);
                                            break;
                                        case 2:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("ClothHelmet", "천헬멧", ItemDb.Armor, DataDb.Head, 0, 0, 1, 0, 0, 0.2f, 0, 0, 1, equipmentArmorPurchaseList[2].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[2].purchase);
                                            break;
                                        case 3:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("ClothGloves", "천장갑", ItemDb.Armor, DataDb.Hand, 0, 0, 1, 0, 0, 0.2f, 0, 0, 1, equipmentArmorPurchaseList[3].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[3].purchase);
                                            break;
                                        case 4:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("ColthShoes", "천신발", ItemDb.Armor, DataDb.Boots, 0, 0, 1, 0, 0, 0.2f, 0, 0, 1, equipmentArmorPurchaseList[4].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[4].purchase);
                                            break;
                                        case 5:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("ColthPants", "천바지", ItemDb.Armor, DataDb.Pants, 0, 0, 1, 0, 0, 0.2f, 0, 0, 1, equipmentArmorPurchaseList[5].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[5].purchase);
                                            break;
                                        case 6:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("LeatherClothes", "가죽옷", ItemDb.Armor, DataDb.Cheast, 0, 0, 6, 0, 0, 0, 0, 0, 0, equipmentArmorPurchaseList[6].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[6].purchase);
                                            break;
                                        case 7:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("LeatherCloak", "가죽망토", ItemDb.Armor, DataDb.Cloack, 0, 0, 4, 0, 0, 0, 0, 0, 0, equipmentArmorPurchaseList[7].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[7].purchase);
                                            break;
                                        case 8:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("LeatherHelmet", "가죽헬멧", ItemDb.Armor, DataDb.Head, 0, 0, 4, 0, 0, 0, 0, 0, 0, equipmentArmorPurchaseList[8].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[8].purchase);
                                            break;
                                        case 9:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("LeatherGloves", "가죽장갑", ItemDb.Armor, DataDb.Hand, 0, 0, 4, 0, 0, 0, 0, 0, 0, equipmentArmorPurchaseList[9].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[9].purchase);
                                            break;
                                        case 10:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("LeatherShoes", "가죽신발", ItemDb.Armor, DataDb.Boots, 0, 0, 4, 0, 0, 0, 0, 0, 0, equipmentArmorPurchaseList[10].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[10].purchase);
                                            break;
                                        case 11:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("LeatherPants", "가죽바지", ItemDb.Armor, DataDb.Pants, 0, 0, 4, 0, 0, 0, 0, 0, 0, equipmentArmorPurchaseList[11].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[11].purchase);
                                            break;
                                        case 12:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronArmor", "철갑옷", ItemDb.Armor, DataDb.Cheast, 0, 0, 12, 0, 0, -0.2f, 0, 0, -1, equipmentArmorPurchaseList[12].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[12].purchase);
                                            break;
                                        case 13:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronCloak", "철망토", ItemDb.Armor, DataDb.Cloack, 0, 0, 6, 0, 0, -0.2f, 0, 0, -1, equipmentArmorPurchaseList[13].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[13].purchase);
                                            break;
                                        case 14:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronHelmet", "철헬멧", ItemDb.Armor, DataDb.Head, 0, 0, 6, 0, 0, -0.2f, 0, 0, -1, equipmentArmorPurchaseList[14].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[14].purchase);
                                            break;
                                        case 15:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronGloves", "철장갑", ItemDb.Armor, DataDb.Hand, 0, 0, 6, 0, 0, -0.2f, 0, 0, -1, equipmentArmorPurchaseList[15].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[15].purchase);
                                            break;
                                        case 16:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronBoots", "철부츠", ItemDb.Armor, DataDb.Boots, 0, 0, 6, 0, 0, -0.2f, 0, 0, -1, equipmentArmorPurchaseList[16].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[16].purchase);
                                            break;
                                        case 17:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("IronPants", "철바지", ItemDb.Armor, DataDb.Pants, 0, 0, 6, 0, 0, -0.2f, 0, 0, -1, equipmentArmorPurchaseList[17].Count, 0, 9, 0, 0, equipmentArmorPurchaseList[17].purchase);
                                            break;
                                    }
                                }
                            }
                            switch (GameManager.data.lanauge)
                            {
                                case LANGUAGE.KOR:
                                    ShopPanel.instance.ShopTextSetting("구매 성공", "총 " + Utils.GetThousandCommaText(purchaseTotal) + " 원이 결제 되었습니다.");
                                    break;
                                case LANGUAGE.ENG:
                                    ShopPanel.instance.ShopTextSetting("Purchase Success", "A total of " + Utils.GetThousandCommaText(purchaseTotal) + " won was paid.");
                                    break;
                            }
                            PlayerManager.instance.player.money -= purchaseTotal;
                            GameCanvas.instance.MoneySetting();
                            armorPurchaseTotal = 0;
                            PurchaseTotalDataSetting(1);
                            for (int i = 0; i < equipmentArmorPurchaseList.Count; i++)
                            {
                                equipmentArmorPurchaseList[i].Count = 0;
                                equipmentArmorPurchaseList[i].totalData = 0;
                            }
                            OnArmorPurchaseData();
                            break;
                        case 2:
                            for (int i = 0; i < equipmentAccessoriesPurchaseList.Count; i++)
                            {
                                if (equipmentAccessoriesPurchaseList[i].Count > 0)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("HpIncreaseRing", "체력증가반지", ItemDb.Accely, DataDb.Ring, 0, 0, 0, 0, 0, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[0].Count, 0, 3, 100, 0, equipmentAccessoriesPurchaseList[0].purchase);
                                            break;
                                        case 1:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("StaminaIncreaseRing", "스태미나증가반지", ItemDb.Accely, DataDb.Ring, 0, 0, 0, 0, 0, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[1].Count, 0, 3, 0, 100, equipmentAccessoriesPurchaseList[1].purchase);
                                            break;
                                        case 2:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("AttackIncreaseRing", "공격력증가반지", ItemDb.Accely, DataDb.Ring, 0, 0, 0, 10, 0, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[2].Count, 0, 3, 0, 0, equipmentAccessoriesPurchaseList[2].purchase);
                                            break;
                                        case 3:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("CriticalIncreaseRing", "크리티컬증가반지", ItemDb.Accely, DataDb.Ring, 0, 0, 0, 0, 15, 5, 0, 0, 0, equipmentAccessoriesPurchaseList[3].Count, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[3].purchase);
                                            break;
                                        case 4:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("ExpIncreaseRing", "경험치증가반지", ItemDb.Accely, DataDb.Ring, 0, 0, 0, 0, 0, 0, 50, 0, 0, equipmentAccessoriesPurchaseList[4].Count, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[4].purchase);
                                            break;
                                        case 5:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("DefenseIncreaseNecklace", "방어력증가목걸이", ItemDb.Accely, DataDb.Necklace, 0, 0, 10, 0, 0, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[5].Count, 0, 3, 0, 0, equipmentAccessoriesPurchaseList[5].purchase);
                                            break;
                                        case 6:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Attack and defense increase necklace", "공방증가목걸이", ItemDb.Accely, DataDb.Necklace,  0, 0, 10, 10, 0, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[6].Count, 0, 3, 0, 0, equipmentAccessoriesPurchaseList[6].purchase);
                                            break;
                                        case 7:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Gold quantity increase necklace", "골드량증가목걸이", ItemDb.Accely, DataDb.Necklace, 0, 0, 0, 0, 0, 0, 0, 50, 0, equipmentAccessoriesPurchaseList[7].Count, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[7].purchase);
                                            break;
                                        case 8:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Luk increase necklace", "운증가목걸이", ItemDb.Accely, DataDb.Necklace,  0, 0, 0, 0, 50, 5, 0, 0, 10, equipmentAccessoriesPurchaseList[8].Count, 0, 3, 0, 0, equipmentAccessoriesPurchaseList[8].purchase);
                                            break;
                                        case 9:
                                            PlayerManager.instance.player.inventory.AddNewItemToInventory("Experience gold amount increase necklace", "경험치골드량증가목걸이", ItemDb.Accely, DataDb.Necklace, 0, 0, 0, 0, 0, 0, 50, 50, 0, equipmentAccessoriesPurchaseList[9].Count, 0, 0, 0, 0, equipmentAccessoriesPurchaseList[9].purchase);
                                            break;
                                    }
                                }
                            }
                            switch (GameManager.data.lanauge)
                            {
                                case LANGUAGE.KOR:
                                    ShopPanel.instance.ShopTextSetting("구매 성공", "총 " + Utils.GetThousandCommaText(purchaseTotal) + " 원이 결제 되었습니다.");
                                    break;
                                case LANGUAGE.ENG:
                                    ShopPanel.instance.ShopTextSetting("Purchase Success", "A total of " + Utils.GetThousandCommaText(purchaseTotal) + " won was paid.");
                                    break;
                            }
                            PlayerManager.instance.player.money -= purchaseTotal;
                            GameCanvas.instance.MoneySetting();
                            accessoriesPurchaseTotal = 0;
                            PurchaseTotalDataSetting(2);
                            for (int i = 0; i < equipmentAccessoriesPurchaseList.Count; i++)
                            {
                                equipmentAccessoriesPurchaseList[i].Count = 0;
                                equipmentAccessoriesPurchaseList[i].totalData = 0;
                            }
                            OnAccessoriesPurchaseData();
                            break;
                    }
                    txtPurchaseTotal.text = "";
                }
                else
                {
                    switch (GameManager.data.lanauge)
                    {
                        case LANGUAGE.KOR:
                            ShopPanel.instance.ShopTextSetting("구매 실패", "돈이 " + Utils.GetThousandCommaText(purchaseTotal - PlayerManager.instance.player.money) + "원 부족합니다.");
                            break;
                        case LANGUAGE.ENG:
                            ShopPanel.instance.ShopTextSetting("Purchase Failed", "I'm " + Utils.GetThousandCommaText(purchaseTotal - PlayerManager.instance.player.money) + " won short");
                            break;
                    }
                }
                break;
            case 1:
                //구매 취소           
                switch(PlayerManager.instance.itemShopNum)
                {
                    case 0:
                        weaponPurchaseTotal = 0;
                        for(int i = 0;i < equipmentWeaponPurchaseList.Count; i++)
                        {
                            equipmentWeaponPurchaseList[i].Count = 0;
                            equipmentWeaponPurchaseList[i].totalData = 0;
                        }
                        purchaseList.itemWeaponPurchaseLoadList();
                        break;
                    case 1:
                        armorPurchaseTotal = 0;
                        for (int i = 0; i < equipmentArmorPurchaseList.Count; i++)
                        {
                            equipmentArmorPurchaseList[i].Count = 0;
                            equipmentArmorPurchaseList[i].totalData = 0;
                        }
                        purchaseList.itemArmorPurchaseLoadList();
                        break;
                    case 2:
                        accessoriesPurchaseTotal = 0;
                        for (int i = 0; i < equipmentAccessoriesPurchaseList.Count; i++)
                        {
                            equipmentAccessoriesPurchaseList[i].Count = 0;
                            equipmentAccessoriesPurchaseList[i].totalData = 0;
                        }
                        purchaseList.itemAccesoryPurchaseLoadList();
                        break;
                }
                PurchaseTotalDataSetting(PlayerManager.instance.itemShopNum);
                purchaseList.AllDefaultColorChange();
                txtPurchaseTotal.text = "";
                break;
        }
    }


    void OnSellOnClick(int num)
    {
        switch(num)
        {
            case 0: //판매 성공
                switch(PlayerManager.instance.itemShopNum)
                {
                    case 0:
                        PlayerManager.instance.player.money += weaponSellTotal;
                        for(int i = 0; i < equipmentWeaponSellList.Count; i++)
                        {
                            if (equipmentWeaponSellList[i].Count > 0)
                            {
                                PlayerManager.instance.player.inventory.RemoveItemByName(equipmentWeaponSellList[i].nameKor, equipmentWeaponSellList[i].Count);
                            }
                        }

                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                ShopPanel.instance.ShopTextSetting("판매 성공", "총 " + Utils.GetThousandCommaText(weaponSellTotal) + " 원이 판매 되었습니다.");
                                break;
                            case LANGUAGE.ENG:
                                ShopPanel.instance.ShopTextSetting("Sales Success", "A total of " + Utils.GetThousandCommaText(weaponSellTotal) + " won was sold.");
                                break;
                        }

                        GameCanvas.instance.MoneySetting();
                        weaponSellTotal = 0;
                        for (int i = 0; i < equipmentWeaponSellList.Count; i++)
                        {
                            equipmentWeaponSellList[i].Count = 0;
                            equipmentWeaponSellList[i].totalData = 0;
                        }
                        OnWeaponSellData();
                        break;
                    case 1:
                        PlayerManager.instance.player.money += armorSellTotal;
                        for (int i = 0; i < equipmentArmorSellList.Count; i++)
                        {
                            if (equipmentArmorSellList[i].Count > 0)
                            {
                                PlayerManager.instance.player.inventory.RemoveItemByName(equipmentArmorSellList[i].nameKor, equipmentArmorSellList[i].Count);
                            }
                        }

                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                ShopPanel.instance.ShopTextSetting("판매 성공", "총 " + Utils.GetThousandCommaText(armorSellTotal) + " 원이 판매 되었습니다.");
                                break;
                            case LANGUAGE.ENG:
                                ShopPanel.instance.ShopTextSetting("Sales Success", "A total of " + Utils.GetThousandCommaText(armorSellTotal) + " won was sold.");
                                break;
                        }

                        GameCanvas.instance.MoneySetting();
                        armorSellTotal = 0;
                        for (int i = 0; i < equipmentArmorSellList.Count; i++)
                        {
                            equipmentArmorSellList[i].Count = 0;
                            equipmentArmorSellList[i].totalData = 0;
                        }
                        OnArmorSellData();
                        break;
                    case 2:
                        PlayerManager.instance.player.money += accessoriesSellTotal;
                        for (int i = 0; i < equipmentAccessoriesSellList.Count; i++)
                        {
                            if (equipmentAccessoriesSellList[i].Count > 0)
                            {
                                PlayerManager.instance.player.inventory.RemoveItemByName(equipmentAccessoriesSellList[i].nameKor, equipmentAccessoriesSellList[i].Count);
                            }
                        }

                        switch (GameManager.data.lanauge)
                        {
                            case LANGUAGE.KOR:
                                ShopPanel.instance.ShopTextSetting("판매 성공", "총 " + Utils.GetThousandCommaText(accessoriesSellTotal) + " 원이 판매 되었습니다.");
                                break;
                            case LANGUAGE.ENG:
                                ShopPanel.instance.ShopTextSetting("Sales Success", "A total of " + Utils.GetThousandCommaText(accessoriesSellTotal) + " won was sold.");
                                break;
                        }

                        GameCanvas.instance.MoneySetting();
                        accessoriesSellTotal = 0;
                        for (int i = 0; i < equipmentAccessoriesSellList.Count; i++)
                        {
                            equipmentAccessoriesSellList[i].Count = 0;
                            equipmentAccessoriesSellList[i].totalData = 0;
                        }
                        OnAccelySellData();
                        break;
                }
                txtSellTotal.text = "";
                break;
            case 1: //판매 취소
                switch(PlayerManager.instance.itemShopNum)
                {
                    case 0:
                        weaponSellTotal = 0;
                        for (int i = 0; i < equipmentWeaponSellList.Count; i++)
                        {
                            equipmentWeaponSellList[i].Count = 0;
                            equipmentWeaponSellList[i].totalData = 0;
                        }
                        sellList.itemWeaponSellLoadList();
                        break;
                    case 1:
                        armorSellTotal = 0;
                        for (int i = 0; i < equipmentArmorSellList.Count; i++)
                        {
                            equipmentArmorSellList[i].Count = 0;
                            equipmentArmorSellList[i].totalData = 0;
                        }
                        sellList.itemArmorSellLoadList();
                        break;
                    case 2:
                        accessoriesSellTotal = 0;
                        for (int i = 0; i < equipmentAccessoriesSellList.Count; i++)
                        {
                            equipmentAccessoriesSellList[i].Count = 0;
                            equipmentAccessoriesSellList[i].totalData = 0;
                        }
                        sellList.itemAccelySellLoadList();
                        break;
                }
                txtSellTotal.text = "";
                break;
        }
    }
}

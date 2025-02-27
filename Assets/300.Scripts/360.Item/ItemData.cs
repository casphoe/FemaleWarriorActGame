using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemDb
{
    Weapon,Armor,Accely,HpPotion,StaminaPotion,None
}

public enum Equip
{
    None,Equip
}

public enum DataDb
{
    Head,Necklace,Cloack,Cheast,Hand,Ring,Pants,Weapon,Boots,None
}

[Serializable]
public class ItemData 
{
    public Guid id;

    public ItemDb db;

    public Equip equip;

    public DataDb dataDb;

    //구매 가격
    public int purchase;

    public Sprite itemSpr;

    //아이템 이름
    public string name;

    public string nameKor;

    //아이템 설명
    public string description;

    public string descriptionKor;

    public int Count;

    public int totalData;

    //체력포션 효과
    public float hpRestoration;

    //스태미나 포션 효과
    public float stminaRestoration;

    //방어력 up
    public float defenceUp;

    //공격력 up
    public float attackUp;

    //크리티컬 데미지 up
    public float crictleDmgUp;

    //크리티컬 확률 up
    public float crictleRateUp;

    //경험치 up
    public int expUp;

    //돈 up
    public int moneyUp;

    //운 up
    public int lukUp;

    //가지고 잇는 개수
    public int haveCount;

    //아이템 강화(몇강인지)
    public int itemEnforce;

    //아이템 최대 강화(몇강이 최대인지)
    public int itemMaxEnforce;

    //체력 증가
    public float HpUp;

    //스태미나 증가
    public float StaminaUp;

    public int equippedCount; //장착된 개수

    public int unequippedCount;

    public void UpdateEquippCount()
    {
        unequippedCount = haveCount - equippedCount;
    }

    public object Clone()
    {
        ItemData clone = (ItemData)this.MemberwiseClone(); // 얕은 복사
        // 추가적으로 참조 타입 필드를 복사할 경우 아래와 같이 복사할 수 있습니다
        // clone.someList = new List<SomeObject>(this.someList);
        return clone;
    }


    public void ItemSetting(string nameEng,string _name,ItemDb _db,DataDb _itemParticular, float _hpRestorationUp, float stminaUp, float defenceUp, float attackUp,float cdUp, float crUp, int expUp, int moneyUp, int lukUp, int haveCount, int currentEnforce, int maxEnforce, float hpUp, float stUp, int productPrice)
    {
        this.id = Guid.NewGuid(); // 아이템 고유번호 설정
        this.name = nameEng;
        this.nameKor = _name;
        this.db = _db;      
        this.hpRestoration = _hpRestorationUp;
        this.stminaRestoration = stminaUp;
        this.defenceUp = defenceUp;
        this.attackUp = attackUp;
        this.crictleDmgUp = cdUp;
        this.crictleRateUp = crUp;
        this.expUp = expUp;
        this.moneyUp = moneyUp;
        this.lukUp = lukUp;       
        this.haveCount = haveCount;
        this.itemEnforce = currentEnforce;
        this.itemMaxEnforce = maxEnforce;
        this.HpUp = hpUp;
        this.StaminaUp = stUp;
        this.purchase = productPrice;
        this.dataDb = _itemParticular;
        UpdateEquippCount();
    }
    #region 강화

    // 강화 비용 계산
    public int GetReinforcementCost()
    {
        // 강화 등급에 따라 20%씩 가격 증가
        return Mathf.CeilToInt(this.purchase * (1 + (0.2f * this.itemEnforce)));
    }
    #endregion 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EquipmentPanel : MonoBehaviour
{
    //0 : 머리,1: 목걸이, 2 : 망토, 3 : 갑옷, 4 : 손, 5 : 반지, 6 : 바지, 7 : 무기, 8 : 신발
    [SerializeField] Button[] btnSelectPanel;
    //0 : 장착,  모두 해제
    [SerializeField] Button[] btnEquip;
    [Header("장비 장착 이미지")]
    //장비 장착 이미지
    public Image[] imgEquip;
    [SerializeField] Button[] btnEquipSelect;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject equipContent;
    //0 : hp , 1 : 스태미나, 2 : 공격력, 3 : defence, 4 : 크리티컬 확률, 5 : 크리티컬 데이미, 6 : 운, 7 : 경험치, 8 : 돈
    [SerializeField] Text[] txtStat;

    public float blinkDuration = 0.5f; //깜빡임 주기

    private Coroutine currentEquipCorutine;
    private Coroutine currentEquipSelectCorutine;
    private Coroutine currentEquip;

    public static EquipmentPanel instance;

    public int selectEquipIndex = -1;
    public int selectIndex = -1;
    public int selectEquip = -1;

    public bool[] isSelectOne;
    public bool[] isSelectTwo;
    public int[] selectTwoIndex;

    public string[] equipStr;

    public List<ItemData> equipItemList = new List<ItemData>();

    private string nameStr = string.Empty;

    private Sprite itemSpr;

    private bool isStartEquip = false;

    private void Awake()
    {
        PlayerManager.instance.isEquipment = false;
        instance = this;
        selectEquipIndex = 0;
        selectIndex = 0;
        selectEquip = 0;
        isStartEquip = false;
        Utils.OnOff(panel, false);

        for(int i = 0; i < isSelectOne.Length; i++)
        {
            isSelectOne[i] = false;
        }

        for (int i = 0; i < isSelectTwo.Length; i++)
        {
            isSelectTwo[i] = false;
            selectTwoIndex[i] = 0;
        }

        for(int i = 0; i < imgEquip.Length; i++)
        {
            Utils.OnOff(imgEquip[i].gameObject, false);
        }

        btnEquip[0].onClick.AddListener(() => OnEquipBtnClickEvent(0));
        btnEquip[1].onClick.AddListener(() => OnEquipBtnClickEvent(1));
    }

    private void Start()
    {
        LoadEquippedItemsFromData();
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false)
        {
            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Equipment))
            {
                PlayerManager.instance.isEquipment = !PlayerManager.instance.isEquipment;
                if(PlayerManager.instance.isEquipment == true)
                {
                    Utils.OnOff(panel, true);
                    isStartEquip = true;
                }
                else
                {
                    Utils.OnOff(panel, false);
                    isStartEquip = false;
                }
            }

            if (PlayerManager.instance.isEquipment == true)
            {
                EquipPanelSetting();
                if(isStartEquip)
                {
                    StartEquipTextSetting();
                }
            }
        }
    }

    void StartEquipTextSetting()
    {
        txtStat[0].text = PlayerManager.instance.player.hp.ToString();

        txtStat[1].text = PlayerManager.instance.player.stamina.ToString();

        txtStat[2].text = PlayerManager.instance.player.attack.ToString();

        txtStat[3].text = PlayerManager.instance.player.defence.ToString();

        txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString();

        txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString();

        txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString();

        txtStat[7].text = PlayerManager.instance.player.expUp.ToString();

        txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString();

        isStartEquip = false;
    }

    void EquipPanelSetting()
    {
        if (isSelectOne[0] == false && isSelectOne[1] == false)
        {
            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Left))
            {
                if (selectIndex > 0)
                {
                    selectIndex -= 1;
                    OnImageEquipBtnClickEvent(selectIndex);
                }
            }

            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Right))
            {
                if (selectIndex < btnEquip.Length - 1)
                {
                    selectIndex += 1;
                    OnImageEquipBtnClickEvent(selectIndex);
                }
            }

            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
            {
                StopAllBinking(btnEquip, 0);
                isSelectOne[selectIndex] = true;
                if (isSelectOne[0] == true)
                {
                    OnImageEquipSelectBtnClickEvent(selectEquipIndex);
                }
                else if (isSelectOne[1] == true)
                {
                    //전부 장착된거 제거 해야함
                    for (int i = 0; i < btnSelectPanel.Length; i++)
                    {
                        Utils.OnOff(btnSelectPanel[i].transform.GetChild(0).gameObject, false);
                        btnSelectPanel[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    }
                }
            }
        }
        else
        {
            if (selectTwoIndex[0] == 0 && selectTwoIndex[1] == 0 && selectTwoIndex[2] == 0 && selectTwoIndex[3] == 0 && selectTwoIndex[4] == 0 && selectTwoIndex[5] == 0 && selectTwoIndex[6] == 0 && selectTwoIndex[7] == 0 && selectTwoIndex[8] == 0)
            {
                if (isSelectOne[0] == true)
                {
                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Up))
                    {
                        if (selectEquipIndex > 0)
                        {
                            selectEquipIndex -= 1;
                            OnImageEquipSelectBtnClickEvent(selectEquipIndex);
                        }
                    }

                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Down))
                    {
                        if (selectEquipIndex < btnSelectPanel.Length - 1)
                        {
                            selectEquipIndex += 1;
                            OnImageEquipSelectBtnClickEvent(selectEquipIndex);
                        }
                    }
                }
                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
                {
                    StopAllBinking(btnSelectPanel, 1);
                    if (selectTwoIndex[selectEquipIndex] == 0)
                    {
                        PlayerManager.instance.player.inventory.FillerParticularityItems(equipItemList, selectEquipIndex);
                        selectTwoIndex[selectEquipIndex] = 1;
                    }
                    Equipment.instance.OnEquipData();
                    isSelectTwo[selectEquipIndex] = true;
                    return;
                }
            }

            if (selectTwoIndex[0] == 1 || selectTwoIndex[1] == 1 || selectTwoIndex[2] == 1 || selectTwoIndex[3] == 1 || selectTwoIndex[4] == 1 || selectTwoIndex[5] == 1 || selectTwoIndex[6] == 1 || selectTwoIndex[7] == 1 || selectTwoIndex[8] == 1)
            {
                btnEquipSelect = new Button[equipItemList.Count];
                for (int i = 0; i < btnEquipSelect.Length; i++)
                {
                    btnEquipSelect[i] = equipContent.transform.GetChild(i).GetComponent<Button>();
                }

                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Left))
                {
                    if (selectEquip > 0)
                    {
                        selectEquip -= 1;
                        OnImageEquipButtonClickEvent(selectEquip);
                    }
                    EquipCharcterStatTextSetting(selectEquip, equipItemList[selectEquip].HpUp, equipItemList[selectEquip].StaminaUp, equipItemList[selectEquip].attackUp, equipItemList[selectEquip].defenceUp, equipItemList[selectEquip].crictleRateUp, equipItemList[selectEquip].crictleDmgUp
                    , equipItemList[selectEquip].lukUp, equipItemList[selectEquip].expUp, equipItemList[selectEquip].moneyUp);
                }

                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Right))
                {
                    if (selectEquip < btnEquipSelect.Length - 1)
                    {
                        selectEquip += 1;
                        OnImageEquipButtonClickEvent(selectEquip);
                    }
                    EquipCharcterStatTextSetting(selectEquip, equipItemList[selectEquip].HpUp, equipItemList[selectEquip].StaminaUp, equipItemList[selectEquip].attackUp, equipItemList[selectEquip].defenceUp, equipItemList[selectEquip].crictleRateUp, equipItemList[selectEquip].crictleDmgUp
                    , equipItemList[selectEquip].lukUp, equipItemList[selectEquip].expUp, equipItemList[selectEquip].moneyUp);
                }


                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
                {                 
                    if (imgEquip[selectEquipIndex].sprite == null) //미장착 상태
                    {                    
                        if (equipItemList[selectEquip].nameKor == "장착해제")
                        {
                            Debug.Log("장착해제");
                            return;
                        }
                        else
                        {                      
                            AddEquip();
                            PlayerManager.instance.player.inventory.FillerParticularityItems(equipItemList, selectEquipIndex);
                            Equipment.instance.OnEquipData();
                            SelectEquipUiChange(0);
                        }
                    }
                    else
                    {
                        if (equipItemList[selectEquip].nameKor == "장착해제")
                        {
                            //해당 아이템 장착 해제
                            RemoveEquip();
                            PlayerManager.instance.player.inventory.FillerParticularityItems(equipItemList, selectEquipIndex);
                            Equipment.instance.OnEquipData();
                            SelectEquipUiChange(0);                          
                        }
                        else
                        {
                            //선택된 아이템으로 장착되고 장착되고 있는 아이템은 장착 해제로 나옴 (기존에 있던 아이템 장착 해제 하고 선택된 아이템으로 장착)
                            RemoveEquip();
                            AddEquip();
                            PlayerManager.instance.player.inventory.FillerParticularityItems(equipItemList, selectEquipIndex);
                            Equipment.instance.OnEquipData();
                            SelectEquipUiChange(0);                         
                        }
                    }
                }
            }
        }

        if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Canel))
        {
            if (isSelectTwo[0] == false && isSelectTwo[1] == false && isSelectTwo[2] == false && isSelectTwo[3] == false && isSelectTwo[4] == false && isSelectTwo[5] == false && isSelectTwo[6] == false && isSelectTwo[7] == false && isSelectTwo[8] == false)
            {
                if (isSelectOne[0] == true || isSelectOne[1] == true)
                {
                    StopAllBinking(btnSelectPanel, 1);
                    OnImageEquipBtnClickEvent(selectIndex);
                    for (int i = 0; i < isSelectOne.Length; i++)
                    {
                        isSelectOne[i] = false;
                    }
                }
            }

            if (isSelectTwo[0] == true || isSelectTwo[1] == true || isSelectTwo[2] == true || isSelectTwo[3] == true || isSelectTwo[4] == true || isSelectTwo[5] == true || isSelectTwo[6] == true || isSelectTwo[7] == true || isSelectTwo[8] == true)
            {
                OnImageEquipSelectBtnClickEvent(selectEquipIndex);
                Equipment.instance.OnEquipDataClear();
                equipItemList.Clear();
                for (int i = 0; i < isSelectTwo.Length; i++)
                {
                    isSelectTwo[i] = false;
                    selectTwoIndex[i] = 0;
                }
            }
        }
    }

    //장착된 아이템 장착 해제
    void RemoveEquip()
    {
        ItemData equipItem = PlayerManager.instance.player.inventory.InventoryItemStat(equipStr[selectEquipIndex]);
        EquipCharcterStatSetting(-equipItem.HpUp, -equipItem.StaminaUp, -equipItem.attackUp, -equipItem.defenceUp, -equipItem.crictleRateUp, -equipItem.crictleDmgUp,
            -equipItem.lukUp, -equipItem.expUp, -equipItem.moneyUp);
        nameStr = GetBaseName(equipStr[selectEquipIndex]);
        Utils.OnOff(imgEquip[selectEquipIndex].gameObject, false);
        imgEquip[selectEquipIndex].preserveAspect = false;
        imgEquip[selectEquipIndex].sprite = null;      
        PlayerManager.instance.player.inventory.RemoveEquipItem(equipStr[selectEquipIndex]);
        equipStr[selectEquipIndex] = string.Empty;
        PM.playerData.equippedItemNames[selectEquipIndex] = string.Empty;
    }

    //선택된 장비 아이템 장착
    void AddEquip()
    {
        nameStr = GetBaseName(equipItemList[selectEquip].nameKor);
        equipStr[selectEquipIndex] = equipItemList[selectEquip].nameKor;
        PM.playerData.equippedItemNames[selectEquipIndex] = equipItemList[selectEquip].nameKor;
        Utils.OnOff(imgEquip[selectEquipIndex].gameObject, true);
        imgEquip[selectEquipIndex].sprite = GetItemSpriteBasedOnName(nameStr);
        imgEquip[selectEquipIndex].preserveAspect = true;
        EquipCharcterStatSetting(equipItemList[selectEquip].HpUp, equipItemList[selectEquip].StaminaUp, equipItemList[selectEquip].attackUp, equipItemList[selectEquip].defenceUp, equipItemList[selectEquip].crictleRateUp, equipItemList[selectEquip].crictleDmgUp
, equipItemList[selectEquip].lukUp, equipItemList[selectEquip].expUp, equipItemList[selectEquip].moneyUp);
        equipItemList[selectEquip].equippedCount = 1;
        equipItemList[selectEquip].equip = Equip.Equip;
        PlayerManager.instance.player.inventory.InventoryEquipItem(equipItemList[selectEquip].nameKor);
    }

    void SelectEquipUiChange(int num)
    {
        selectEquip = num;
        OnImageEquipButtonClickEvent(selectEquip);
        EquipCharcterStatTextSetting(selectEquip, equipItemList[selectEquip].HpUp, equipItemList[selectEquip].StaminaUp, equipItemList[selectEquip].attackUp, equipItemList[selectEquip].defenceUp, equipItemList[selectEquip].crictleRateUp, equipItemList[selectEquip].crictleDmgUp
                        , equipItemList[selectEquip].lukUp, equipItemList[selectEquip].expUp, equipItemList[selectEquip].moneyUp);
    }


    private string GetBaseName(string itemName)
    {
        int parenIndex = itemName.IndexOf('(');
        return parenIndex > -1 ? itemName.Substring(0, parenIndex) : itemName;
    }

    public Sprite GetItemSpriteBasedOnName(string itemName)
    {
        switch (nameStr)
        {
            case "체력포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[0];
                break;
            case "중간체력포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[1];
                break;
            case "상급체력포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[2];
                break;
            case "스태미나포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[3];
                break;
            case "중간스태미나포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[4];
                break;
            case "상급스태미나포션":
                itemSpr = InventoryPanel.instance.PotionSpriteList[5];
                break;
            case "부려진검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[0];
                break;
            case "녹슨대거":
                itemSpr = InventoryPanel.instance.weaponSpirteList[1];
                break;
            case "대거":
                itemSpr = InventoryPanel.instance.weaponSpirteList[2];
                break;
            case "철대거":
                itemSpr = InventoryPanel.instance.weaponSpirteList[3];
                break;
            case "시미터":
                itemSpr = InventoryPanel.instance.weaponSpirteList[4];
                break;
            case "철검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[5];
                break;
            case "강철검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[6];
                break;
            case "기사검":
                itemSpr = InventoryPanel.instance.weaponSpirteList[7];
                break;
            case "플랑베르주":
                itemSpr = InventoryPanel.instance.weaponSpirteList[8];
                break;
            case "천옷":
                itemSpr = InventoryPanel.instance.aromrSpriteList[0];
                break;
            case "천망토":
                itemSpr = InventoryPanel.instance.aromrSpriteList[1];
                break;
            case "천헬멧":
                itemSpr = InventoryPanel.instance.aromrSpriteList[2];
                break;
            case "천장갑":
                itemSpr = InventoryPanel.instance.aromrSpriteList[3];
                break;
            case "천신발":
                itemSpr = InventoryPanel.instance.aromrSpriteList[4];
                break;
            case "천바지":
                itemSpr = InventoryPanel.instance.aromrSpriteList[5];
                break;
            case "가죽옷":
                itemSpr = InventoryPanel.instance.aromrSpriteList[6];
                break;
            case "가죽망토":
                itemSpr = InventoryPanel.instance.aromrSpriteList[7];
                break;
            case "가죽헬멧":
                itemSpr = InventoryPanel.instance.aromrSpriteList[8];
                break;
            case "가죽장갑":
                itemSpr = InventoryPanel.instance.aromrSpriteList[9];
                break;
            case "가죽신발":
                itemSpr = InventoryPanel.instance.aromrSpriteList[10];
                break;
            case "가죽바지":
                itemSpr = InventoryPanel.instance.aromrSpriteList[11];
                break;
            case "철갑옷":
                itemSpr = InventoryPanel.instance.aromrSpriteList[12];
                break;
            case "철망토":
                itemSpr = InventoryPanel.instance.aromrSpriteList[13];
                break;
            case "철헬멧":
                itemSpr = InventoryPanel.instance.aromrSpriteList[14];
                break;
            case "철장갑":
                itemSpr = InventoryPanel.instance.aromrSpriteList[15];
                break;
            case "철부츠":
                itemSpr = InventoryPanel.instance.aromrSpriteList[16];
                break;
            case "철바지":
                itemSpr = InventoryPanel.instance.aromrSpriteList[17];
                break;
            case "체력증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[0].itemSpr;
                break;
            case "스태미나증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[1].itemSpr;
                break;
            case "공격력증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[2].itemSpr;
                break;
            case "크리티컬증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[3].itemSpr;
                break;
            case "경험치증가반지":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[4].itemSpr;
                break;
            case "방어력증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[5].itemSpr;
                break;
            case "공방증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[6].itemSpr;
                break;
            case "골드량증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[7].itemSpr;
                break;
            case "운증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[8].itemSpr;
                break;
            case "경험치골드량증가목걸이":
                itemSpr = EquipmentList.instance.equipmentAccessoriesPurchaseList[9].itemSpr;
                break;
        }

        return itemSpr;
    }

    void EquipCharcterStatSetting(float Hp, float Stamina, float attack, float defence, float critcleRate, float critcleDmg, int luk, int exp, int money)
    {
        PlayerManager.instance.player.hp += Hp;
        PlayerManager.instance.player.stamina += Stamina;
        PlayerManager.instance.player.attack += attack;
        PlayerManager.instance.player.defence += defence;
        PlayerManager.instance.player.critcleRate += critcleRate;
        PlayerManager.instance.player.critcleDmg += critcleDmg;
        PlayerManager.instance.player._stat.luk += luk;
        PlayerManager.instance.player.expUp += exp;
        PlayerManager.instance.player.money += money;

        Player.instance.currentHp += Hp;
        Player.instance.currentStamina += Stamina;
        GameCanvas.instance.SliderEquipChange();
    }

    void EquipCharcterStatTextSetting(int num,float Hp,float Stmaina, float attack, float defence, float critcleRate, float critcleDmg, int luk, int exp,int money)
    {
        if(num == 0) // 장착해제
        {
            if(equipStr[selectEquipIndex] == string.Empty)
            {
                txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.hp + Hp).ToString() + "</color>";

                txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.stamina + Stmaina).ToString() + "</color>";

                txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.attack + attack).ToString() + "</color>";

                txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.defence + defence).ToString() + "</color>";

                txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.critcleRate + critcleRate).ToString() + "</color>";

                txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.critcleDmg + critcleDmg).ToString() + "</color>";

                txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player._stat.luk + luk).ToString() + "</color>";

                txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.expUp + exp).ToString() + "</color>";

                txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.moneyUp + money).ToString() + "</color>";
            }
            else
            {
                ItemData equipItem = PlayerManager.instance.player.inventory.InventoryItemStat(equipStr[selectEquipIndex]);
                //변경점이 없으면 흰색 텍스트 변경점이 있으면 빨간색 텍스트
                if(PlayerManager.instance.player.hp - equipItem.HpUp == PlayerManager.instance.player.hp)
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.hp - equipItem.HpUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.hp - equipItem.HpUp).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.stamina - equipItem.StaminaUp == PlayerManager.instance.player.stamina)
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.stamina - equipItem.StaminaUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.stamina - equipItem.StaminaUp).ToString() + "</color>";
                }

                if(PlayerManager.instance.player.attack - equipItem.attackUp == PlayerManager.instance.player.attack)
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.attack - equipItem.attackUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.attack - equipItem.attackUp).ToString() + "</color>";
                }
                
                if(PlayerManager.instance.player.defence - equipItem.defenceUp == PlayerManager.instance.player.defence)
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.defence - equipItem.defenceUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.defence - equipItem.defenceUp).ToString() + "</color>";
                }

                if(PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp == PlayerManager.instance.player.critcleRate)
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp).ToString() + "</color>";
                }
                
                if(PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp == PlayerManager.instance.player.critcleDmg)
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp).ToString() + "</color>";
                }
                
                if(PlayerManager.instance.player._stat.luk - equipItem.lukUp == PlayerManager.instance.player._stat.luk)
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player._stat.luk - equipItem.lukUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player._stat.luk - equipItem.lukUp).ToString() + "</color>";
                }

                if(PlayerManager.instance.player.expUp - equipItem.expUp == PlayerManager.instance.player.expUp)
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.expUp - equipItem.expUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.expUp - equipItem.expUp).ToString() + "</color>";
                }

                if(PlayerManager.instance.player.moneyUp - equipItem.moneyUp == PlayerManager.instance.player.moneyUp)
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.moneyUp - equipItem.moneyUp).ToString() + "</color>";
                }
                else
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.moneyUp - equipItem.moneyUp).ToString() + "</color>";
                }             
            }
        }
        else
        {
            //변경점이 있는 텍스트는 노란색으로 표시
            if (equipStr[selectEquipIndex] == string.Empty)
            {
                if (PlayerManager.instance.player.hp + Hp > PlayerManager.instance.player.hp)
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.hp + Hp).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.hp + Hp == PlayerManager.instance.player.hp)
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.hp + Hp).ToString() + "</color>";
                }
                else
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.hp + Hp).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.stamina + Stmaina > PlayerManager.instance.player.stamina)
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.stamina + Stmaina).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.stamina + Stmaina == PlayerManager.instance.player.stamina)
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.stamina + Stmaina).ToString() + "</color>";
                }
                else
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.stamina + Stmaina).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.attack + attack > PlayerManager.instance.player.attack)
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.attack + attack).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.attack + attack == PlayerManager.instance.player.attack)
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.attack + attack).ToString() + "</color>";
                }
                else
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.attack + attack).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.defence + defence > PlayerManager.instance.player.defence)
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.defence + defence).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.defence + defence == PlayerManager.instance.player.defence)
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.defence + defence).ToString() + "</color>";
                }
                else
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.defence + defence).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.critcleRate + critcleRate > PlayerManager.instance.player.critcleRate)
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.critcleRate + critcleRate).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.critcleRate + critcleRate == PlayerManager.instance.player.critcleRate)
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.critcleRate + critcleRate).ToString() + "</color>";
                }
                else
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.critcleRate + critcleRate).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.critcleDmg + critcleDmg > PlayerManager.instance.player.critcleDmg)
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.critcleDmg + critcleDmg).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.critcleDmg + critcleDmg == PlayerManager.instance.player.critcleDmg)
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.critcleDmg + critcleDmg).ToString() + "</color>";
                }
                else
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.critcleDmg + critcleDmg).ToString() + "</color>";
                }

                if (PlayerManager.instance.player._stat.luk + luk > PlayerManager.instance.player._stat.luk)
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player._stat.luk + luk).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player._stat.luk + luk == PlayerManager.instance.player._stat.luk)
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player._stat.luk + luk).ToString() + "</color>";
                }
                else
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player._stat.luk + luk).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.expUp + exp > PlayerManager.instance.player.expUp)
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.expUp + exp).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.expUp + exp == PlayerManager.instance.player.expUp)
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.expUp + exp).ToString() + "</color>";
                }
                else
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.expUp + exp).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.moneyUp + money > PlayerManager.instance.player.moneyUp)
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=yellow>" + (PlayerManager.instance.player.moneyUp + money).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.moneyUp + money == PlayerManager.instance.player.moneyUp)
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=white>" + (PlayerManager.instance.player.moneyUp + money).ToString() + "</color>";
                }
                else
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=red>" + (PlayerManager.instance.player.moneyUp + money).ToString() + "</color>";
                }
            }
            else
            {
                ItemData equipItem = PlayerManager.instance.player.inventory.InventoryItemStat(equipStr[selectEquipIndex]);

                if (PlayerManager.instance.player.hp - equipItem.HpUp + Hp == PlayerManager.instance.player.hp)
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.hp - equipItem.HpUp) + Hp).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.hp - equipItem.HpUp + Hp < PlayerManager.instance.player.hp)
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.hp - equipItem.HpUp) + Hp).ToString() + "</color>";
                }
                else
                {
                    txtStat[0].text = PlayerManager.instance.player.hp.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.hp - equipItem.HpUp) + Hp).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.stamina - equipItem.StaminaUp + Stmaina == PlayerManager.instance.player.stamina)
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.stamina - equipItem.StaminaUp) + Stmaina).ToString() + "</color>";
                }
                else if (PlayerManager.instance.player.stamina - equipItem.StaminaUp + Stmaina < PlayerManager.instance.player.stamina)
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.stamina - equipItem.StaminaUp) + Stmaina).ToString() + "</color>";
                }
                else
                {
                    txtStat[1].text = PlayerManager.instance.player.stamina.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.stamina - equipItem.StaminaUp) + Stmaina).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.attack - equipItem.attackUp + attack == PlayerManager.instance.player.attack)
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.attack - equipItem.attackUp) + attack).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.attack - equipItem.attackUp + attack < PlayerManager.instance.player.attack)
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.attack - equipItem.attackUp) + attack).ToString() + "</color>";
                }
                else
                {
                    txtStat[2].text = PlayerManager.instance.player.attack.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.attack - equipItem.attackUp) + attack).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.defence - equipItem.defenceUp + defence == PlayerManager.instance.player.defence)
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.defence - equipItem.defenceUp) + defence).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.defence - equipItem.defenceUp + defence < PlayerManager.instance.player.defence)
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.defence - equipItem.defenceUp) + defence).ToString() + "</color>";
                }
                else
                {
                    txtStat[3].text = PlayerManager.instance.player.defence.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.defence - equipItem.defenceUp) + defence).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp + critcleRate == PlayerManager.instance.player.critcleRate)
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp) + critcleRate).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp + critcleRate < PlayerManager.instance.player.critcleRate)
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp) + critcleRate).ToString() + "</color>";
                }
                else
                {
                    txtStat[4].text = PlayerManager.instance.player.critcleRate.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.critcleRate - equipItem.crictleRateUp) + critcleRate).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp + critcleDmg == PlayerManager.instance.player.critcleDmg)
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp) + critcleDmg).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp + critcleDmg < PlayerManager.instance.player.critcleDmg)
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp) + critcleDmg).ToString() + "</color>";
                }
                else
                {
                    txtStat[5].text = PlayerManager.instance.player.critcleDmg.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.critcleDmg - equipItem.crictleDmgUp) + critcleDmg).ToString() + "</color>";
                }

                if (PlayerManager.instance.player._stat.luk - equipItem.lukUp + luk == PlayerManager.instance.player._stat.luk)
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player._stat.luk - equipItem.lukUp) + luk).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player._stat.luk - equipItem.lukUp + luk < PlayerManager.instance.player._stat.luk)
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player._stat.luk - equipItem.lukUp) + luk).ToString() + "</color>";
                }
                else
                {
                    txtStat[6].text = PlayerManager.instance.player._stat.luk.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player._stat.luk - equipItem.lukUp) + luk).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.expUp - equipItem.expUp + exp == PlayerManager.instance.player.expUp)
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.expUp - equipItem.expUp) + exp).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.expUp - equipItem.expUp + exp == PlayerManager.instance.player.expUp)
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.expUp - equipItem.expUp) + exp).ToString() + "</color>";
                }
                else
                {
                    txtStat[7].text = PlayerManager.instance.player.expUp.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.expUp - equipItem.expUp) + exp).ToString() + "</color>";
                }

                if (PlayerManager.instance.player.moneyUp - equipItem.moneyUp + money == PlayerManager.instance.player.moneyUp)
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=white>" + ((PlayerManager.instance.player.moneyUp - equipItem.moneyUp) + money).ToString() + "</color>";
                }
                else if(PlayerManager.instance.player.moneyUp - equipItem.moneyUp + money < PlayerManager.instance.player.moneyUp)
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=red>" + ((PlayerManager.instance.player.moneyUp - equipItem.moneyUp) + money).ToString() + "</color>";
                }
                else
                {
                    txtStat[8].text = PlayerManager.instance.player.moneyUp.ToString() + " → " + "<color=yellow>" + ((PlayerManager.instance.player.moneyUp - equipItem.moneyUp) + money).ToString() + "</color>";
                }
            }
        }
    }

    void OnEquipBtnClickEvent(int num)
    {
        StopAllBinking(btnEquip, 0);

        switch (num)
        {
            case 0:
                OnImageEquipSelectBtnClickEvent(selectEquipIndex);
                break;
            case 1:
                for (int i = 0; i < btnSelectPanel.Length; i++)
                {
                    Utils.OnOff(btnSelectPanel[i].transform.GetChild(0).gameObject, false);
                    btnSelectPanel[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                }
                OnImageEquipBtnClickEvent(selectIndex);
                break;
        }
    }

    void OnImageEquipSelectBtnClickEvent(int num)
    {
        selectEquipIndex = num;

        StopAllBinking(btnSelectPanel, 1);

        StartBlinking(selectEquipIndex, btnSelectPanel, 1);
    }

    void OnImageEquipButtonClickEvent(int num)
    {
        selectEquip = num;

        StopAllBinking(btnEquipSelect, 2);

        StartBlinking(selectEquip, btnEquipSelect, 2);
    }

    //장착 클릭시 옆에 캐릭터창 selectIndex 값에 따라서 깜빡이는게 달라짐
    void OnImageEquipBtnClickEvent(int num)
    {
        selectIndex = num;

        StopAllBinking(btnEquip,0);

        StartBlinking(selectIndex, btnEquip, 0);
    }

    private void StartBlinking(int index, Button[] btn, int num)
    {
        switch(num)
        {
            case 0:
                if (currentEquipCorutine != null)
                {
                    StopCoroutine(currentEquipCorutine);
                }
                currentEquipCorutine = StartCoroutine(Blink(index, btn));
                break;
            case 1:
                if (currentEquipSelectCorutine != null)
                {
                    StopCoroutine(currentEquipSelectCorutine);
                }
                currentEquipSelectCorutine = StartCoroutine(Blink(index, btn));
                break;
            case 2:
                if(currentEquip != null)
                {
                    StopCoroutine(currentEquip);
                }
                currentEquip = StartCoroutine(Blink(index, btn));
                break;
        }      
    }

    private IEnumerator Blink(int index, Button[] btn)
    {
        Image buttonImage = btn[index].GetComponent<Image>();
        Color originalColor = buttonImage.color;

        while (true)
        {
            buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            yield return new WaitForSeconds(blinkDuration);

            buttonImage.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    void StopAllBinking(Button[] btn, int num)
    {
        switch(num)
        {
            case 0:
                if(currentEquipCorutine != null)
                {
                    StopCoroutine(currentEquipCorutine);
                }
                break;
            case 1:
                if (currentEquipSelectCorutine != null)
                {
                    StopCoroutine(currentEquipSelectCorutine);
                }
                break;
            case 2:
                if (currentEquip != null)
                {
                    StopCoroutine(currentEquip);
                }
                break;
        }

        for (int i = 0; i < btn.Length; i++)
        {
            ResetButtonAlpha(btn[i]);
        }
    }

    private void ResetButtonAlpha(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Color originalColor = buttonImage.color;
        buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }

    #region 장비 장착 불러오는 기능
    public void LoadEquippedItemsFromData()
    {
        for (int i = 0; i < PlayerManager.instance.player.equippedItemNames.Length; i++)
        {
            string itemName = PlayerManager.instance.player.equippedItemNames[i];
            if (!string.IsNullOrEmpty(itemName))
            {
                ItemData item = PlayerManager.instance.player.inventory.InventoryItemStat(itemName);
                if (item != null)
                {
                    equipStr[i] = itemName;
                    nameStr = itemName;
                    imgEquip[i].sprite = GetItemSpriteBasedOnName(itemName);
                    imgEquip[i].preserveAspect = true;
                    Utils.OnOff(imgEquip[i].gameObject, true);

                                     
                    PlayerManager.instance.player.inventory.InventoryEquipItem(itemName);               
                }
            }
        }
    }
    #endregion
}

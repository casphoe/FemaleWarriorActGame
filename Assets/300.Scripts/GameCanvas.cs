using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Gamepad, InputSystem
using UnityEngine.InputSystem.Controls; // ButtonControl 

public class GameCanvas : MonoBehaviour
{
    [SerializeField] InputField inputName;
    [SerializeField] Button[] btnInput;

    [SerializeField] Slider[] characterSlider;

    //0 : Gold, 1 : Level, 2 : Exp, 3 : HpPotion, 4 : StaminaPotion
    [SerializeField] Text[] txtGameSetting;

    [SerializeField] Text[] txtSlotKey;

    [SerializeField] Button btnOption;

    public Image[] imgPotion;

    public static GameCanvas instance;

    [SerializeField] GameObject pausePanel;

    //0 : 취소, 1 : 저장, 2 : 불러오기 , 3 : 키보드 설정 , 4  : 게임 설졍, 5 : 사운드 설정
    [SerializeField] Button[] btnPauselPanel;

    [SerializeField] Button[] btnSlotPanel;

    [SerializeField] GameObject[] saveSlots;

    [SerializeField] Text[] txtSlot0;
    [SerializeField] Text[] txtSlot1;
    [SerializeField] Text[] txtSlot2;

    [SerializeField] Button[] btnBuy;

    [SerializeField] Button[] btnItemBuy;

    [SerializeField] Button[] btnShopBuy;

    [SerializeField] Button[] btnItemShop;

    [SerializeField] Button[] btnBlessOrTeleport;

    [SerializeField] Button[] btnItemSelect;

    public GameObject buyPanel;

    public GameObject itemBuyPanel;

    public GameObject blessPanel;

    [SerializeField] GameObject shopPanel;

    [SerializeField] Sprite[] shopSprite;

    [SerializeField] GameObject[] potionPurchaseSell;

    [SerializeField] GameObject[] itemSelectObject;

    [SerializeField] GameObject blessShopPanel;

    public int selectPanel = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inputName.text = PlayerManager.instance.player.name;
        inputName.interactable = false;
        SliderSetting();
        btnInput[0].onClick.AddListener(() => OnInputButtonSetting(0));
        btnInput[1].onClick.AddListener(() => OnInputButtonSetting(1));
        btnInput[2].onClick.AddListener(() => OnInputButtonSetting(2));      
        txtGameSetting[0].text = GetThousandCommaText(PlayerManager.instance.player.money);
        LevelAndExpSetting();
        PotionSetting();
        selectPanel = 0;
        

        btnOption.onClick.AddListener(() => OnPauseClick());
        Utils.OnOff(pausePanel, false);

        btnPauselPanel[0].onClick.AddListener(() => OnBtnPauseClick(0));
        btnPauselPanel[1].onClick.AddListener(() => OnBtnPauseClick(1));
        btnPauselPanel[2].onClick.AddListener(() => OnBtnPauseClick(2));
        btnPauselPanel[6].onClick.AddListener(() => OnBtnPauseClick(6));

        btnSlotPanel[0].onClick.AddListener(() => OnSlotPanelClick(0));
        btnSlotPanel[1].onClick.AddListener(() => OnSlotPanelClick(1));
        btnSlotPanel[2].onClick.AddListener(() => OnSlotPanelClick(2));
        btnSlotPanel[3].onClick.AddListener(() => OnSlotPanelClick(3));

        LanguageManager.Instance.UpdateLocalizedTexts();

        btnBuy[0].onClick.AddListener(() => OnBuyClickEvent(0));
        btnBuy[1].onClick.AddListener(() => OnBuyClickEvent(1));
        btnBuy[2].onClick.AddListener(() => OnBuyClickEvent(2));

        btnShopBuy[0].onClick.AddListener(() => OnShopBuyClickEvent(0));
        btnShopBuy[1].onClick.AddListener(() => OnShopBuyClickEvent(1));
        btnShopBuy[2].onClick.AddListener(() => OnShopBuyClickEvent(2));

        btnBlessOrTeleport[0].onClick.AddListener(() => OnBlessOrTelePortButtonClickEvent(0));
        btnBlessOrTeleport[1].onClick.AddListener(() => OnBlessOrTelePortButtonClickEvent(1));
        btnBlessOrTeleport[2].onClick.AddListener(() => OnBlessOrTelePortButtonClickEvent(2));

        btnItemBuy[0].onClick.AddListener(() => OnItemBuyClickEvent(0));
        btnItemBuy[1].onClick.AddListener(() => OnItemBuyClickEvent(1));
        btnItemBuy[2].onClick.AddListener(() => OnItemBuyClickEvent(2));
        btnItemBuy[3].onClick.AddListener(() => OnItemBuyClickEvent(3));

        btnItemShop[0].onClick.AddListener(() => OnItemShopClickEvent(0));
        btnItemShop[1].onClick.AddListener(() => OnItemShopClickEvent(1));
        btnItemShop[2].onClick.AddListener(() => OnItemShopClickEvent(2));
        btnItemShop[3].onClick.AddListener(() => OnItemShopClickEvent(3));

        btnItemSelect[0].onClick.AddListener(() => OnItemSelectClickEvent(0));
        btnItemSelect[1].onClick.AddListener(() => OnItemSelectClickEvent(1));
        btnItemSelect[2].onClick.AddListener(() => OnItemSelectClickEvent(2));
        SlotKeyTextChange();
    }

    private void Update()
    {
        if(PlayerManager.GetCustomKeyDown(CustomKeyCode.PauseKey))
        {
            OnPauseClick();
        }
    }


    async void OnSlotPanelClick(int num)
    {
       if(num == 3)
        {
            Utils.OnOff(pausePanel.transform.GetChild(0).gameObject, true);
            Utils.OnOff(pausePanel.transform.GetChild(1).gameObject, false);
        }
        else
        {
            switch(selectPanel)
            {
                case 1: //저장
                    int saveIndex = num;
                    PlayerManager.instance.player.SetPosition(Player.instance.transform.localPosition);

                    GoddessStatueManager.instance.SaveMapStateTo(PlayerManager.instance.player);

                    PlayerManager.instance.player.SyncVisitedFromDict();

                    PM.RegisterNewPlayer(PlayerManager.instance.player, saveIndex);

                    await PM.SavePlayerDataAndSyncAsync();     // 로컬 저장 + 클라우드 업로드 한 번에

                    SlotUiSetting();
                    break;
                case 2: //불러오기
                    PM.LoadPlayerData();

                    int LoadIndex = num;

                    PM.playerData = PM.playerList[LoadIndex];
                    PlayerManager.instance.player = PM.playerData;

                    PlayerManager.instance.player.RebuildVisitedDict();

                    GoddessStatueManager.instance.LoadMapStateFrom(PlayerManager.instance.player);

                    CM.instance.SetMap(PlayerManager.instance.player.currentMapNum, snapToTarget: true);

                    var pos = PM.playerData.GetPosition();
                    Player.instance.transform.localPosition = pos;

                    EnemyManager.instance.ActivateEnemies(PlayerManager.instance.player.currentMapNum);
                    break;
            }
        }
    }

    public void SlotKeyTextChange()
    {
        for (int i = 0; i < 7; i++)
        {
            CustomKeyCode key = CustomKeyCode.ShortcutKey1 + i; // ShortcutKey1 ~ ShortcutKey7
            var map = GameManager.data.keyMappings[key];

            if (Gamepad.current != null && !string.IsNullOrEmpty(map.gamepadButton))
            {
                txtSlotKey[i].text = map.gamepadButton;
            }
            else
            {
                txtSlotKey[i].text = map.keyCode.ToString();
            }
        }
    }

    void OnPauseClick()
    {
        PlayerManager.instance.isPause = !PlayerManager.instance.isPause;
        if(PlayerManager.instance.isPause == true)
        {
            Time.timeScale = 0;
            Utils.OnOff(pausePanel, true);
        }
        else
        {
            Time.timeScale = 1;
            Utils.OnOff(pausePanel, false);
        }
    }

    void OnBtnPauseClick(int num)
    {
        for(int i = 0; i < pausePanel.transform.childCount; i++)
        {
            Utils.OnOff(pausePanel.transform.GetChild(i).gameObject, false);
        }
        Utils.OnOff(pausePanel.transform.GetChild(0).gameObject, true);
        switch (num)
        {
            case 0:
                PlayerManager.instance.isPause = false;
                Time.timeScale = 1;
                Utils.OnOff(pausePanel, false);
                break;
            case 1:
                selectPanel = num;
                Utils.OnOff(pausePanel.transform.GetChild(0).gameObject, false);
                Utils.OnOff(pausePanel.transform.GetChild(1).gameObject, true);
                SlotUiSetting();
                break;
            case 2:
                selectPanel = num;
                Utils.OnOff(pausePanel.transform.GetChild(0).gameObject, false);
                Utils.OnOff(pausePanel.transform.GetChild(1).gameObject, true);
                SlotUiSetting();
                break;
            case 3:
                Utils.OnOff(pausePanel.transform.GetChild(2).gameObject, true);
                break;
            case 4:
                Utils.OnOff(pausePanel.transform.GetChild(3).gameObject, true);
                break;
            case 5:
                Utils.OnOff(pausePanel.transform.GetChild(4).gameObject, true);
                break;
            case 6:
                PlayerManager.instance.isPause = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(1);
                break;
        }
    }

    void SlotUiSetting()
    {
        if (PM.playerList.Count > 0)
        {
            for (int i = 0; i < saveSlots.Length; i++)
            {
                // 해당 슬롯에 데이터가 있는지 확인
                if (i < PM.playerList.Count && PM.playerList[i] != null)
                {
                    // 슬롯에 데이터가 있을 경우 활성화하고 데이터를 UI에 표시
                    Utils.OnOff(saveSlots[i], true);
                    switch (i)
                    {
                        case 0:
                            txtSlot0[0].text = PM.playerList[i].name;
                            txtSlot0[1].text = PM.playerList[i].level.ToString();
                            txtSlot0[2].text = PM.playerList[i].hp.ToString();
                            txtSlot0[3].text = PM.playerList[i].attack.ToString();
                            txtSlot0[4].text = PM.playerList[i].defence.ToString();
                            txtSlot0[5].text = PM.playerList[i].stamina.ToString();
                            txtSlot0[6].text = PM.playerList[i]._stat.luk.ToString();
                            txtSlot0[7].text = PM.playerList[i].critcleRate.ToString();
                            txtSlot0[8].text = PM.playerList[i].critcleDmg.ToString();
                            txtSlot0[9].text = PM.playerList[i].money.ToString();
                            txtSlot0[10].text = PM.playerList[i].skillCount.ToString();
                            txtSlot0[11].text = PM.playerList[i].currentExp.ToString();
                            break;
                        case 1:
                            txtSlot1[0].text = PM.playerList[i].name;
                            txtSlot1[1].text = PM.playerList[i].level.ToString();
                            txtSlot1[2].text = PM.playerList[i].hp.ToString();
                            txtSlot1[3].text = PM.playerList[i].attack.ToString();
                            txtSlot1[4].text = PM.playerList[i].defence.ToString();
                            txtSlot1[5].text = PM.playerList[i].stamina.ToString();
                            txtSlot1[6].text = PM.playerList[i]._stat.luk.ToString();
                            txtSlot1[7].text = PM.playerList[i].critcleRate.ToString();
                            txtSlot1[8].text = PM.playerList[i].critcleDmg.ToString();
                            txtSlot1[9].text = PM.playerList[i].money.ToString();
                            txtSlot1[10].text = PM.playerList[i].skillCount.ToString();
                            txtSlot1[11].text = PM.playerList[i].currentExp.ToString();
                            break;
                        case 2:
                            txtSlot2[0].text = PM.playerList[i].name;
                            txtSlot2[1].text = PM.playerList[i].level.ToString();
                            txtSlot2[2].text = PM.playerList[i].hp.ToString();
                            txtSlot2[3].text = PM.playerList[i].attack.ToString();
                            txtSlot2[4].text = PM.playerList[i].defence.ToString();
                            txtSlot2[5].text = PM.playerList[i].stamina.ToString();
                            txtSlot2[6].text = PM.playerList[i]._stat.luk.ToString();
                            txtSlot2[7].text = PM.playerList[i].critcleRate.ToString();
                            txtSlot2[8].text = PM.playerList[i].critcleDmg.ToString();
                            txtSlot2[9].text = PM.playerList[i].money.ToString();
                            txtSlot2[10].text = PM.playerList[i].skillCount.ToString();
                            txtSlot2[11].text = PM.playerList[i].currentExp.ToString();
                            break;
                    }
                }
                else
                {
                    Utils.OnOff(saveSlots[i], false);
                }
            }
        }
        else
        {
            for (int i = 0; i < saveSlots.Length; i++)
            {
                Utils.OnOff(saveSlots[i], false);
            }
        }
    }

    public string GetThousandCommaText(int data) 
    {
        if (data == 0)
        {
            return "0";
        }
        else
        {
            return string.Format("{0:#,###}", data);
        }
    }

    public void MoneySetting()
    {
        txtGameSetting[0].text = GetThousandCommaText(PlayerManager.instance.player.money);
    }

    public void LevelAndExpSetting()
    {
        switch(GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtGameSetting[1].text = "레벨 : " + PlayerManager.instance.player.level;
                txtGameSetting[2].text = "경험치 : " + PlayerManager.instance.player.currentExp + " / "  + PlayerManager.instance.player.levelUpExp;
                break;
            case LANGUAGE.ENG:
                txtGameSetting[1].text = "Level : " + PlayerManager.instance.player.level;
                txtGameSetting[2].text = "Exp : " + PlayerManager.instance.player.currentExp + " / " + PlayerManager.instance.player.levelUpExp;
                break;
        }
    }

    public void PotionSetting()
    {
        switch(PlayerManager.instance.player.hpPotionSelectnum)
        {
            case 0:
                PlayerManager.instance.hpPotionCount[0] = PlayerManager.instance.player.inventory.GetItemCountByName("체력포션");
                break;
            case 1:
                PlayerManager.instance.hpPotionCount[1] = PlayerManager.instance.player.inventory.GetItemCountByName("중간체력포션");
                break;
            case 2:
                PlayerManager.instance.hpPotionCount[2] = PlayerManager.instance.player.inventory.GetItemCountByName("상급체력포션");
                break;
        }

        switch(PlayerManager.instance.player.staminaPotionSelectnum)
        {
            case 0:
                PlayerManager.instance.staminaPotionCount[0] = PlayerManager.instance.player.inventory.GetItemCountByName("스태미나포션");
                break;
            case 1:
                PlayerManager.instance.staminaPotionCount[1] = PlayerManager.instance.player.inventory.GetItemCountByName("중간스태미나포션");
                break;
            case 2:
                PlayerManager.instance.staminaPotionCount[2] = PlayerManager.instance.player.inventory.GetItemCountByName("상급스태미나포션");
                break;
        }

        txtGameSetting[3].text = PlayerManager.instance.hpPotionCount[PlayerManager.instance.player.hpPotionSelectnum] + " / " + PlayerManager.instance.player.maxHpPotionCount;
        txtGameSetting[4].text = PlayerManager.instance.staminaPotionCount[PlayerManager.instance.player.staminaPotionSelectnum] + " / " + PlayerManager.instance.player.maxStaminaPotionCount;
    }


    public void HpPotionUiSetting(float amount, float coolDown)
    {
        float timeSinceLastUse = Time.time - amount;
        float cooldownTime = coolDown;
        imgPotion[0].fillAmount = Mathf.Clamp01(timeSinceLastUse / cooldownTime);
    }

    public void StaminaPotionUiSetting(float amount, float coolDown)
    {
        float timeSinceLastUse = Time.time - amount;
        float cooldownTime = coolDown;
        imgPotion[1].fillAmount = Mathf.Clamp01(timeSinceLastUse / cooldownTime);
    }

    void OnInputButtonSetting(int num)
    {
        switch(num)
        {
            case 0:
                inputName.interactable = true;
                break;
            case 1:
                PlayerManager.instance.player.name = inputName.text;
                inputName.interactable = false;
                break;
            case 2:
                inputName.text = PlayerManager.instance.player.name;
                inputName.interactable = false;
                break;
        }
    }

    void SliderSetting()
    {
        characterSlider[0].maxValue = PlayerManager.instance.player.hp;
        characterSlider[0].value = characterSlider[0].maxValue;

        characterSlider[1].maxValue = PlayerManager.instance.player.stamina;
        characterSlider[1].value = characterSlider[1].maxValue;
    }

    public void SliderEquipChange()
    {
        characterSlider[0].maxValue = PlayerManager.instance.player.hp;
        characterSlider[0].value = Player.instance.currentHp;

        characterSlider[1].maxValue = PlayerManager.instance.player.stamina;
        characterSlider[1].value = Player.instance.currentStamina;
    }

    public void SliderChange(int num,int change,float value)
    {
        switch(num)
        {
            //hp
            case 0:
                switch (change)
                {
                    //플러스
                    case 0:
                        if (characterSlider[0].value <= characterSlider[0].maxValue)
                        {
                            characterSlider[0].value += value;
                            if (characterSlider[0].value > characterSlider[0].maxValue)
                            {
                                characterSlider[0].value = characterSlider[0].maxValue;
                            }
                        }
                        break;
                    //마이너스
                    case 1:
                        if (characterSlider[0].value >= 0)
                        {
                            characterSlider[0].value -= value;
                            if (characterSlider[0].value <= 0)
                            {
                                characterSlider[0].value = 0;
                                //게임 오버처리
                            }
                        }
                        break;
                }
                break;
            //스태미나
            case 1:
                switch(change)
                {
                    //플러스
                    case 0:
                        if (characterSlider[1].value <= characterSlider[1].maxValue)
                        {
                            characterSlider[1].value += value;
                            if (characterSlider[1].value > characterSlider[1].maxValue)
                            {
                                characterSlider[1].value = characterSlider[1].maxValue;
                            }                                              
                        }                      
                        break;
                    //마이너스
                    case 1:
                        if (characterSlider[1].value >= 0)
                        {
                            characterSlider[1].value -= value;
                            if (characterSlider[1].value <= 0)
                            {
                                characterSlider[1].value = 0;
                            }                           
                        }
                        break;
                }
                break;
        }
    }


    void OnBuyClickEvent(int num)
    {
        switch(num)
        {
            case 0:
                PlayerManager.instance.selectShop = 0;
                Utils.OnOff(shopPanel, true);
                for(int i = 0; i < 2; i++)
                {
                    Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(i).gameObject, false);
                }
                Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(0).gameObject, true);
                OnShopUiChange(0);
                break;
            case 1:
                PlayerManager.instance.selectShop = 1;
                Utils.OnOff(shopPanel, true);
                for (int i = 0; i < 2; i++)
                {
                    Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(i).gameObject, false);
                }
                Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(0).gameObject, true);
                OnShopUiChange(0);
                break;
            case 2:
                Utils.OnOff(buyPanel, false);
                PlayerManager.instance.isBuy = false;
                break;
        }
    }

    void OnItemBuyClickEvent(int num)
    {
        switch(num)
        {
            case 0: //구매
                PlayerManager.instance.selectShop = 0;
                Utils.OnOff(shopPanel, true);
                for (int i = 0; i < 2; i++)
                {
                    Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(i).gameObject, false);
                }
                Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(1).gameObject, true);
                OnItemSelectClickEvent(0);
                break;
            case 1: //판매
                PlayerManager.instance.selectShop = 1;
                Utils.OnOff(shopPanel, true);
                for (int i = 0; i < 2; i++)
                {
                    Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(i).gameObject, false);
                }
                Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(1).gameObject, true);
                OnItemSelectClickEvent(0);
                break;
            case 2: //강화
                PlayerManager.instance.selectShop = 2;
                Utils.OnOff(shopPanel, true);
                for (int i = 0; i < 2; i++)
                {
                    Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(i).gameObject, false);
                }
                Utils.OnOff(shopPanel.transform.GetChild(0).GetChild(1).gameObject, true);
                OnItemSelectClickEvent(0);
                break;
            case 3:
                Utils.OnOff(itemBuyPanel, false);
                PlayerManager.instance.isBuy = false;
                break;
        }
        OnShopUiChange(1);
    }

    void OnShopBuyClickEvent(int num)
    {
        switch(num)
        {
            case 0:
                PlayerManager.instance.selectShop = 0;
                OnShopUiChange(0);
                break;
            case 1:
                PlayerManager.instance.selectShop = 1;
                OnShopUiChange(0);
                break;
            case 2:
                Utils.OnOff(shopPanel, false);
                break;
        }
    }

    void OnItemShopClickEvent(int num)
    {
        switch (num)
        {
            case 0:
                PlayerManager.instance.selectShop = 0;
                
                break;
            case 1:
                PlayerManager.instance.selectShop = 1;
                
                break;
            case 2:
                PlayerManager.instance.selectShop = 2;
                break;
            case 3:
                Utils.OnOff(shopPanel, false);
                break;
        }
        OnShopUiChange(1);
        OnItemSelectClickEvent(0);
    }

    void OnItemSelectClickEvent(int num)
    {
        OnShopUiChange(1);
        PlayerManager.instance.itemShopNum = num;
        switch (num)
        {
            case 0: //무기
                switch(PlayerManager.instance.selectShop)
                {
                    //구매
                    case 0:
                        EquipmentList.instance.OnWeaponPurchaseData();
                        break;
                    case 1: //판매
                        EquipmentList.instance.OnWeaponSellData();
                        break;
                    case 2: //강화
                        EnforceList.instance.ShowItemsByType(ItemDb.Weapon);
                        EnforceList.instance.WeaponEnforceShow();
                        break;
                }
                break;
            case 1: //방어구
                switch (PlayerManager.instance.selectShop)
                {
                    case 0:
                        EquipmentList.instance.OnArmorPurchaseData();
                        break;
                    case 1:
                        EquipmentList.instance.OnArmorSellData();
                        break;
                    case 2: //강화
                        EnforceList.instance.ShowItemsByType(ItemDb.Armor);
                        EnforceList.instance.ArmorEnforceShow();
                        break;
                }
                break;
            case 2: //액세서리
                switch (PlayerManager.instance.selectShop)
                {
                    case 0:
                        EquipmentList.instance.OnAccessoriesPurchaseData();
                        break;
                    case 1:
                        EquipmentList.instance.OnAccelySellData();
                        break;
                    case 2:
                        EnforceList.instance.ShowItemsByType(ItemDb.Accely);
                        EnforceList.instance.AccelyEnforceShow();
                        break;
                }
                break;
        }
    }

    void OnShopUiChange(int num)
    {
        if(num == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                Utils.OnOff(shopPanel.transform.GetChild(i + 1).gameObject, false);
                Utils.ImageSpriteChange(btnShopBuy[i].image, shopSprite[0]);
                Utils.OnOff(potionPurchaseSell[i], false);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                Utils.OnOff(shopPanel.transform.GetChild(i + 1).gameObject, false);
            }

            for(int i = 0; i < 3; i++)
            {
                Utils.ImageSpriteChange(btnItemShop[i].image, shopSprite[0]);
                Utils.ImageSpriteChange(btnItemSelect[i].image, shopSprite[0]);
                Utils.OnOff(itemSelectObject[i], false);
            }
        }

        switch(PlayerManager.instance.shopNum)
        {
            case 0:
                Utils.OnOff(shopPanel.transform.GetChild(1).gameObject, true);
                switch (PlayerManager.instance.selectShop)
                {
                    case 0: //구매
                        Utils.ImageSpriteChange(btnItemShop[0].image, shopSprite[1]);
                        Utils.OnOff(itemSelectObject[0], true);
                        break;
                    case 1: //판매
                        Utils.ImageSpriteChange(btnItemShop[1].image, shopSprite[1]);
                        Utils.OnOff(itemSelectObject[1], true);
                        break;
                    case 2: //강화
                        Utils.ImageSpriteChange(btnItemShop[1].image, shopSprite[1]);
                        Utils.OnOff(itemSelectObject[2], true);
                        break;
                }
                break;
            case 1:
                Utils.OnOff(shopPanel.transform.GetChild(2).gameObject, true);
                switch(PlayerManager.instance.selectShop)
                {
                    case 0:
                        Utils.ImageSpriteChange(btnShopBuy[0].image, shopSprite[1]);
                        PotionPurchaseList.instance.OnPurchaseData();
                        Utils.OnOff(potionPurchaseSell[0], true);
                        break;
                    case 1:
                        Utils.ImageSpriteChange(btnShopBuy[1].image, shopSprite[1]);
                        PotionPurchaseList.instance.OnSellData();
                        Utils.OnOff(potionPurchaseSell[1], true);
                        break;
                }
                break;
        }
    }


    void OnBlessOrTelePortButtonClickEvent(int num)
    {
        switch(num)
        {
            //축복
            case 0:
                Utils.OnOff(blessShopPanel, true);
                break;
                //텔레포트
            case 1:
                GoddessStatueManager.instance.MapOpenSet(true);               
                break;
            case 2:
                PlayerManager.instance.isState = false;
                Utils.OnOff(blessPanel, false);
                break;
        }
    }
}
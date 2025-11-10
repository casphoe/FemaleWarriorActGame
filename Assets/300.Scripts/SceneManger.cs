using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class SceneManger : MonoBehaviour
{
    [SerializeField] Button[] btnStart;

    [SerializeField] Button[] btnSlot;

    [SerializeField] Button[] btnOption;

    [SerializeField] GameObject startObj;
    [SerializeField] GameObject optionObj;

    [SerializeField] GameObject[] slotdataDb;

    [SerializeField] GameObject[] optionObject;

    [SerializeField] Text[] txtSlot0;
    [SerializeField] Text[] txtSlot1;
    [SerializeField] Text[] txtSlot2;

    public static SceneManger instnace;



    private void Awake()
    {
        btnStart[0].onClick.AddListener(() => OnClickStartScene(0));
        btnStart[1].onClick.AddListener(() => OnClickStartScene(1));
        btnStart[2].onClick.AddListener(() => OnClickStartScene(2));
        btnStart[3].onClick.AddListener(() => OnClickStartScene(3));       
        btnSlot[0].onClick.AddListener(() => OnGameStartClick(0));
        btnSlot[1].onClick.AddListener(() => OnGameStartClick(1));
        btnSlot[2].onClick.AddListener(() => OnGameStartClick(2));
        btnSlot[3].onClick.AddListener(() => OnGameStartClick(3));
        btnSlot[4].onClick.AddListener(() => OnGameStartClick(4));
        instnace = this;
        Utils.OnOff(startObj, false);
        Utils.OnOff(optionObj, false);
        btnOption[0].onClick.AddListener(() => OptionClick(0));
        btnOption[1].onClick.AddListener(() => OptionClick(1));
        btnOption[2].onClick.AddListener(() => OptionClick(2));
        btnOption[3].onClick.AddListener(() => OptionClick(3));
        btnOption[4].onClick.AddListener(() => OptionClick(4));
        GameManager.data.lanauge = LANGUAGE.KOR;
    }

    async void OnClickStartScene(int num)
    {
        GameManager.data.startNum = num;
        switch (GameManager.data.startNum)
        {
            case 0: //새로 시작
                Utils.OnOff(startObj, true);
                OnPlayerSlotUIDb();
                btnSlot[1].interactable = false;
                break;
            case 1: //계속하기
                Utils.OnOff(startObj, true);
                OnPlayerSlotUIDb();
                btnSlot[1].interactable = false;
                break;
            case 2: //firebase 로그아웃
                await FireBaseManager.WaitUntilReady();
                await FireBaseManager.SignOutAndWaitAsync();
                PM.SetActiveUser("_default");  // 로컬 메모리/경로 초기화
                SceneManager.LoadScene(0);
                break;
            case 3: //옵션
                Utils.OnOff(optionObj, true);
                OptionClick(0);
                break;
        }
    }

    void OnPlayerSlotUIDb()
    {
        if (PM.playerList.Count > 0)
        {
            for(int i = 0; i < slotdataDb.Length; i++)
            {
                // 해당 슬롯에 데이터가 있는지 확인
                if (i < PM.playerList.Count && PM.playerList[i] != null)
                {
                    // 슬롯에 데이터가 있을 경우 활성화하고 데이터를 UI에 표시
                    Utils.OnOff(slotdataDb[i], true);
                    switch(i)
                    {
                        case 0:
                            txtSlot0[0].text = PM.playerList[i].name;
                            txtSlot0[1].text = PM.playerList[i].level.ToString();
                            txtSlot0[2].text = PM.playerList[i].hp.ToString();
                            txtSlot0[3].text = PM.playerList[i].attack.ToString();
                            txtSlot0[4].text = PM.playerList[i].defence.ToString();
                            txtSlot0[5].text = PM.playerList[i]._stat.luk.ToString();
                            txtSlot0[6].text = PM.playerList[i].critcleRate.ToString();
                            txtSlot0[7].text = PM.playerList[i].stamina.ToString();
                            txtSlot0[8].text = PM.playerList[i].money.ToString();
                            txtSlot0[9].text = PM.playerList[i].skillCount.ToString();
                            txtSlot0[10].text = PM.playerList[i].currentExp.ToString();
                            break;
                        case 1:
                            txtSlot1[0].text = PM.playerList[i].name;
                            txtSlot1[1].text = PM.playerList[i].level.ToString();
                            txtSlot1[2].text = PM.playerList[i].hp.ToString();
                            txtSlot1[3].text = PM.playerList[i].attack.ToString();
                            txtSlot1[4].text = PM.playerList[i].defence.ToString();
                            txtSlot1[5].text = PM.playerList[i]._stat.luk.ToString();
                            txtSlot1[6].text = PM.playerList[i].critcleRate.ToString();
                            txtSlot1[7].text = PM.playerList[i].stamina.ToString();
                            txtSlot1[8].text = PM.playerList[i].money.ToString();
                            txtSlot1[9].text = PM.playerList[i].skillCount.ToString();
                            txtSlot1[10].text = PM.playerList[i].currentExp.ToString();
                            break;
                        case 2:
                            txtSlot2[0].text = PM.playerList[i].name;
                            txtSlot2[1].text = PM.playerList[i].level.ToString();
                            txtSlot2[2].text = PM.playerList[i].hp.ToString();
                            txtSlot2[3].text = PM.playerList[i].attack.ToString();
                            txtSlot2[4].text = PM.playerList[i].defence.ToString();
                            txtSlot2[5].text = PM.playerList[i]._stat.luk.ToString();
                            txtSlot2[6].text = PM.playerList[i].critcleRate.ToString();
                            txtSlot2[7].text = PM.playerList[i].stamina.ToString();
                            txtSlot2[8].text = PM.playerList[i].money.ToString();
                            txtSlot2[9].text = PM.playerList[i].skillCount.ToString();
                            txtSlot2[10].text = PM.playerList[i].currentExp.ToString();
                            break;
                    }
                }
                else
                {
                    Utils.OnOff(slotdataDb[i], false);
                }
            }
        }
        else
        {
            for (int i = 0; i < slotdataDb.Length; i++)
            {
                Utils.OnOff(slotdataDb[i], false);
            }
        }
    }

    async void OnGameStartClick(int num)
    {
        if (num == 0)
        {
            Utils.OnOff(startObj, false);
        }
        else if(num == 1) //시작하기
        {
            //해당 데이터로 시작하기
            var sel = GameManager.data.selectSlotNum;
            PM.playerData = PM.playerList[sel];
            PlayerManager.instance.player = PM.playerData;

            SceneManager.LoadScene(2);
        }
        else
        {
            GameManager.data.selectSlotNum = num - 2;          
            switch (GameManager.data.startNum)
            {
                case 0: 
                    var startSel = GameManager.data.selectSlotNum;

                    //새로시작 슬롯 있으면 삭제하고 다시 작성
                    InitializeNewGameForSelectedSlot(startSel);
                    // 1) 로컬 슬롯에 기록
                    PM.RegisterNewPlayer(PM.playerData, GameManager.data.selectSlotNum);

                    // 2) Firebase 업로드 (로그인 상태면)
                    await PM.SavePlayerDataAndSyncAsync();     // 로컬 저장 + 클라우드 업로드 한 번에

                    //UI 변경
                    OnPlayerSlotUIDb();
                    btnSlot[1].interactable = true;
                    break;
                case 1:
                    // playerList 범위 내에 있는지 확인 후 인터랙티브 설정
                    if (GameManager.data.selectSlotNum < PM.playerList.Count && PM.playerList[GameManager.data.selectSlotNum] != null)
                    {
                        btnSlot[1].interactable = true;
                    }
                    else
                    {
                        btnSlot[1].interactable = false;
                    }
                    break;
            }
        }
    }

    void InitializeNewGameForSelectedSlot(int slotIndex)
    {
        // 새로운 PlayerData를 쓰거나, 기존 구조를 초기화
        PM.playerData = PM.playerList.Count > slotIndex && PM.playerList[slotIndex] != null
            ? PM.playerList[slotIndex]
            : new PlayerData(); // 필요 시 기본 생성/팩토리 사용

        // 공통 기본값
        GameManager.data.totalPlayTime = 0;
        PM.playerData.money = 40000;
        PM.playerData.currentMapNum = 1;
        PM.playerData.expUp = 0;
        PM.playerData.moneyUp = 0;

        for (int i = 0; i < 4; i++)
        {
            PM.playerData.isBuff[i] = false;
            PM.playerData.buffRemainTime[i] = 0;
        }

        PM.playerData.SetLevel(1);
        PM.playerData.name = "FemaleWarrior";
        PM.playerData.hpPotionSelectnum = 0;
        PM.playerData.staminaPotionSelectnum = 0;
        PM.playerData.maxHpPotionCount = 5;
        PM.playerData.currentExp = 0;
        PM.playerData.rewardClearList.Clear();
        PM.playerData.rewardNumList.Clear();
        PM.playerData.skillCount = 1000;
        PM.playerData.statPoint = 1000;
        PM.playerData.currentMapNum = 1;
        PM.playerData.SetPosition(new Vector2(-23.68f, -7.92f));

        // 인벤토리 초기화 후 정확히 5,5만 지급
        PM.playerData.inventory.Clear(); // 인벤토리 전체를 비운다 (메서드명은 프로젝트에 맞춰 사용)

        GrantStarterPotionsClamped(PM.playerData, hpMax: 5, staminaMax: 5);
    }

    void GrantStarterPotionsClamped(PlayerData pd, int hpMax, int staminaMax)
    {
        // 현재 보유 수량을 가져오는 헬퍼가 없으면 하나 만들어 두세요.
        int curHp = pd.inventory.GetItemCountByName("LessHpPotion");        // 구현 필요
        int curSt = pd.inventory.GetItemCountByName("LessStaminaPotion");   // 구현 필요

        // HP 포션: 모자란 만큼만 추가 (최대 hpMax)
        if (curHp < hpMax)
        {
            int add = hpMax - curHp;
            pd.inventory.AddNewItemToInventory(
                "LessHpPotion", "체력포션",
                ItemDb.HpPotion, DataDb.None,
                0.15f, 0, 0, 0, 0, 0, 0, 0,
                0, add, 0, 0, 0, 0, 150
            );
        }

        // 스태미나 포션: 모자란 만큼만 추가 (최대 staminaMax)
        if (curSt < staminaMax)
        {
            int add = staminaMax - curSt;
            pd.inventory.AddNewItemToInventory(
                "LessStaminaPotion", "스태미나포션",
                ItemDb.StaminaPotion, DataDb.None,
                0, 0.15f, 0, 0, 0, 0, 0, 0,
                0, add, 0, 0, 0, 0, 130
            );
        }

        // 만약 AddNewItemToInventory가 “새 항목 추가”만 가능하고 수량 증가가 불가하면
        // GetItem/SetCount 같은 메서드를 만들어서 최종 수량을 Math.Min(기존+추가, Max)로 맞춰주세요.
    }

    void OptionClick(int num)
    {
        if(num != 4)
        {
            for (int i = 0; i < optionObject.Length; i++)
            {
                Utils.OnOff(optionObject[i], false);
                Utils.ImageColorChange(btnOption[i].image, Color.white);
                Utils.TextColorChange(btnOption[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
            }
            Utils.OnOff(optionObject[num], true);
            Utils.ImageColorChange(btnOption[num].image, Color.red);
            Utils.TextColorChange(btnOption[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
        }
        else
        {
            Utils.OnOff(optionObj, false);            
        }
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            Utils.OnOff(btnOption[3].gameObject, true);
        }
        else
        {
            Utils.OnOff(btnOption[3].gameObject, false);
        }
    }
}

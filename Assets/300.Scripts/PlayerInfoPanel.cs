using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//레벨업 시 캐릭터 스텟을 찍을 수 있고 현재 장착 아이템 및 현재 캐릭터 스텟을 볼 수 있는 Panel
public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] GameObject playerInfoPanel;
    [Header("캐릭터 장착 장비 이미지")]
    [SerializeField] Image[] imgCharacterEquip;

    [Header("캐릭터 스텟")]
    [SerializeField] Text[] txtCharcterStat;

    [Header("해당 캐릭터 스텟에 스텟 포인트 사용한 값")]
    [SerializeField] Text[] txtCharacterStatCount;
    //현재 스텟포인트
    [SerializeField] Text txtStatPoint;
    //현재 캐릭터 레벨
    [SerializeField] Text txtLevel;
    [SerializeField] Text txtExp;
    [SerializeField] Text txtName;

    [Header("스텟 포인트를 사용해서 캐릭터 스텟을 올려주는 버튼")]
    [SerializeField] Button[] btnStatUp;
    [Header("스텟 포인트를 사용해서 캐릭터 스텟을 올려준 스텟을 다시 내려주는 버튼")]
    [SerializeField] Button[] btnStatDown;

    [Header("찍은 스텟 포인트 저장,찍었던 스텟 포인트를 되돌리는 버튼")]
    //한번 저장하면 스텟을 찍은 것을 직전으로 되돌릴 수 없음
    [SerializeField] Button[] btnStat;


    private void Awake()
    {
        Utils.OnOff(playerInfoPanel, false);
    }


    private void Start()
    {
        for(int i = 0; i < imgCharacterEquip.Length; i++)
        {
            Utils.OnOff(imgCharacterEquip[i].gameObject, false);
        }
    }

    private void Update()
    {
        if(PlayerManager.GetCustomKeyDown(CustomKeyCode.PlayerInfo))
        {
            PlayerManager.instance.isPlayerInfo = !PlayerManager.instance.isPlayerInfo;
            var player = PlayerManager.instance.player;
            Utils.OnOff(playerInfoPanel, PlayerManager.instance.isPlayerInfo);
            StartCoroutine(PlayerEquipImage(PlayerManager.instance.isPlayerInfo));
            PlayerStatSetting(PlayerManager.instance.isPlayerInfo, player);
            StatBtnUISetting(PlayerManager.instance.isPlayerInfo, player);
        }
    }

    IEnumerator PlayerEquipImage(bool isAictive)
    {
        yield return null;
        if(isAictive == true)
        {
            for (int i = 0; i < imgCharacterEquip.Length; i++)
            {
                imgCharacterEquip[i].sprite = EquipmentPanel.instance.imgEquip[i].sprite;

                if (imgCharacterEquip[i].sprite != null)
                {
                    Utils.OnOff(imgCharacterEquip[i].gameObject, true);
                    imgCharacterEquip[i].preserveAspect = true;
                }
            }
        }
    }

    void PlayerStatSetting(bool isAictive, PlayerData playerData)
    {
        if (!isAictive)
            return;
        var player = playerData;
        switch (GameManager.data.lanauge)
        {
            case LANGUAGE.KOR:
                txtStatPoint.text = "스탯 포인트 : " + player.statPoint.ToString("F0");
                txtCharcterStat[0].text = "힘 :" + player._stat.str.ToString();
                txtCharcterStat[1].text = "지능 :" + player._stat.intellect.ToString();
                txtCharcterStat[2].text = "체력 :" + player._stat.condition.ToString();
                txtCharcterStat[3].text = "민첩 :" + player._stat.dex.ToString();
                txtCharcterStat[4].text = "운 :" + player._stat.luk.ToString();
                break;
            case LANGUAGE.ENG:
                txtStatPoint.text = "Stat Point : " + player.statPoint.ToString("F0");
                txtCharcterStat[0].text = "Str :" + player._stat.str.ToString();
                txtCharcterStat[1].text = "Int :" + player._stat.intellect.ToString();
                txtCharcterStat[2].text = "Con :" + player._stat.condition.ToString();
                txtCharcterStat[3].text = "Dex :" + player._stat.dex.ToString();
                txtCharcterStat[4].text = "Luk :" + player._stat.luk.ToString();
                break;
        }
        txtName.text = player.name;
        txtLevel.text = player.level.ToString();
        txtExp.text = player.currentExp.ToString("F0") + " / " + player.levelUpExp.ToString("F0");
        txtCharacterStatCount[0].text = player._stat.strStatCount.ToString();
        txtCharacterStatCount[1].text = player._stat.intellectStatCount.ToString();
        txtCharacterStatCount[2].text = player._stat.conditonStatCount.ToString();
        txtCharacterStatCount[3].text = player._stat.dexStatCount.ToString();
        txtCharacterStatCount[4].text = player._stat.lukStatCount.ToString();
    }

    void StatBtnUISetting(bool isAictive, PlayerData playerData)
    {
        if (!isAictive)
            return;
        var player = playerData;
        if(player.statPoint == 0)
        {
            for(int i = 0; i < btnStatUp.Length; i++)
            {
                btnStatUp[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < btnStatUp.Length; i++)
            {
                btnStatUp[i].interactable = true;
            }
        }

        if (player._stat.curStrStatCount > 0)       
            btnStatDown[0].interactable = true;       
        else
            btnStatDown[0].interactable = false;

        if (player._stat.curIntellectStatCount > 0)
            btnStatDown[1].interactable = true;
        else
            btnStatDown[1].interactable = false;

        if (player._stat.curConditonStatCount > 0)
            btnStatDown[2].interactable = true;
        else
            btnStatDown[2].interactable = false;

        if (player._stat.curDexStatCount > 0)
            btnStatDown[3].interactable = true;
        else
            btnStatDown[3].interactable = false;

        if (player._stat.curLukStatCount > 0)
            btnStatDown[4].interactable = true;
        else
            btnStatDown[4].interactable = false;
    }

    public void StatPointMinusClickEvent(int num)
    {
        var player = PlayerManager.instance.player;
        switch (num)
        {
            case 0:
                player._stat.curStrStatCount -= 1;
                player._stat.strStatCount -= 1;
                player._stat.str -= 1;
                break;
            case 1:
                player._stat.curIntellectStatCount -= 1;
                player._stat.intellectStatCount -= 1;
                player._stat.intellect -= 1;
                break;
            case 2:
                player._stat.curConditonStatCount -= 1;
                player._stat.conditonStatCount -= 1;
                player._stat.condition -= 1;
                break;
            case 3:
                player._stat.curDexStatCount -= 1;
                player._stat.dexStatCount -= 1;
                player._stat.dex -= 1;
                break;
            case 4:
                player._stat.curLukStatCount -= 1;
                player._stat.lukStatCount -= 1;
                player._stat.luk -= 1;
                break;
        }
        player.statPoint += 1;
    }

    public void StatPointPlusClickEvent(int num)
    {
        var player = PlayerManager.instance.player;
        switch (num)
        {
            case 0:
                player._stat.curStrStatCount += 1;
                player._stat.strStatCount += 1;
                player._stat.str += 1;
                break;
            case 1:
                player._stat.curIntellectStatCount += 1;
                player._stat.intellectStatCount += 1;
                player._stat.intellect += 1;
                break;
            case 2:
                player._stat.curConditonStatCount += 1;
                player._stat.conditonStatCount += 1;
                player._stat.condition += 1;
                break;
            case 3:
                player._stat.curDexStatCount += 1;
                player._stat.dexStatCount += 1;
                player._stat.dex += 1;
                break;
            case 4:
                player._stat.curLukStatCount += 1;
                player._stat.lukStatCount += 1;
                player._stat.luk += 1;
                break;
        }
        player.statPoint -= 1;
    }
}

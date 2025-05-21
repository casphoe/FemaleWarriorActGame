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

            Utils.OnOff(playerInfoPanel, PlayerManager.instance.isPlayerInfo);
            StartCoroutine(PlayerEquipImage(PlayerManager.instance.isPlayerInfo));
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//플레이어를 관리하는 함수
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public PlayerData player;

    public bool isInvisble;
    public bool isJump;
    public bool isGround;
    public bool IsDead;

    public bool isBuy = false; //캐릭터가 Npc 근처에 있을 경우
    public bool isPause = false;
    public bool isState = false; //여신상 근처에 캐릭터가 있을 경우
    public bool isInventroy = false;
    public bool isEquipment = false;
    public bool isSkillPage = false;
    public bool isDownAttacking = false;
    public bool isAiming = false;

    //0 : buy, 1 : 판매
    public int selectShop = -1;
    // 0 : 아이템, 1 : 물약
    public int shopNum = -1;

    public int itemShopNum = -1;

    public int[] hpPotionCount = new int[3];
    public int[] staminaPotionCount = new int[3];

    private void Awake()
    {
        // PlayerManager가 존재 하는지 있는지 확인
        if (instance == null)
        {
            instance = this;
            PM.LoadPlayerData();
            DontDestroyOnLoad(this); // Keep this object between scene loads
        }
        else if (instance != this)
        {
            //두 개 이상 씬에 존재할 경우 삭제
            Destroy(gameObject);
        }
    }

    public void LevelUp()
    {
        if(player.currentExp >= player.levelUpExp)
        {
            player.currentExp = player.levelUpExp - player.currentExp;
            player.level += 1;
            player.SetLevel(player.level);
        }
    }

    public void PassivePlayerStatSkillSetting(int num, float hp, float stamina, float attackUp, float defenceUp, float crictleRateUp, float crictleDmgUp,
       float hpRestoration, float StaminaRestoration)
    {
        switch (num)
        {
            case 0: //증가
                player.hp += hp;
                player.stamina += stamina;
                Player.instance.currentHp += hp;
                Player.instance.currentStamina += stamina;
                player.attack += attackUp;
                player.defense += defenceUp;
                player.critcleRate += crictleRateUp;
                player.critcleDmg += crictleDmgUp;
                player.hpAutoRestoration = hpRestoration;
                player.staminaAutoRestoration += StaminaRestoration;
                break;        
        }
        GameCanvas.instance.SliderEquipChange();
    }
}

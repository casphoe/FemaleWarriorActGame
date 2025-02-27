using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PotionData
{
    hp,stmaina
}

public class Potion : MonoBehaviour
{
    [SerializeField] Sprite[] PotionSpr;

    [SerializeField] Button[] btnPotinChange;

    [SerializeField] Image img;

    public PotionData data;
    int dataNum = 0;
    private void Awake()
    {
        img = GetComponent<Image>();
        dataNum = (int)data;
        switch(dataNum)
        {
            case 0:
                img.sprite = PotionSpr[PlayerManager.instance.player.hpPotionSelectnum];
                break;
            case 1:
                img.sprite = PotionSpr[PlayerManager.instance.player.staminaPotionSelectnum];
                break;
        }

        btnPotinChange[0].onClick.AddListener(() => OnPotionChange(0));
        btnPotinChange[1].onClick.AddListener(() => OnPotionChange(1));
    }


    void OnPotionChange(int num)
    {      
        switch(dataNum)
        {
            case 0:
                switch(num)
                {
                    case 0:
                        if(PlayerManager.instance.player.hpPotionSelectnum > 0)
                        {
                            PlayerManager.instance.player.hpPotionSelectnum -= 1;
                            btnPotinChange[1].interactable = true;
                        }
                        else
                        {
                            btnPotinChange[0].interactable = false;
                        }
                        break;
                    case 1:
                        if (PlayerManager.instance.player.hpPotionSelectnum < 2)
                        {
                            PlayerManager.instance.player.hpPotionSelectnum += 1;
                            btnPotinChange[0].interactable = true;
                        }
                        else
                        {
                            btnPotinChange[1].interactable = false;
                        }
                        break;
                }
                img.sprite = PotionSpr[PlayerManager.instance.player.hpPotionSelectnum];
                break;
            case 1:
                switch (num)
                {
                    case 0:
                        if (PlayerManager.instance.player.staminaPotionSelectnum > 0)
                        {
                            PlayerManager.instance.player.staminaPotionSelectnum -= 1;
                            btnPotinChange[1].interactable = true;
                        }
                        else
                        {
                            btnPotinChange[0].interactable = false;
                        }
                        break;
                    case 1:
                        if (PlayerManager.instance.player.staminaPotionSelectnum < 2)
                        {
                            PlayerManager.instance.player.staminaPotionSelectnum += 1;
                            btnPotinChange[0].interactable = true;
                        }
                        else
                        {
                            btnPotinChange[1].interactable = false;
                        }
                        break;
                }
                img.sprite = PotionSpr[PlayerManager.instance.player.staminaPotionSelectnum];
                break;
        }
        GameCanvas.instance.PotionSetting();
    }
}

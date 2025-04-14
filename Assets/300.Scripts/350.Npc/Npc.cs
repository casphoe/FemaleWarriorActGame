using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NpcData
{
    Item, Poition
}

public class Npc : MonoBehaviour
{
    public NpcData data;

    public GameObject buyCanvas;
    public NpcBuyCanvas npcCavnas;

    bool isPlayerNear = false;

    private void Start()
    {
        Utils.OnOff(buyCanvas, false);
        isPlayerNear = false;
    }

    private void Update()
    {
        if(isPlayerNear == true)
        {
            Utils.OnOff(buyCanvas, true);
            if (npcCavnas.rectBuyCanvas != null)
            {
                Utils.OnOff(npcCavnas.rectBuyCanvas.gameObject, true);
            }

            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.ActionKey))
            {
                PlayerManager.instance.isBuy = true;
                switch (data)
                {
                    case NpcData.Item:
                        Utils.OnOff(GameCanvas.instance.itemBuyPanel, true);
                        break;
                    case NpcData.Poition:
                        Utils.OnOff(GameCanvas.instance.buyPanel, true);
                        break;
                }
                PlayerManager.instance.shopNum = (int)data;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerNear = false;
            Utils.OnOff(buyCanvas, false);
            if (npcCavnas.rectBuyCanvas != null)
            {
                Utils.OnOff(npcCavnas.rectBuyCanvas.gameObject, false);
            }
        }
    }
}

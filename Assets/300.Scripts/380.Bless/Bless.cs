using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : MonoBehaviour
{
    public string statueID; // 여신상 고유 ID
    public GameObject blessCanvas;
    BlessPanel panel;
    bool isPlayerNear = false;

    private void Start()
    {
        Utils.OnOff(blessCanvas, false);
        panel = blessCanvas.GetComponent<BlessPanel>();
        isPlayerNear = false;
    }

    private void Update()
    {
        if(isPlayerNear == true)
        {
            Utils.OnOff(blessCanvas, true);

            if (panel.rectBlessCanvas != null)
            {
                Utils.OnOff(panel.rectBlessCanvas.gameObject, true);
            }

            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.ActionKey))
            {
                if (!GoddessStatueManager.instance.IsStatueDiscovered(statueID))
                {
                    GoddessStatueManager.instance.RegisterStatue(statueID);
                }
                PlayerManager.instance.isState = true;
                Utils.OnOff(GameCanvas.instance.blessPanel, true);
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
            Utils.OnOff(blessCanvas, false);
            if (panel.rectBlessCanvas != null)
            {
                Utils.OnOff(panel.rectBlessCanvas.gameObject, false);
            }
        }
    }
}

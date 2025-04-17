using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : MonoBehaviour
{
    public GameObject blessCanvas;
    BlessPanel panel;
    bool isPlayerNear = false;
    public bool isRegistered = false; //석상이 등록되었는지 확인하기 위한 여부

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
                if(!isRegistered)
                {
                    isRegistered = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : MonoBehaviour
{
    public GameObject blessCanvas;
    BlessPanel panel;

    private void Start()
    {
        Utils.OnOff(blessCanvas, false);
        panel = blessCanvas.GetComponent<BlessPanel>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Utils.OnOff(blessCanvas, true);

            if(panel.rectBlessCanvas != null)
            {
                Utils.OnOff(panel.rectBlessCanvas.gameObject, true);
            }

            if (Input.GetKey(GameManager.data.keyMappings[CustomKeyCode.ActionKey]))
            {
                PlayerManager.instance.isState = true;
                Utils.OnOff(GameCanvas.instance.blessPanel, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Utils.OnOff(blessCanvas, false);
            if (panel.rectBlessCanvas != null)
            {
                Utils.OnOff(panel.rectBlessCanvas.gameObject, false);
            }
        }
    }
}

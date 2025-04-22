using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlertData
{
    Warning,Notice
}

public class Alert : MonoBehaviour
{
    [SerializeField] AlertData data;

    [SerializeField] GameObject AlertCanves;
    AlertPanel panel;

    bool isPlayerNear = false;


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
            Utils.OnOff(AlertCanves, false);
            if (panel.rectBlessCanvas != null)
            {
                Utils.OnOff(panel.rectBlessCanvas.gameObject, false);
            }
        }
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            Utils.OnOff(AlertCanves, true);

            if (panel.rectBlessCanvas != null)
            {
                Utils.OnOff(panel.rectBlessCanvas.gameObject, true);
            }
        }
    }

    private void Start()
    {
        isPlayerNear = false;
        Utils.OnOff(AlertCanves, false);
        panel = AlertCanves.GetComponent<AlertPanel>();
    }
}

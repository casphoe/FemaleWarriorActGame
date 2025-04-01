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
        }
    }
}

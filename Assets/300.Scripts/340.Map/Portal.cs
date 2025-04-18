using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Animator anim;

    public int portalIndex;
    public int nextPortalIndex;
    public string targetMapID;
    private bool playerInPortal = false;
    private GameObject playerObj;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Gate", true);
            playerInPortal = true;
            playerObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Gate", false);
            playerInPortal = false;
            playerObj = null;
        }
    }

    private void Update()
    {
        if (playerInPortal && PlayerManager.GetCustomKeyDown(CustomKeyCode.ActionKey))
        {
            StartCoroutine(PortalCorute(playerObj));
        }
    }

    IEnumerator PortalCorute(GameObject player)
    {
        yield return null;
        Portal[] portals = FindObjectsOfType<Portal>();
        foreach (Portal portal in portals)
        {
            if (portal.portalIndex == nextPortalIndex)
            {
                Player.instance.transform.position = portal.transform.position;

                // 맵 번호 변경 및 등록 처리
                int mapNum = -1;

                switch (nextPortalIndex)
                {
                    case 0: mapNum = 0; break;
                    case 1: mapNum = 1; break;
                    case 2: mapNum = 1; break;
                    case 3: mapNum = 2; break;
                    case 4: mapNum = 2; break;
                    case 5: mapNum = 3; break;
                    default: break;
                }

                if (mapNum != -1)
                {
                    Player.instance.currentMapNum = mapNum;

                    //맵 방문 처리
                }

                break;
            }
        }
    }
}

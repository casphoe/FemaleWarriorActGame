using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Animator anim;

    public int portalIndex;
    public int nextPortalIndex;
    private bool playerInPortal = false;
    private GameObject playerObj;

    [Header("맵 이동의 관련된 변수들")]
    public string targetMapID;
    public string targetMapNameKor;
    public string targetMapNameEng;
    public MapType mapType;
    public int iconIndex;

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
                    case 6: mapNum = 4; break;
                    default: break;
                }

                if (mapNum != -1)
                {
                    // 1) 현재 맵 번호 반영
                    Player.instance.currentMapNum = mapNum;
                    var pd = PlayerManager.instance.player;
                    pd.currentMapNum = mapNum;
                    // 2) 여신상은 Register, 그 외는 AddMap
                    if (mapType == MapType.GoddessStatue)
                    {
                        // 중복 등록을 막아주는 HashSet 기반이므로 여러번 호출해도 안전
                        GoddessStatueManager.instance.RegisterStatue(
                            targetMapID, targetMapNameKor, targetMapNameEng, mapNum, iconIndex);
                    }
                    else
                    {
                        GoddessStatueManager.instance.AddMap(
                            targetMapID, targetMapNameKor, targetMapNameEng, mapType, mapNum, iconIndex);
                        GoddessStatueManager.instance.OnEnterNewMap(targetMapID);
                    }

                    // 3) 미니맵 캐릭터 마커/카메라/적 활성화
                    GoddessStatueManager.instance.MoveCharacterToStatue(targetMapID);
                    CM.instance.SnapToTarget(mapNum);
                    EnemyManager.instance.ActivateEnemies(mapNum);

                }

                break;
            }
        }
    }
}

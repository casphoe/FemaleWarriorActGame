using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//퀘스트 수락 및 완료, 제거 하게 해주는 오브젝트
public class QuestBillBoard : MonoBehaviour
{
    public GameObject questCanvas;
    public QuestAcceptPlus questAccept;


    bool isPlayerNear = false;

    private void Start()
    {
        Utils.OnOff(questCanvas, false);
        isPlayerNear = false;
    }


    private void Update()
    {
        if (isPlayerNear == true)
        {
            Utils.OnOff(questCanvas, true);
            if(questAccept.rectBuyCanvas != null)
            {
                Utils.OnOff(questAccept.rectBuyCanvas.gameObject, true);
            }

            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.ActionKey))
            {
                PlayerManager.instance.isQuest = true;
                QuestManager.instance.OnOffQuestUI(true);
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
            Utils.OnOff(questCanvas, false);
            if (questAccept.rectBuyCanvas != null)
            {
                Utils.OnOff(questAccept.rectBuyCanvas.gameObject, false);
            }
        }
    }
}

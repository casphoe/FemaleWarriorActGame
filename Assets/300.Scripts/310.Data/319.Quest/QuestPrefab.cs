using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gpm.Ui;

public class QuestPrefabData : InfiniteScrollData
{
    public int index = 0;
    public int number = 0;

    public QuestPrefab prefab; 
}

public class QuestPrefab : InfiniteScrollItem
{
    public Text[] txt;

    public string titleKor = string.Empty;
    public string titleEng = string.Empty;

    public string descKor = string.Empty;
    public string descEng = string.Empty;
    //0 : 수락, 1 : 진행중인 퀘스트 , 2 : 완료, 3 : 제거
    public int index = 0;

    public int listIndex = 0;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

        QuestPrefabData data = (QuestPrefabData)scrollData;

        listIndex = data.index;

        data.prefab = this;

        switch (index)
        {
            case 0:
                titleKor = QuestManager.instance.acceptQuest[listIndex].titleKor;
                titleEng = QuestManager.instance.acceptQuest[listIndex].titleEng;
                descKor = QuestManager.instance.acceptQuest[listIndex].descriptionKor;
                descEng = QuestManager.instance.acceptQuest[listIndex].descriptionEng;

                switch(GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txt[0].text = "수락 가능";
                        txt[1].text = titleKor;
                        txt[2].text = "클리어 조건 : " + QuestManager.instance.acceptQuest[listIndex].requiredAmount.ToString();
                        txt[3].text = "획득 경험치 : " + QuestManager.instance.acceptQuest[listIndex].rewardExp.ToString();
                        txt[4].text = "획득 재화 : " + QuestManager.instance.acceptQuest[listIndex].rewardMoney.ToString();
                        break;
                    case LANGUAGE.ENG:
                        txt[0].text = "Acceptable";
                        txt[1].text = titleEng;
                        txt[2].text = "Clear conditions : " + QuestManager.instance.acceptQuest[listIndex].requiredAmount.ToString();
                        txt[3].text = "Gain Exp : " + QuestManager.instance.acceptQuest[listIndex].rewardExp.ToString();
                        txt[4].text = "Gain Money : " + QuestManager.instance.acceptQuest[listIndex].rewardMoney.ToString();
                        break;
                }
                break;
            case 1:
                titleKor = QuestManager.instance.ongoingQuest[listIndex].titleKor;
                titleEng = QuestManager.instance.ongoingQuest[listIndex].titleEng;

                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:                    
                        txt[0].text = titleKor;
                        txt[1].text = "클리어 조건 : " + QuestManager.instance.ongoingQuest[listIndex].requiredAmount.ToString();
                        txt[2].text = "현재 상태 : " + QuestManager.instance.ongoingQuest[listIndex].currentAmount.ToString();
                        txt[3].text = "획득 경험치 : " + QuestManager.instance.ongoingQuest[listIndex].rewardExp.ToString();
                        txt[4].text = "획득 재화 : " + QuestManager.instance.ongoingQuest[listIndex].rewardMoney.ToString();
                        break;
                    case LANGUAGE.ENG:                  
                        txt[0].text = titleEng;
                        txt[1].text = "Clear conditions : " + QuestManager.instance.ongoingQuest[listIndex].requiredAmount.ToString();
                        txt[2].text = "Current status : " + QuestManager.instance.ongoingQuest[listIndex].currentAmount.ToString();
                        txt[3].text = "Gain Exp : " + QuestManager.instance.ongoingQuest[listIndex].rewardExp.ToString();
                        txt[4].text = "Gain Money : " + QuestManager.instance.ongoingQuest[listIndex].rewardMoney.ToString();
                        break;
                }
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    public void OnClick()
    {
        OnSelect();
    }
}

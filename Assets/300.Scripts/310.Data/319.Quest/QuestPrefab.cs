﻿using System.Collections;
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

    public bool isCompelete = false;

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
                if (listIndex < 0 || listIndex >= QuestManager.instance.ongoingQuest.Count)
                {
                    return;
                }

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
                if (listIndex < 0 || listIndex >= QuestManager.instance.completeQuest.Count)
                {
                    return;
                }

                titleKor = QuestManager.instance.completeQuest[listIndex].titleKor;
                titleEng = QuestManager.instance.completeQuest[listIndex].titleEng;

                descKor = QuestManager.instance.completeQuest[listIndex].descriptionKor;
                descEng = QuestManager.instance.completeQuest[listIndex].descriptionEng;

                isCompelete = QuestManager.instance.completeQuest[listIndex].isComplete;

                if(isCompelete == true)
                {
                    GetComponent<Button>().interactable = false;
                }
                else
                {
                    GetComponent<Button>().interactable = true;
                }

                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        if(isCompelete == true)
                        {
                            txt[0].text = "퀘스트 완료";
                        }
                        else
                        {
                            txt[0].text = "퀘스트 완료 가능";
                        }
                        txt[1].text = titleKor;
                        txt[2].text = "획득 경험치 : " + QuestManager.instance.completeQuest[listIndex].rewardExp.ToString();
                        txt[3].text = "획득 재화 : " + QuestManager.instance.completeQuest[listIndex].rewardMoney.ToString();
                        break;
                    case LANGUAGE.ENG:
                        if (isCompelete == true)
                        {
                            txt[0].text = "Quest completed";
                        }
                        else
                        {
                            txt[0].text = "Quest can be completed";
                        }
                        txt[1].text = titleEng;
                        txt[2].text = "Gain Exp : " + QuestManager.instance.completeQuest[listIndex].rewardExp.ToString();
                        txt[3].text = "Gain Money : " + QuestManager.instance.completeQuest[listIndex].rewardMoney.ToString();
                        break;
                }
                break;
            case 3:
                //제거 될 때 listIndex가 제대로 적용이 안되고 titleKor = QuestManager.instance.ongoingQuest[listIndex].titleKor; 넘어오는 현상이 발생
                if (listIndex < 0 || listIndex >= QuestManager.instance.ongoingQuest.Count)
                {
                    return;
                }

                titleKor = QuestManager.instance.ongoingQuest[listIndex].titleKor;
                titleEng = QuestManager.instance.ongoingQuest[listIndex].titleEng;
                descKor = QuestManager.instance.ongoingQuest[listIndex].descriptionKor;
                descEng = QuestManager.instance.ongoingQuest[listIndex].descriptionEng;

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
        }
    }

    public void OnClick()
    {
        OnSelect();
    }
}

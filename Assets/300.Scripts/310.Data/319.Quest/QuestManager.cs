using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AcceptQuest(PlayerData playerData, QuestData originalQuest)
    {
        QuestData newQuest = new QuestData
        {
            questId = originalQuest.questId,
            descriptionKor = originalQuest.descriptionKor,
            descriptionEng = originalQuest.descriptionEng,
            clearCondition = originalQuest.clearCondition,
            requiredAmount = originalQuest.requiredAmount,
            rewardExp = originalQuest.rewardExp,
            rewardMoney = originalQuest.rewardMoney,

            currentAmount = 0,
            isAccepted = true,
            isCleared = false,
            isComplete = false
        };

        playerData.questList.Add(newQuest);
        Debug.Log($"퀘스트 {newQuest.questId} 수락됨 (새로 생성)");
    }

    public void RemoveQuest(PlayerData playerData, int questId)
    {
        QuestData quest = playerData.questList.Find(q => q.questId == questId);
        if (quest != null)
        {
            playerData.questList.Remove(quest);
            Debug.Log($"퀘스트 {questId} 삭제됨");
        }
    }

    public void CompleteQuest(PlayerData playerData, int questId)
    {
        QuestData quest = playerData.questList.Find(q => q.questId == questId && quest.isCleared && !quest.isComplete);
        if (quest != null)
        {
            quest.isComplete = true;

            playerData.currentExp += quest.rewardExp;
            playerData.money += quest.rewardMoney;

            Debug.Log($"퀘스트 {quest.questId} 클리어됨! 보상: EXP {quest.rewardExp}, Money {quest.rewardMoney}");
        }
        else
        {
            Debug.Log("클리어할 수 없는 퀘스트거나 아직 완료 조건이 안 됨.");
        }
    }
}

[Serializable]
public class QuestData
{
    public int questId;                   // 퀘스트 고유 ID
    public string titleKor;             // 퀘스트 제목(한국어)
    public string titleEng;             // 퀘스트 제목 (영어)
    public string descriptionKor;        // 퀘스트 내용 (한국어)
    public string descriptionEng;        // 퀘스트 내용 (영어)
    public int rewardExp;                // 클리어 시 얻는 경험치
    public int rewardMoney;              // 클리어 시 얻는 돈

    public int requiredAmount;             // 클리어를 위해 필요한 수치 (예: 5마리)
    public int currentAmount;              // 현재까지 진행된 수치 (예: 3마리)

    public bool isAccepted = false;            // 수락 여부
    public bool isComplete = false;            // 클리어 버튼 눌러 완료된 여부 (수동)
    public bool isCleared;               // 클리어 여부

    public bool IsConditionMet => currentAmount >= requiredAmount;
}
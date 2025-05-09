using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] QuestDataReader dataReader;

    [SerializeField] GameObject qusetPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Utils.OnOff(qusetPanel, false);
    }

    #region 수락, 제거, 완료
    public void AcceptQuest(PlayerData playerData, QuestData originalQuest)
    {
        // 이미 같은 퀘스트를 수락 중인지 확인
        var existingQuest = playerData.questList.Find(q => q.questId == originalQuest.questId);

        // 반복 퀘스트가 아니고, 이미 완료했다면 다시 수락 불가
        if (existingQuest != null && !originalQuest.isRepeat)
        {
            Debug.Log($"퀘스트 {originalQuest.questId}는 이미 수락되었거나 완료되어 재수락 불가.");
            return;
        }

        QuestData newQuest = new QuestData(
        originalQuest.questId,
        originalQuest.titleKor,
        originalQuest.titleEng,
        originalQuest.descriptionKor,
        originalQuest.descriptionEng,
        originalQuest.rewardExp,
        originalQuest.rewardMoney,
        originalQuest.requiredAmount,
        0,                      // currentAmount는 새 퀘스트이므로 0부터 시작
        true,                  // 수락 상태
        false,                 // 완료 버튼 누르기 전
        false,                  // 클리어 전
        originalQuest.isRepeat
    );
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
        QuestData quest = playerData.questList.Find(q => q.questId == questId && q.isCleared && !q.isComplete);
        if (quest != null)
        {
            quest.isComplete = true;

            playerData.currentExp += quest.rewardExp;
            playerData.money += quest.rewardMoney;

            Debug.Log($"퀘스트 {quest.questId} 클리어됨! 보상: EXP {quest.rewardExp}, Money {quest.rewardMoney}");

            // 반복 퀘스트면 완료 후 제거 (재수락 가능)
            if (quest.isRepeat)
            {
                playerData.questList.Remove(quest);
                Debug.Log($"반복 퀘스트 {quest.questId} 완료 후 제거되어 재수락 가능함.");
            }
        }
        else
        {
            Debug.Log("클리어할 수 없는 퀘스트거나 아직 완료 조건이 안 됨.");
        }
    }
    #endregion

    #region UI

    #endregion
}
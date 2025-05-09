using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    [SerializeField] QuestDataReader dataReader;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ApplyQuestDataToAll();
    }

    #region 구글 스프레트 시트
    void ApplyQuestDataToAll()
    {
        foreach (QuestData quest in dataReader.QuestDataList)
        {
            //UI에 퀘스트를 등록하는 코드
        }
    }
    #endregion

    #region 수락, 제거, 완료
    public void AcceptQuest(PlayerData playerData, QuestData originalQuest)
    {
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
        false                  // 클리어 전
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
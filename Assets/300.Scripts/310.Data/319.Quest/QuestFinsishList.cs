using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class QuestFinsishList : MonoBehaviour
{
    public InfiniteScroll questFinishScrollList;

    public GameObject questFinishListContent;

    private List<QuestPrefabData> dataList = new List<QuestPrefabData>();

    public int index = 0;
    public int selectIndex = 0;

    private void Awake()
    {
        questFinishScrollList.AddSelectCallback((data) =>
        {
            QuestManager.instance.selectQuestNum = ((QuestPrefabData)data).index;
            selectIndex = QuestManager.instance.selectQuestNum;
            QuestManager.instance.OnOffUiSetObject(true);
            QuestManager.instance.OnUiSetTextSetting(2);
        });
    }

    void QuestCompeteleListClear()
    {
        dataList.Clear();
        questFinishScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) // 이 부분 확인할 것
        {
            QuestPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void CompeteLoadList()
    {
        QuestCompeteleListClear();
        if(dataList.Count != QuestManager.instance.completeQuest.Count)
        {
            int difference = Mathf.Abs(dataList.Count - QuestManager.instance.completeQuest.Count);
            for (int i = 0; i < difference; i++)
            {
                QuestCompertInsertData();
            }
        }
        AllUpdate();
    }

    void QuestCompertInsertData()
    {
        QuestPrefabData data = new QuestPrefabData();
        data.index = index++;
        data.number = questFinishScrollList.GetItemCount() + 1;
        dataList.Add(data);
        questFinishScrollList.InsertData(data);

        QuestManager.instance.questPrefabList.Add(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        questFinishScrollList.UpdateAllData();
    }
}

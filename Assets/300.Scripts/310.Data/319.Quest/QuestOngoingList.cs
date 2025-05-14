using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class QuestOngoingList : MonoBehaviour
{
    public InfiniteScroll questCurrentScrollList;
    public GameObject questCurrentListContent;

    private List<QuestPrefabData> dataList = new List<QuestPrefabData>();

    public int index = 0;

    void QuestCurrentClear()
    {
        dataList.Clear();
        questCurrentScrollList.ClearData();
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

    public void CurrentQuestLoadList()
    {
        QuestCurrentClear();
        if (dataList.Count != QuestManager.instance.ongoingQuest.Count)
        {
            int difference = Mathf.Abs(dataList.Count - QuestManager.instance.ongoingQuest.Count);
            for (int i = 0; i < difference; i++)
            {
                QuestCurrentInsertData();
            }
        }
        AllUpdate();
    }

    void QuestCurrentInsertData()
    {
        QuestPrefabData data = new QuestPrefabData();
        data.index = index++;
        data.number = questCurrentScrollList.GetItemCount() + 1;
        dataList.Add(data);
        questCurrentScrollList.InsertData(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        questCurrentScrollList.UpdateAllData();
    }
}

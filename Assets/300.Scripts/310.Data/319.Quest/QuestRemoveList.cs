using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class QuestRemoveList : MonoBehaviour
{
    public InfiniteScroll questRemoveScrollList;

    public GameObject questRemoveListContent;

    public int index = 0;
    public int selectIndex = 0;

    private List<QuestPrefabData> dataList = new List<QuestPrefabData>();

    private void Awake()
    {
        questRemoveScrollList.AddSelectCallback((data) =>
        {
            QuestManager.instance.selectQuestNum = ((QuestPrefabData)data).index;
            selectIndex = QuestManager.instance.selectQuestNum;
            QuestManager.instance.OnOffUiSetObject(true);
            QuestManager.instance.OnUiSetTextSetting(3);
        });
    }

    void QuestRemoveListClear()
    {
        dataList.Clear();
        questRemoveScrollList.ClearData();
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

    public void RemoveLoadList()
    {
        QuestRemoveListClear();
        if(dataList.Count != QuestManager.instance.ongoingQuest.Count)
        {
            int difference = Mathf.Abs(dataList.Count - QuestManager.instance.ongoingQuest.Count);
            for (int i = 0; i < difference; i++)
            {
                QuestRemoveInsertData();
            }
        }
        AllUpdate();
    }

    void QuestRemoveInsertData()
    {
        QuestPrefabData data = new QuestPrefabData();
        data.index = index++;
        data.number = questRemoveScrollList.GetItemCount() + 1;
        dataList.Add(data);
        questRemoveScrollList.InsertData(data);

        QuestManager.instance.questPrefabList.Add(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        questRemoveScrollList.UpdateAllData();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;

public class QuestAcceptList : MonoBehaviour
{
    public InfiniteScroll questAcceptScrollList;
    public GameObject questAccpetListContent;

    public int index = 0;
    public int selectIndex = 0;

    private List<QuestPrefabData> dataList = new List<QuestPrefabData>();

    private void Awake()
    {
        questAcceptScrollList.AddSelectCallback((data) =>
        {
            QuestManager.instance.selectQuestNum = ((QuestPrefabData)data).index;
            selectIndex = QuestManager.instance.selectQuestNum;
            QuestManager.instance.OnOffUiSetObject(true);
            QuestManager.instance.OnUiSetTextSetting(0);
        });
    }

    void QuestAccpetListClear()
    {
        dataList.Clear();
        questAcceptScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++)  // 이 부분 확인할 것
        {
            QuestPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void AcceptLoadList()
    {
        QuestAccpetListClear();
        if (dataList.Count != QuestManager.instance.acceptQuest.Count)
        {
            int difference = Mathf.Abs(dataList.Count - QuestManager.instance.acceptQuest.Count);
            for (int i = 0; i < difference; i++)
            {
                QuestAccpertInsertData();
            }
        }
        AllUpdate();
    }

    void QuestAccpertInsertData()
    {
        QuestPrefabData data = new QuestPrefabData();
        data.index = index++;
        data.number = questAcceptScrollList.GetItemCount() + 1;
        dataList.Add(data);
        questAcceptScrollList.InsertData(data);

        QuestManager.instance.questPrefabList.Add(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        questAcceptScrollList.UpdateAllData();
    }
}

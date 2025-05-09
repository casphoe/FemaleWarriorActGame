using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public bool isAccepted;           // 수락 여부
    public bool isComplete;            // 클리어 버튼 눌러 완료된 여부 (수동)
    public bool isCleared;               // 클리어 여부
    public bool isRepeat;

    public bool IsConditionMet => currentAmount >= requiredAmount;

    public QuestData(int questId, string titleKor, string titleEng, string descriptionKor, string descriptionEng, int rewardExp, int rewardMoney, int requiredAmount, int currentAmount, bool isAccepted, bool isComplete, bool isCleared, bool isRepeat)
    {
        this.questId = questId;
        this.titleKor = titleKor;
        this.titleEng = titleEng;
        this.descriptionKor = descriptionKor;
        this.descriptionEng = descriptionEng;
        this.rewardExp = rewardExp;
        this.rewardMoney = rewardMoney;
        this.requiredAmount = requiredAmount;
        this.currentAmount = currentAmount;
        this.isAccepted = isAccepted;
        this.isComplete = isComplete;
        this.isCleared = isCleared;
        this.isRepeat = isRepeat;
    }
}
[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/QuestDataReader", order = int.MaxValue)]
public class QuestDataReader : DataReaderBase
{
    //시트에서 데이터를 읽으면 이 리스트에 저장
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")][SerializeField] public List<QuestData> QuestDataList = new List<QuestData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        int questId = 0, rewardExp = 0,  rewardMoney = 0, requiredAmount = 0, currentAmount = 0;
        string titleKor = null, titleEng = null, descriptionKor = null, descriptionEng = null;
        bool isAccepted = false, isComplete = false, isCleared = false, isRepeat = false;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "questId":
                    {
                        questId = int.Parse(list[i].value);
                        break;
                    }
                case "titleKor":
                    {
                        titleKor = list[i].value;
                        break;
                    }
                case "titleEng":
                    {
                        titleEng = list[i].value;
                        break;
                    }
                case "descriptionKor":
                    {
                        descriptionKor = list[i].value;
                        break;
                    }
                case "descriptionEng":
                    {
                        descriptionEng = list[i].value;
                        break;
                    }
                case "rewardExp":
                    {
                        rewardExp = int.Parse(list[i].value);
                        break;
                    }
                case "rewardMoney":
                    {
                        rewardMoney = int.Parse(list[i].value);
                        break;
                    }
                case "requiredAmount":
                    {
                        requiredAmount = int.Parse(list[i].value);
                        break;
                    }
                case "currentAmount":
                    {
                        currentAmount = int.Parse(list[i].value);
                        break;
                    }
                case "isAccepted":
                    {
                        //파싱이 실패해도 예외가 안되고 false로 초기화 하기 위해서 사용
                        bool.TryParse(list[i].value, out isAccepted);
                        break;
                    }
                case "isComplete":
                    {
                        bool.TryParse(list[i].value, out isComplete);
                        break;
                    }
                case "isCleared":
                    {
                        bool.TryParse(list[i].value, out isCleared);
                        break;
                    }
                case "isRepeat":
                    {
                        bool.TryParse(list[i].value, out isRepeat);
                        break;
                    }
            }
        }
        QuestDataList.Add(new QuestData(questId, titleKor, titleEng, descriptionKor, descriptionEng, rewardExp, rewardMoney, requiredAmount, currentAmount, isAccepted, isComplete, isCleared, isRepeat));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(QuestDataReader))]
public class QuestDataReaderEditor : Editor
{
    QuestDataReader data;

    private void OnEnable()
    {
        data = (QuestDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 읽기(API 호출)"))
        {
            data.QuestDataList.Clear();
            UpdateStats(UpdateMethodOne);
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        for (int i = data.START_ROW_LENGTH; i <= data.END_ROW_LENGTH; ++i)
        {
            data.UpdateStats(ss.rows[i], i);
        }

        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();          // 디스크 저장
        AssetDatabase.Refresh();             // 갱신     
    }
}
#endif

using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct CheastData
{
    public int id;
    public string name;
    public string skill;
    public int gold;
    public int exp;
    public float addhp;
    public float addstamina;

    public CheastData(int id, string name, string skill,int gold, int exp, float addhp, float addstamina)
    {
        this.id = id;
        this.name = name;
        this.skill = skill;
        this.gold = gold;
        this.exp = exp;
        this.addhp = addhp;
        this.addstamina = addstamina;
    }
}

//구글 스프레트 시트를 통한 보물 상자 데이터 
[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/TreasureChasetDataReader", order = int.MaxValue)]
public class TreasureChasetDataReader : DataReaderBase
{
    //시트에서 데이터를 읽으면 이 리스트에 저장
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")]
    [SerializeField] public List<CheastData> CheastList = new List<CheastData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        int id = 0, gold = 0, exp = 0;
        string name = null, skill = null;
        float addhp = 0, addstamina = 0;
        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "id":
                    {
                        id = int.Parse(list[i].value);
                        break;
                    }
                case "name":
                    {
                        name = list[i].value;
                        break;
                    }
                case "skill":
                    {
                        skill = list[i].value;
                        break;
                    }
                case "gold":
                    {
                        gold = int.Parse(list[i].value);
                        break;
                    }
                case "exp":
                    {
                        exp = int.Parse(list[i].value);
                        break;
                    }
                case "addHp":
                    {
                        addhp = int.Parse(list[i].value);
                        break;
                    }
                case "addstamina":
                    {
                        addstamina = int.Parse(list[i].value);
                        break;
                    }
            }
        }

        CheastList.Add(new CheastData(id, name,skill,gold,exp,addhp,addstamina));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TreasureChasetDataReader))]
public class TreasureChasetDataReaderEditor : Editor
{
    TreasureChasetDataReader data;

    private void OnEnable()
    {
        data = (TreasureChasetDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 읽기(API 호출)"))
        {
            UpdateStats(UpdateMethodOne);
            data.CheastList.Clear();
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
    }
}
#endif
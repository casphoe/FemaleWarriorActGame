using UnityEngine;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif
//적 설정 값 구조체
[Serializable]
public struct EnemyData
{
    public int id;
    public string name;
    public float hp;
    public float attack;
    public float defence;
    public int addMoney;
    public int addExp;
    public float critcleRate;
    public float critcleDmg;
    public string attackPattern;
    public float attackRange;
    public float guardValue;
    public float guardRecoverycoolTime;
    public float guardRecoveryValue;

    public EnemyData(int id, string name, float hp, float attack, float defence, int addMoney, int addExp , float critcleRate, float critcleDmg, string attackPattern, float attackRange, float guardValue, float guardRecoverycoolTime, float guardRecoveryValue)
    {
        this.id = id;
        this.name = name;
        this.hp = hp;
        this.attack = attack;
        this.defence = defence;
        this.addMoney = addMoney;
        this.addExp = addExp;
        this.critcleRate = critcleRate;
        this.critcleDmg = critcleDmg;
        this.attackPattern = attackPattern;
        this.attackRange = attackRange;
        this.guardValue = guardValue;
        this.guardRecoverycoolTime = guardRecoverycoolTime;
        this.guardRecoveryValue = guardRecoveryValue;
    }
}

// 스크립터블 오브젝트를 생성하기 위한 UI를 등록
[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/EnemyDataReader", order = int.MaxValue)]
public class EnemyDataReader : DataReaderBase
{
    //시트에서 데이터를 읽으면 이 리스트에 저장
    [Header("스프레드시트에서 읽혀져 직렬화 된 오브젝트")][SerializeField] public List<EnemyData> DataList = new List<EnemyData>();
    //각 행을 읽을 때마다 데이터를 저장하기 위해 구조체를 생성하고 리스트에 삽입
    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        int id = 0, addMoney = 0, addExp = 0;
        string name = null, attackPattern = null;
        float hp = 0, attack = 0, defence = 0, critcleRate = 0, critcleDmg = 0, attackRange = 0, guardValue = 0, guardRecoverycoolTime = 0, guardRecoveryValue = 0;

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
                case "hp":
                    {
                        hp = float.Parse(list[i].value);
                        break;
                    }
                case "attack":
                    {
                        attack = float.Parse(list[i].value);
                        break;
                    }
                case "defence":
                    {
                        defence = float.Parse(list[i].value);
                        break;
                    }
                case "addMoney":
                    {
                        addMoney = int.Parse(list[i].value);
                        break;
                    }
                case "addExp":
                    {
                        addExp = int.Parse(list[i].value);
                        break;
                    }
                case "critcleRate":
                    {
                        critcleRate = float.Parse(list[i].value);
                        break;
                    }
                case "critcleDmg":
                    {
                        critcleDmg = float.Parse(list[i].value);
                        break;
                    }
                case "attackPattern":
                    {
                        attackPattern = list[i].value;
                        break;
                    }
                case "attackRange":
                    {
                        attackRange = float.Parse(list[i].value);
                        break;
                    }
                case "guardValue":
                    {
                        guardValue = float.Parse(list[i].value);
                        break;
                    }
                case "guardRecoverycoolTime":
                    {
                        guardRecoverycoolTime = float.Parse(list[i].value);
                        break;
                    }
                case "guardRecoveryValue":
                    {
                        guardRecoveryValue = float.Parse(list[i].value);
                        break;
                    }
            }
        }

        DataList.Add(new EnemyData(id, name, hp,attack,defence,addMoney,addExp,critcleRate,critcleDmg, attackPattern, attackRange, guardValue, guardRecoverycoolTime, guardRecoveryValue));
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(EnemyDataReader))]
public class EnemyDataReaderEditor : Editor
{
    EnemyDataReader data;

    void OnEnable()
    {
        data = (EnemyDataReader)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\n스프레드 시트 읽어오기");

        if (GUILayout.Button("데이터 읽기(API 호출)"))
        {
            data.DataList.Clear();           //  먼저 비움
            UpdateStats(UpdateMethodOne);      //  그 후 채움
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
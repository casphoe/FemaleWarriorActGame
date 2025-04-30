﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDataReader enemyDataReader;

    [Header("스테이지 별 적 그룹")]
    public GameObject[] enemyGroups; // 0: Stage1, 1: Stage2, 2: Stage3 등

    public static EnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ApplyEnemyDataToAll();
    }

    private void ApplyEnemyDataToAll()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        List<EnemyData> dataList = enemyDataReader.DataList;

        foreach (Enemy enemy in allEnemies)
        {
            EnemyData? data = FindMatchingData(enemy.id, dataList);

            if (data.HasValue)
            {
                enemy.Init(data.Value);
            }
            else
            {
                Debug.LogWarning($"'{enemy.id}' 에 해당하는 데이터가 없습니다.");
            }
        }

        //적들끼리 충돌 무시
        for (int i = 0; i < allEnemies.Length; i++)
        {
            Collider2D colA = allEnemies[i].GetComponent<Collider2D>();
            if (colA == null) continue;

            for (int j = i + 1; j < allEnemies.Length; j++)
            {
                Collider2D colB = allEnemies[j].GetComponent<Collider2D>();
                if (colB == null) continue;

                Physics2D.IgnoreCollision(colA, colB);
            }
        }
    }

    private EnemyData? FindMatchingData(int id, List<EnemyData> dataList)
    {
        foreach (EnemyData data in dataList)
        {
            if (data.id == id) // id 값을 토대로 데이터를 가져옴
                return data;
        }

        return null;
    }

    public void ActivateEnemies(int mapNum)
    {
        int stageIndex = GetStageIndexFromMapNum(mapNum);

        for (int i = 0; i < enemyGroups.Length; i++)
        {
            if (enemyGroups[i] != null)
                enemyGroups[i].SetActive(i == stageIndex);
        }
    }

    private int GetStageIndexFromMapNum(int mapNum)
    {
        switch (mapNum)
        {
            case 2: return 0; // Stage_1
            case 3: return 1; // Stage_2
            case 4: return 2; // Stage_3
            default: return -1; // 알 수 없는 맵번호
        }
    }
}

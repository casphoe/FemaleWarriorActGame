using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyDataReader enemyDataReader;

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
}

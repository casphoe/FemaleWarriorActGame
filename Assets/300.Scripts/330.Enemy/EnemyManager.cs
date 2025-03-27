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
                Debug.LogWarning($"'{enemy.id}' �� �ش��ϴ� �����Ͱ� �����ϴ�.");
            }
        }
    }

    private EnemyData? FindMatchingData(int id, List<EnemyData> dataList)
    {
        foreach (EnemyData data in dataList)
        {
            if (data.id == id) // id ���� ���� �����͸� ������
                return data;
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChestManager : MonoBehaviour
{
    [SerializeField] private TreasureChasetDataReader dataReader;

    private void Start()
    {
        ApplyTreasureChestDataToAll();
    }

    private void ApplyTreasureChestDataToAll()
    {
        TreasureChest[] allChest = FindObjectsOfType<TreasureChest>();

        List<CheastData> dataList = dataReader.CheastList;

        foreach (TreasureChest cheast in allChest)
        {
            CheastData? data = FindMatchingData(cheast.id, dataList);

            if (data.HasValue)
            {
                cheast.Init(data.Value);
            }
            else
            {
                Debug.LogWarning($"'{cheast.id}' 에 해당하는 데이터가 없습니다.");
            }
        }
    }

    private CheastData? FindMatchingData(int id, List<CheastData> dataList)
    {
        foreach (CheastData data in dataList)
        {
            if (data.id == id) // id 값을 토대로 데이터를 가져옴
                return data;
        }

        return null;
    }
}

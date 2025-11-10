using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private IEnumerator Start()
    {
        // EnemyDataReader가 로딩을 끝낼 때까지 대기 (필요 시)
        yield return new WaitUntil(() => enemyDataReader != null && enemyDataReader.DataList != null && enemyDataReader.DataList.Count > 0);

        // 비활성 포함 전체 적에 데이터 적용
        ApplyEnemyDataToAll_IncludingInactive();
        
        IgnoreCollisionsForCurrentlyActiveEnemies();
    }

    /// <summary>
    /// enemyGroups가 지정돼 있으면 그 하위에서, 아니면 씬 루트부터 비활성 포함 전부 스캔
    /// </summary>
    private List<Enemy> GetAllEnemies_IncludingInactive()
    {
        var result = new List<Enemy>();

        if (enemyGroups != null && enemyGroups.Length > 0 && enemyGroups.Any(g => g != null))
        {
            foreach (var root in enemyGroups)
            {
                if (root == null) continue;
                // true => 비활성 포함
                result.AddRange(root.GetComponentsInChildren<Enemy>(true));
            }
        }
        else
        {
            // 그룹을 안 쓰는 경우: 씬 루트에서 전체 탐색
            var activeScene = SceneManager.GetActiveScene();
            var roots = activeScene.GetRootGameObjects();
            foreach (var root in roots)
                result.AddRange(root.GetComponentsInChildren<Enemy>(true));
        }

        // 중복 제거(여러 그룹/루트에 포함된 경우 대비)
        return result.Distinct().ToList();
    }

    /// <summary>
    /// (옵션) 현재 '활성화된' 적들 사이의 콜라이더 충돌을 개별로 무시하고 싶을 때.
    /// 레이어 매트릭스를 이미 껐으면 보통 필요 없습니다.
    /// </summary>
    private void IgnoreCollisionsForCurrentlyActiveEnemies()
    {
        Enemy[] allEnemiesActiveOnly = FindObjectsOfType<Enemy>(); // 활성만
        for (int i = 0; i < allEnemiesActiveOnly.Length; i++)
        {
            var colA = allEnemiesActiveOnly[i].GetComponent<Collider2D>();
            if (colA == null) continue;

            for (int j = i + 1; j < allEnemiesActiveOnly.Length; j++)
            {
                var colB = allEnemiesActiveOnly[j].GetComponent<Collider2D>();
                if (colB == null) continue;
                Physics2D.IgnoreCollision(colA, colB, true);
            }
        }
    }

    private void ApplyEnemyDataToAll_IncludingInactive()
    {
        var allEnemies = GetAllEnemies_IncludingInactive();
        var dataList = enemyDataReader.DataList;

        foreach (var enemy in allEnemies)
        {
            EnemyData? data = FindMatchingData(enemy.id, dataList);
            if (data.HasValue)
            {
                // 비활성이어도 메서드 호출은 됩니다. Init 내부가 활성 상태를 가정하지 않도록만 유의.
                enemy.Init(data.Value);
            }
            else
            {
                Debug.LogWarning($"'{enemy.id}' 에 해당하는 데이터가 없습니다.");
            }
        }
    }

    private EnemyData? FindMatchingData(int id, List<EnemyData> dataList)
    {
        foreach (var data in dataList)
        {
            if (data.id == id) return data;
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

        // 스테이지 전환 시, 방금 켠 그룹의 비활성 적들도 이미 데이터는 들어가 있지만
        // 혹시 런타임에 프리팹에서 새로 인스턴스된 경우를 대비해 한 번 더 보정
        ApplyEnemyDataToAll_IncludingInactive();

        // (옵션) 개별 충돌 무시 보정
        // IgnoreCollisionsForCurrentlyActiveEnemies();
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

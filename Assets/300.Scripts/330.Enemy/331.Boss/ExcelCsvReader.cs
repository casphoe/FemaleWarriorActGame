using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum DataType { Boss }

public class ExcelCsvReader : MonoBehaviour
{
    [Header("에디터 전용: 드래그&드롭")]
    public TextAsset stringTableFile;  // BossStringTable.csv

    public DataType dataType;

    // key:value 형태로 저장
    public BossStringTable stringBossTable = new BossStringTable();

    [Header("Inspector에서 stringTableFile 확인용 리스트")]
    public List<BossStringTableRow> bossStringTableList = new List<BossStringTableRow>();

    [Header("빌드에서 폴더 상으로 csv 파일 업로드 하기 위해서 사용되는 변수")]
    public string bossTableFileName = string.Empty;
    public bool useStreamingAssets = false; // 에디터에서는 false, 빌드에서는 true 권장

    public static ExcelCsvReader instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 1. StringTable 먼저 로드
        string stringTableText = "";
        if (useStreamingAssets)
        {
            string path = Path.Combine(Application.streamingAssetsPath, bossTableFileName);
            if (!File.Exists(path))
            {
                Debug.LogWarning("[StringTable] StreamingAssets에서 파일을 찾을 수 없습니다: " + path);
                return;
            }
            stringTableText = File.ReadAllText(path, Encoding.UTF8);
        }
        else
        {
            if (stringTableFile == null)
            {
                Debug.LogWarning("[StringTable] Resources/TextAsset이 할당되지 않았습니다.");
                return;
            }
            stringTableText = stringTableFile.text;
        }
        stringBossTable.Load(stringTableText);

        bossStringTableList.Clear();

        foreach (var row in stringBossTable.rows.Values)
        {
            // 새 인스턴스로 복사 (필수)
            bossStringTableList.Add(new BossStringTableRow
            {
                Index = row.Index,
                Hp = row.Hp,
                Power = row.Power,
                Defense =row.Defense,
                GuardGauge = row.GuardGauge,
                NameKor = row.NameKor,
                NameEng = row.NameEng,
                AddCoin = row.AddCoin,
                AddExp = row.AddExp,
                StunRecoveryTimer = row.StunRecoveryTimer
            });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public enum DataType { Boss }

public class ExcelCsvReader : MonoBehaviour
{
    [Header("에디터 전용: 드래그&드롭")]
    public TextAsset csvFile;          // QuizTable.csv
    public TextAsset stringTableFile;  // StringTable.csv

    public DataType dataType;

    // key:value 형태로 저장
    public Dictionary<int, Boss> dicQuizCsvData = new Dictionary<int, Boss>();
    public BossStringTable stringTable = new BossStringTable();

    [Header("Inspector에서 확인용 리스트")]
    public List<BossDataEntry> debugQuizList = new List<BossDataEntry>();

    [Header("Inspector에서 stringTableFile 확인용 리스트")]
    public List<BossStringTableRow> debugQuizStringTableList = new List<BossStringTableRow>();

    [Header("빌드에서 폴더 상으로 csv 파일 업로드 하기 위해서 사용되는 변수")]
    public string quizTableFileName = string.Empty;
    public string stringTableFileName = string.Empty;
    public bool useStreamingAssets = false; // 에디터에서는 false, 빌드에서는 true 권장
}

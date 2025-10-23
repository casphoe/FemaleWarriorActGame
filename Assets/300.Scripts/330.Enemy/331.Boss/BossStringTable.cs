using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

//보스 엑셀 데이터를 가져올 스트링 테이블(엑셀 csv 파일로 불러옴)
[Serializable]
public class BossStringTableRow
{
    public int Index; //보스 번호
    public string NameEng; //보스 이름(영어)
    public string NameKor; //보스 이름(한글)
    public float Hp; //체력
    public float Power; //공격력
    public float Defense; //방어력
    public float GuardGauge; //가드 게이지
    public float StunRecoveryTimer;
    public int AddExp; //터치시 획득 경험치 양
    public int AddCoin; //터치시 획득 골드 양

    public BossStringTableRow(string[] data)
    {
        var C = CultureInfo.InvariantCulture;

        Index = int.Parse(data[0], C);
        NameEng = data[1];
        NameKor = data[2];
        Hp = float.Parse(data[3], C);
        Power = float.Parse(data[4], C);
        Defense = float.Parse(data[5], C);
        GuardGauge = float.Parse(data[6], C);
        StunRecoveryTimer = float.Parse(data[7], C);
        AddExp = int.Parse(data[8], C);
        AddCoin = int.Parse(data[9], C);
    }

    public BossStringTableRow() { }
}

public class BossStringTable
{
    public Dictionary<int, BossStringTableRow> rows = new Dictionary<int, BossStringTableRow>();

    public void Load(string csvText)
    {
        var lines = csvText.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            // 기존 코드:
            // var data = lines[i].Split(',');

            // 새 코드: 쉼표 포함도 안전하게
            var data = ParseCsvLine(lines[i]).ToArray();

            var row = new BossStringTableRow(data);
            rows[row.Index] = row;
        }
    }
    // 쉼표 포함 셀 지원하는 파서
    public static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string cur = "";
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"') // "" → "
                {
                    cur += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(cur);
                cur = "";
            }
            else
            {
                cur += c;
            }
        }
        result.Add(cur); // 마지막 데이터 추가
        return result;
    }
}

//서버로 관리 firebase 서버로 관리
[Serializable]
public class BossData
{
    public int Index; //보스 번호
    public bool IsClear; //터치했는지 여부 (경험치 골드를 한번만 얻게 하기 위해서)
    public bool IsTryAgain; //재도전 여부
}

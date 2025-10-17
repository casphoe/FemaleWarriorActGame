using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CSVReader
{
    // CSV를 읽고 Dictionary<int, T>로 반환
    public static Dictionary<TKey, T> ReadCSV<T, TKey>(string csvText)
        where T : ICSVParsable<TKey>, new()
    {
        var result = new Dictionary<TKey, T>();
        using (StringReader reader = new StringReader(csvText))
        {
            string line;
            reader.ReadLine(); // skip header
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var split = ParseCsvLine(line).ToArray();
                T entry = new T();
                entry.Parse(split);
                result.Add(entry.GetKey(), entry);
            }
        }
        return result;
    }

    // TextAsset 버전도 유지하고 싶으면 아래처럼 추가!
    public static Dictionary<TKey, T> ReadCSV<T, TKey>(TextAsset csvFile)
        where T : ICSVParsable<TKey>, new()
    {
        return ReadCSV<T, TKey>(csvFile.text);
    }

    // 쉼표, 따옴표 지원하는 커스텀 파서
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
        result.Add(cur); // 마지막 데이터
        return result;
    }
}

// CSV 파싱 가능한 객체를 위한 인터페이스
public interface ICSVParsable<TKey>
{
    void Parse(string[] data); // 데이터를 파싱하는 메서드
    TKey GetKey();             // 딕셔너리의 key로 사용할 값 반환
}
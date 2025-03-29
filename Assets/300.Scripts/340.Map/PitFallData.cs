using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
//함정 클래스
public class PitFallData 
{
    public string name = string.Empty;

    // 이름별 기본 데미지 테이블
    private static Dictionary<string, float> trapBaseDamages = new Dictionary<string, float>()
    {
        { "spiketrap", 10 },
        { "deephole", 15 },
        { "firepit", 20 }
    };

    public float difficultyMultiplier = 1;
    
    public float GetFinalDamage()
    {
        difficultyMultiplier = SetDifficulty();
        float baseDamage = GetBaseDamageByName();
        return baseDamage * difficultyMultiplier;
    }

    float SetDifficulty()
    {
        switch (GameManager.data.diffucity)
        {
            case Diffucity.Easy: return 0.5f;
            case Diffucity.Normal: return 1f;
            case Diffucity.Hard: return 1.5f;
            default: return 1f;
        }
    }

    float GetBaseDamageByName()
    {
        string key = name.ToLower();
        if (trapBaseDamages.TryGetValue(key, out float value))
        {
            return value;
        }       
        return 10f; // 기본값
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SerializeField]
public class GameManagerData
{
    [SerializeField]
    public Diffucity diffucity { set; get; }

    [SerializeField]
    public LANGUAGE lanauge { set; get; }

    [SerializeField]
    public CustomKeyCode keyCode { set; get; }

    [SerializeField]
    public Day day { set; get; }

    [SerializeField]
    public float totalPlayTime = 0;

    public int startNum;
    public int selectSlotNum;
    public int selectDiffucityNum;

    public float bgmSoundValue;
    public float effectSoundValue;

    public Dictionary<CustomKeyCode, KeyCode> keyMappings = new Dictionary<CustomKeyCode, KeyCode>();

    public string KeyMappingDataSavePath = Environment.CurrentDirectory + "/keyMappings.json";
    public string systemSavePath = Environment.CurrentDirectory + "/systemData.json";
}

/// <summary>
/// 난이도에 따라서 몬스터 강함 및 경험치 배수 돈 배수가 달라짐 난이도가 높으면 높을 수록 몬스터가 강해지고 
/// 경험치 배수 및 돈 배수가 내러감
/// </summary>
[SerializeField]
public enum Diffucity
{
    Easy,Normal,Hard
}

[SerializeField]
public enum Day
{
    Afternoon,Night
}


[SerializeField]
public enum CustomKeyCode
{
    Left,Right,Jump,Attack,Evasion, Equipment,PlayerInfo, Skill, Inventory,BlockKey, ShortcutKey1, ShortcutKey2, ShortcutKey3, ShortcutKey4, ShortcutKey5, ShortcutKey6, ShortcutKey7, ActionKey, PauseKey
}

[SerializeField]
public enum LANGUAGE
{
    KOR, ENG
}

public class GameManager 
{
    internal static GameManagerData data = new GameManagerData();

    internal static void SaveDataToIni()
    {
        File.WriteAllText(data.systemSavePath, GetSystemFileContent());
    }

    private static string GetSystemFileContent()
    {
        return
        "[설정]\n" +
        "[설정 데이터]\n" +
            $"LANGUAGE={data.lanauge}\n" +
             $"Diffucity={data.diffucity}\n" +
              $"Bgm={data.bgmSoundValue}\n" +
              $"Effect={data.effectSoundValue}\n";
    }

    internal static void LoadDataToIni()
    {
        if(File.Exists(data.systemSavePath))
        {
            string[] LoadSetting = File.ReadAllLines(data.systemSavePath);
            foreach (string dataStr in LoadSetting)
            {
                if (dataStr.StartsWith("LANGUAGE="))
                {
                    data.lanauge = ParseLanguageFromString(dataStr);
                }
                else if(dataStr.StartsWith("Diffucity="))
                {
                    data.diffucity = ParaseDiffucityFromString(dataStr);
                }
                else if (dataStr.StartsWith("Bgm="))
                {
                    data.bgmSoundValue = float.Parse(dataStr.Substring("Bgm=".Length));
                }
                else if (dataStr.StartsWith("Effect="))
                {
                    data.effectSoundValue = float.Parse(dataStr.Substring("Effect=".Length));
                }
            }
            
            switch(data.lanauge)
            {
                case LANGUAGE.KOR:
                    OptionManager.instance.LanaugeSetting(0);
                    break;
                case LANGUAGE.ENG:
                    OptionManager.instance.LanaugeSetting(1);
                    break;
            }

            switch(data.diffucity)
            {
                case Diffucity.Easy:
                    OptionManager.instance.GameOptionSetting(0);
                    break;
                case Diffucity.Normal:
                    OptionManager.instance.GameOptionSetting(1);
                    break;
                case Diffucity.Hard:
                    OptionManager.instance.GameOptionSetting(2);
                    break;
            }

            if (data.bgmSoundValue > 0)
            {
                OptionManager.instance.BgmSoundSetting(0);
            }
            else
            {
                OptionManager.instance.BgmSoundSetting(1);
            }


            if(data.effectSoundValue > 0)
            {
                OptionManager.instance.EffectSoundSetting(0);
            }
            else
            {
                OptionManager.instance.EffectSoundSetting(1);
            }
        }
        else
        {
            OptionManager.instance.GameOptionSetting(1);
            OptionManager.instance.LanaugeSetting(0);
            OptionManager.instance.BgmSoundSetting(0);
            OptionManager.instance.EffectSoundSetting(0);
        }
    }

    public static LANGUAGE ParseLanguageFromString(string data)
    {
        string parseString = data.Substring("LANGUAGE=".Length);
        LANGUAGE parseLanuage;
        if (Enum.TryParse(parseString, out parseLanuage))
        {
            return parseLanuage;
        }
        else
        {
            return LANGUAGE.KOR;
        }
    }

    public static Diffucity ParaseDiffucityFromString(string data)
    {
        string parseString = data.Substring("Diffucity=".Length);
        Diffucity parseDiffucity;
        if (Enum.TryParse(parseString, out parseDiffucity))
        {
            return parseDiffucity;
        }
        else
        {
            return Diffucity.Normal;
        }
    }
}

[Serializable]
public class KeyMapping
{
    public CustomKeyCode customKeyCode;
    public KeyCode keyCode;
}

[Serializable]
public class KeyMappingsData
{
    public KeyMapping[] keyMappings;
}
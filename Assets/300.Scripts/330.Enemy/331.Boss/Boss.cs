using UnityEngine;

[System.Serializable]
public class Boss : ICSVParsable<int>
{
    public int BossId;

    public string NameEng; //보스 이름(영어)
    public string NameKor; //보스 이름(한글)
    public float Hp; //체력
    public float Power; //공격력
    public float Defense; //방어력
    public float GuardGauge; //가드 게이지
    public float StunRecoveryTimer;
    public int AddExp; //터치시 획득 경험치 양
    public int AddCoin; //터치시 획득 골드 양

    public int GetKey() => BossId;

    public void Parse(string[] data)
    {
        BossId = int.Parse(data[0]);
    }
}

[System.Serializable]
public class BossDataEntry
{
    public int key;
    public Boss value;
}

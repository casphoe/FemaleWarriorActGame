using UnityEngine;

[System.Serializable]
public class Boss : ICSVParsable<int>
{
    public int BossId;

    public string NameEng; //���� �̸�(����)
    public string NameKor; //���� �̸�(�ѱ�)
    public float Hp; //ü��
    public float Power; //���ݷ�
    public float Defense; //����
    public float GuardGauge; //���� ������
    public float StunRecoveryTimer;
    public int AddExp; //��ġ�� ȹ�� ����ġ ��
    public int AddCoin; //��ġ�� ȹ�� ��� ��

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

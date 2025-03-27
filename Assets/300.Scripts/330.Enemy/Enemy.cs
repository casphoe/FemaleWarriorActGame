using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,Attack,Death,Pharse
}

public class Enemy : MonoBehaviour
{
    public int id;

    private float hp;
    private float attack;
    private float defence;
    private int addMoney;
    private int addExp;

    public void Init(EnemyData data)
    {
        id = data.id;
        hp = data.hp;
        attack = data.attack;
        defence = data.defence;
        addMoney = data.addMoney;
        addExp = data.addExp;
    }
}

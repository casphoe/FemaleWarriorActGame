using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Patrol, Attack,Death, Chase
}

public class Enemy : MonoBehaviour
{
    //0 : 슬라임, 1 : 박쥐, 2 : 쥐, 3: 동굴게, 4 : 이블아이, 5 : 머시룸, 6 : 고블린, 7 : 해골, 8 : 스켈레톤 , 9 : 골렘, 10 : 갑옷 골렘
    public int id;
    [Header("구글 스프레트 시트에서 가져온 적 데이터")]
    public float hp;
    public float attack;
    public float defence;
    public int addMoney;
    public int addExp;
    [Header("난이도에 따른 적 데이터")]
    public float currentHp;
    public float currentAttack;
    public float currentDefence;
    public int currentAddMoney;
    public int currentAddExp;

    public State enemyState = State.Patrol;

    private Transform player;
    private Animator anim;
    private SpriteRenderer sprite;
    #region 적 데이터 관리 
    //EnemyManager에서 적 데이터 가져오는 함수
    public void Init(EnemyData data)
    {
        id = data.id;
        hp = data.hp;
        attack = data.attack;
        defence = data.defence;
        addMoney = data.addMoney;
        addExp = data.addExp;
        EnemyDifficuitySetting();
    }
    //난이도에 따른 적 데이터 설정
    void EnemyDifficuitySetting()
    {
        float multiplier = 1f;

        switch (GameManager.data.diffucity)
        {
            case Diffucity.Easy:
                multiplier = 0.5f;
                break;
            case Diffucity.Normal:
                multiplier = 1f;
                break;
            case Diffucity.Hard:
                multiplier = 1.5f;
                break;
        }

        currentHp = hp * multiplier;
        currentAttack = attack * multiplier;
        currentDefence = defence * multiplier;

        currentAddMoney = Mathf.RoundToInt(addMoney * multiplier);
        currentAddExp = Mathf.RoundToInt(addExp * multiplier);
    }
    #endregion

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
}
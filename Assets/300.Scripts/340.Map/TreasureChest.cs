using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//보물 상자 관련 스크립트 & 액션키로 열수 있습니다 한번 열면 다시 새로 시작하지 않는 이상 똑같은 보물상자는 열수 없습니다.
public class TreasureChest : MonoBehaviour
{
    private Animator anim;

    private bool isOpened = false;
    private bool isPlayerNear = false;


    [Header("구글 스프레트 시트")]
    public int id;
    public string skill;
    public float addHp;
    public float addStamina;
    public int gold;
    public int exp;

    public void Init(CheastData data)
    {
        id = data.id;
        skill = data.skill;
        gold = data.gold;
        exp = data.exp;
        addHp = data.addhp;
        addStamina = data.addstamina;
    }


    private void Awake()
    {
        anim = this.transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        if (isOpened || !isPlayerNear)
            return;

        if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ActionKey]))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        anim.SetBool("IsOpen", true);
        PlayerManager.instance.AddExp(exp);
        PlayerManager.instance.AddMoney(gold);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}

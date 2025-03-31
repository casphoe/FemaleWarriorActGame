using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 공격 함수
public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;

    private int facingDir = -1;

    public void AttackHit()
    {
        //기준점 : 플레이어 위치 + 방향 * 거리
        Vector2 attackCenter = (Vector2)transform.position + new Vector2(facingDir * 0.5f, 0f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCenter, attackRange, Player.instance.enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(PlayerManager.instance.player.attack, PlayerManager.instance.player.critcleRate, PlayerManager.instance.player.critcleDmg);
        }
    }


    public void SetFacingDir(float x)
    {
        facingDir = x > 0 ? 1 : -1;
    }
}

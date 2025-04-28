using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrapHit : MonoBehaviour
{
    public float knockbackForce = 10f; // 튕겨내는 힘

    string nameStr = string.Empty;

    PitFallData data = new PitFallData();

    float damge = 0;

    Player player;

    private void Awake()
    {
        nameStr = gameObject.name;
        data.name = nameStr;
        damge = data.GetFinalDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            player = collision.GetComponent<Player>();
            if (player.rb != null)
            {
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(hitDirection * knockbackForce, ForceMode2D.Impulse);
            }

            // 1) 함정에 부딪힌 순간 데미지
            player = collision.GetComponent<Player>();
            player.TakeDamage(damge, 0, 0, 0);

            // 2) 튕겨날 때 힘 저장 (찾기 데미지용)
            PlayerFallDamage fallDamage = collision.GetComponent<PlayerFallDamage>();
            if (fallDamage != null)
            {
                fallDamage.StoreKnockbackForce(knockbackForce);
            }
        }

        if(collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.RockTrapHit();
            }
        }
    }
}

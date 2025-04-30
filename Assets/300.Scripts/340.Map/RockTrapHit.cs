using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTrapHit : MonoBehaviour
{
    public float knockbackForce = 12.5f; // 튕겨내는 힘

    string nameStr = string.Empty;

    PitFallData data = new PitFallData();

    BoxCollider2D boxCollider;

    float damge = 0;

    Player player;

    LayerMask enemyLayer; // Enemy 레이어 마스크

    private void Awake()
    {
        nameStr = gameObject.name;
        data.name = nameStr;
        damge = data.GetFinalDamage(); //데미지 추출
        enemyLayer = LayerMask.GetMask("Enemy"); // "Enemy" 레이어를 인식하도록 설정
        boxCollider = GetComponent<BoxCollider2D>(); //박스 콜라이더를 가져옴
    }

    private void Update()
    {
        if (boxCollider == null) return;

        Vector2 boxSize = boxCollider.size; //콜라이더 사이즈
        Vector2 boxCenter = (Vector2)transform.position + boxCollider.offset; //박스 콜라이더의 실제 중심 위치 계산

        //현재 박스 범위 안에 있는 적들을 실시간 탐지
        Collider2D[] enemies = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, enemyLayer);

        foreach (var enemyCol in enemies)
        {
            Enemy enemy = enemyCol.GetComponent<Enemy>();
            if (enemy != null && !enemy.wasHitByTrap)
            {
                enemy.wasHitByTrap = true;
                enemy.RockTrapHit();
            }
        }
    }

    //충돌 하려고하는 객체 또는 지금 객체 둘중 하나에 RigidyBody가 있어야지 충돌 체크가 됩니다. 둘다 없으면 위에 업데이트 문을 통해서 충돌 체크 해주시면 됩니다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // (1) 죽었거나 무적이면 그냥 return
            if (PlayerManager.instance.IsDead || PlayerManager.instance.isInvisble)
            {
                return;
            }

            player = collision.GetComponent<Player>();
            if (player.rb != null)
            {
                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;

                // (1) 위쪽 방향 추가해서 위로 확 튕기게
                Vector2 knockbackDirection = (hitDirection + Vector2.up * 0.9f).normalized;

                player.rb.velocity = Vector2.zero;
                player.rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                player.isKnockback = true; // << 튕김 시작
                player.StartCoroutine(player.EndKnockback(1));
            }

            // 1) 함정에 부딪힌 순간 데미지
            player = collision.GetComponent<Player>();
            player.TakeDamage(damge, 0, 1.1f, 0);

            // 2) 튕겨날 때 힘 저장 (찾기 데미지용)
            PlayerFallDamage fallDamage = collision.GetComponent<PlayerFallDamage>();
            if (fallDamage != null)
            {
                fallDamage.StoreKnockbackForce(knockbackForce);
            }
        }
    }
}

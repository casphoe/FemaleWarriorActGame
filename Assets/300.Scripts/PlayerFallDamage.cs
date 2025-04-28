using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDamage : MonoBehaviour
{
    public LayerMask groundLayer; // 땅 레이어

    public float damageMultiplier = 1.0f; // 저장된 튕긴 힘에 곱할 배율

    private float storedKnockbackForce = 0f;
    private bool hasStoredForce = false;

    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void StoreKnockbackForce(float force)
    {
        storedKnockbackForce = force;
        hasStoredForce = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasStoredForce && ((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            float damage = storedKnockbackForce * damageMultiplier;

            if (player != null)
            {
                player.TakeDamage(damage,0,0,0);
            }

            hasStoredForce = false;
            storedKnockbackForce = 0f;
        }
    }
}

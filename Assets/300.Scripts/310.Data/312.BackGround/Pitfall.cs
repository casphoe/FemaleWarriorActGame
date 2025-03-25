using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    string name = string.Empty;

    PitFallData data = new PitFallData();

    float damge = 0;

    private void Awake()
    {
        name = gameObject.name;
        data.name = name;
        damge = data.GetFinalDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damge);
            }
        }
    }
}

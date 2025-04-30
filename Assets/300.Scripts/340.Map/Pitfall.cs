using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    string nameStr = string.Empty;

    PitFallData data = new PitFallData();

    float damge = 0;

    bool isPlayerCollision = false;

    Player player;

    private void Awake()
    {
        nameStr = gameObject.name;
        data.name = nameStr;
        damge = data.GetFinalDamage();
        isPlayerCollision = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerCollision = true;
            player = collision.GetComponent<Player>();
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerCollision = false;
            player = null;
        }
    }

    private void Update()
    {
        if(isPlayerCollision == true)
        {
            if(player != null)
            {
                player.TakeDamage(damge, 0, 1.1f, 0);
            }
        }
    }
}

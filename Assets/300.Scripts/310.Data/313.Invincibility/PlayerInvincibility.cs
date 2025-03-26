using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//캐릭터 무적효과 기능
public class PlayerInvincibility : MonoBehaviour
{
    public float invincibilityDuration = 2f;       // 무적 지속 시간
    public float flashInterval = 0.1f;             // 깜빡이는 간격

    private SpriteRenderer render;
    private Player player;


    private void Awake()
    {
        player = GetComponent<Player>();
        render = GetComponent<SpriteRenderer>();
    }

    public void TriggerInvincibility()
    {
        if (!PlayerManager.instance.isInvincibility)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        PlayerManager.instance.isInvincibility = true;

        float timer = 0f;
        //무적 지속 시간동안 반복
        while (timer < invincibilityDuration)
        {
            //캐릭터 스프라이트를 보였다 안 보였다를 반복해서 깜빡이는 효과를 연출 했습니다.
            render.enabled = false;
            yield return new WaitForSeconds(flashInterval);
            render.enabled = true;
            yield return new WaitForSeconds(flashInterval);
            timer += flashInterval * 2;
        }

        PlayerManager.instance.isInvincibility = false;
    }
}

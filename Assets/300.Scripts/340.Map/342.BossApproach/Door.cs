using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isPlayerOpen = false;
    bool isPlayerEntry = false;

    SpriteRenderer render;

    [SerializeField] Sprite[] doorChangeSprite = new Sprite[2];

    private void Awake()
    {
        isPlayerOpen = false;
        isPlayerEntry = false;
        render = GetComponent<SpriteRenderer>();
        render.sprite = doorChangeSprite[0];
    }

    IEnumerator FadeOutSprite(float duration)
    {
        Color c = render.color;

        float startAlpha = c.a;
        float t = 0;
        while(t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 0f, t / duration);
            render.color = c;
            yield return null;
        }
        c.a = 0f;
        render.color = c;
    }

    IEnumerator FadeInSprite(float duration)
    {
        Color c = render.color;

        float startAlpha = c.a;
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 1f, t / duration); // 목표값을 1로!
            render.color = c;
            yield return null;
        }
        c.a = 1f;
        render.color = c;
    }

    IEnumerator FadeOutSequesne()
    {
        isPlayerEntry = false;

        yield return StartCoroutine(FadeOutSprite(2));

        render.sprite = doorChangeSprite[1];

        yield return StartCoroutine(FadeInSprite(2));

        isPlayerOpen = true;
    }

    IEnumerator EntryFadeOutSequesne()
    {
        yield return StartCoroutine(FadeOutSprite(2));

        render.sprite = doorChangeSprite[0];

        yield return StartCoroutine(FadeInSprite(2));

        isPlayerEntry = true;
        isPlayerOpen = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerOpen)
            return;

        if(collision.gameObject.CompareTag("Player"))
        {
            if(PlayerManager.GetCustomKeyDown(CustomKeyCode.ActionKey))
            {
                StartCoroutine(FadeOutSequesne());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isPlayerEntry)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if(isPlayerOpen)
            {
                StartCoroutine(EntryFadeOutSequesne());
            }
        }
    }
}

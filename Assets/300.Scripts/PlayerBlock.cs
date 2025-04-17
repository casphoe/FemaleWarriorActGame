using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    public GameObject shieldVisual;
    public GameObject blockSpark;

    public bool isBlocking = false;

    private float blockDuration = 1.5f;
    private float blockTimer = 0;

    private SpriteRenderer shieldRenderer;
    private Color initialColor;

    private SpriteRenderer blockSparkRender;
    private Color sparkInitialColor;
    private float blockStaminaCost = 2;

    private void Awake()
    {
        isBlocking = false;
        if (shieldVisual != null)
        {
            shieldRenderer = shieldVisual.GetComponent<SpriteRenderer>();
            initialColor = shieldRenderer.color;
        }

        if (blockSpark != null)
        {
            blockSparkRender = blockSpark.GetComponent<SpriteRenderer>();
            sparkInitialColor = blockSparkRender.color;
        }

        Utils.OnOff(shieldVisual, false);
        Utils.OnOff(blockSpark, false);
    }

    private void Update()
    {
        if(PlayerManager.instance.isStun == false)
        {
            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.BlockKey) && !isBlocking && Player.instance.currentStamina >= blockStaminaCost)
            {
                StartBlock();
            }


            if (isBlocking)
            {
                blockTimer -= Time.deltaTime;
                // 알파값을 blockTimer에 비례해서 감소
                float alpha = Mathf.Clamp01(blockTimer / blockDuration);

                if (shieldRenderer != null)
                {
                    Color newColor = initialColor;
                    newColor.a = alpha;
                    shieldRenderer.color = newColor;
                }

                if (blockTimer <= 0)
                {
                    EndBlock();
                }
            }
        }
    }

    void StartBlock()
    {
        isBlocking = true;
        blockTimer = blockDuration;
        Player.instance.currentStamina -= blockStaminaCost;
        GameCanvas.instance.SliderChange(1, 1, blockStaminaCost);
        if (shieldRenderer != null)
        {
            Color c = shieldRenderer.color;
            c.a = 1f;
            shieldRenderer.color = c;
        }

        Utils.OnOff(shieldVisual, true);
    }

    void EndBlock()
    {
        isBlocking = false;
        Utils.OnOff(shieldVisual, false);
    }

    public void OnBlocked()
    {
        Utils.OnOff(blockSpark, true);

        if (blockSparkRender != null)
        {
            Color c = blockSparkRender.color;
            c.a = 1f;
            blockSparkRender.color = c;
        }

        // 기존 코루틴이 있다면 멈추고 다시 시작
        StopCoroutine("SparkFadeCoroutine");
        StartCoroutine("SparkFadeCoroutine");
    }

    private IEnumerator SparkFadeCoroutine()
    {
        float duration = 0.5f; // 이펙트가 사라지는 시간
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);

            if (blockSparkRender != null)
            {
                Color c = blockSparkRender.color;
                c.a = alpha;
                blockSparkRender.color = c;
            }

            yield return null;
        }

        Utils.OnOff(blockSpark, false);
    }
}

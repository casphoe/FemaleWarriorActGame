using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuardShrinkSlider : MonoBehaviour
{
    [Range(0f, 1f)] float setValue = 1f; // 슬라이더 값 (0~1)

    [Header("UI Elements")]
    public RectTransform fillBar; // 주황색 바 이미지

    private Vector2 originalSize;

    private void Start()
    {
        if(fillBar != null)
        {
            //슬라이더의 원래 사이즈
            originalSize = fillBar.sizeDelta;
        }
        SetValue(1f);
    }
    //value 값을 통해서 0~1 범위로 UI 조절
    public void SetValue(float value)
    {
        setValue = Mathf.Clamp01(value);
        UpdateFill();
    }

    void UpdateFill()
    {
        if (fillBar == null) return;

        // scaleX만 줄이되, 가운데 기준으로
        float width = originalSize.x * setValue;
        fillBar.sizeDelta = new Vector2(width, originalSize.y);

        // 위치 조정 (Pivot이 중앙일 경우 불필요함)
        fillBar.anchoredPosition = Vector2.zero;
    }
}

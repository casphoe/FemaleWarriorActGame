using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAcceptPlus : MonoBehaviour
{
    public GameObject qusetObject;
    public GameObject canvas;
    public RectTransform rectBuyCanvas;

    public float height = 1.7f;

    public Camera uiCamera;

    private void Start()
    {
        rectBuyCanvas = Instantiate(qusetObject, canvas.transform).GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
        Vector2 canvasPos;

        // 월드 좌표 → 캔버스 내 로컬 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            Camera.main.WorldToScreenPoint(worldPos),
            uiCamera,
            out canvasPos
        );

        rectBuyCanvas.anchoredPosition = canvasPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertPanel : MonoBehaviour
{
    public GameObject createPanel;
    public GameObject canvas;

    public RectTransform rectBlessCanvas;

    public float height = 1.6f;

    public Camera uiCamera;
    public string parentName;

    public AlertText alertText;

    private void Awake()
    {
        parentName = this.gameObject.transform.parent.name;
        rectBlessCanvas = Instantiate(createPanel, canvas.transform).GetComponent<RectTransform>();
        alertText = rectBlessCanvas.transform.GetChild(0).GetComponent<AlertText>();
        alertText.createParentName = parentName;
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

        rectBlessCanvas.anchoredPosition = canvasPos;
    }
}

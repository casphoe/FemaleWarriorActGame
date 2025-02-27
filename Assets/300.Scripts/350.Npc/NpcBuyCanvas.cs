using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBuyCanvas : MonoBehaviour
{
    public GameObject buyObject;
    public GameObject canvas;

    public RectTransform rectBuyCanvas;

    public float height = 1.7f;

    private void Start()
    {
        rectBuyCanvas = Instantiate(buyObject, canvas.transform).GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 _buyPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        rectBuyCanvas.position = _buyPos;
    }
}

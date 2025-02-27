using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessPanel : MonoBehaviour
{
    public GameObject createPanel;
    public GameObject canvas;

    public RectTransform rectBlessCanvas;

    public float height = 2;

    private void Start()
    {
        rectBlessCanvas = Instantiate(createPanel, canvas.transform).GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 _buyPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0));
        rectBlessCanvas.position = _buyPos;
    }
}

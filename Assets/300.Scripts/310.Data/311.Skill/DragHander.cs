using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragHander : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    bool isDragging = false;
    Canvas canvas;
    RectTransform skillRectTransform;

    public void IsStart(bool _drag, Canvas _canvas, RectTransform _transform)
    {
        isDragging = _drag;
        canvas = _canvas;
        skillRectTransform = _transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging == true)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

            skillRectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        // UI ���� ���� (Raycast ��� EventSystem ���)
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

        bool isPlaced = false;

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("SkillSlot"))
            {
                // ���Կ� ��ġ
                //skillRectTransform.SetParent(result.gameObject.transform);
                skillRectTransform.anchoredPosition = Vector2.zero;
                isPlaced = true;
                break;
            }
        }

        if (!isPlaced)
        {
            // ������ ������ ����
            Destroy(this.gameObject);
        }
    }
}

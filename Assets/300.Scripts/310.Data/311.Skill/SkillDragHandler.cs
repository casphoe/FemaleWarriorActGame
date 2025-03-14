using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//액티브 스킬 드래그 해주는 함수
public class SkillDragHandler : MonoBehaviour, IPointerDownHandler
{
    GameObject skillInstance; // 생성된 스킬 아이콘 오브젝트
    RectTransform skillRectTransform;
    Canvas canvas;

    [SerializeField] Image skillImage;
    [SerializeField] GameObject dragSkillObject;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        skillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartSkillDrag(eventData); // 마우스 클릭 위치 전달
    }

    public void StartSkillDrag(PointerEventData eventData = null)
    {
        if (dragSkillObject != null)
        {
            // 새로운 오브젝트 생성
            skillInstance = Instantiate(dragSkillObject, canvas.transform);
            skillRectTransform = skillInstance.GetComponent<RectTransform>();

            skillInstance.transform.SetParent(canvas.transform, false);
            if (eventData != null)
            {
                // 마우스 클릭한 위치를 UI 좌표로 변환 (Canvas Scaler 보정 포함)
                Vector2 localPoint;
                RectTransform canvasRect = canvas.GetComponent<RectTransform>();

                bool isValid = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasRect, eventData.position, eventData.pressEventCamera, out localPoint);

                if (isValid)
                {
                    // UI 위치에 오브젝트 생성
                    skillRectTransform.anchoredPosition = localPoint;
                }
                else
                {
                    Debug.LogWarning("ScreenPointToLocalPointInRectangle 변환 실패");
                }

                // UI 위치에 오브젝트 생성
                skillRectTransform.anchoredPosition = localPoint;
            }
            skillInstance.transform.GetChild(0).GetComponent<Image>().sprite = skillImage.sprite;
            skillInstance.transform.GetComponent<DragHander>().IsStart(true, canvas, skillRectTransform);
        }
    }
}

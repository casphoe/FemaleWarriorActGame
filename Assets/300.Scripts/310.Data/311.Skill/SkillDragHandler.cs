using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//액티브 스킬 드래그 해주는 함수
public class SkillDragHandler : MonoBehaviour
{
    GameObject skillInstance; // 생성된 스킬 아이콘 오브젝트
    RectTransform skillRectTransform;
    Canvas canvas;

    Image skillImage;
    [SerializeField] GameObject dragSkillObject;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        skillImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void CreateSkillInstance()
    {
        if (dragSkillObject != null)
        {
            // 새로운 오브젝트 생성
            GameObject skillInstance = Instantiate(dragSkillObject, canvas.transform);
            RectTransform skillRectTransform = skillInstance.GetComponent<RectTransform>();

            // 클릭한 버튼의 World Position을 그대로 적용
            skillRectTransform.position = transform.position;

            // 클릭한 버튼의 이미지 적용 (스킬 아이콘)
            skillInstance.transform.GetChild(0).GetComponent<Image>().sprite = skillImage.sprite;
            skillInstance.transform.GetComponent<DragHander>().IsStart(true, canvas, skillRectTransform);
        }
    }
}

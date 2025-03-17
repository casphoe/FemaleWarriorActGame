using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragHander : MonoBehaviour, IDragHandler, IEndDragHandler
{
    bool isDragging = false;
    Canvas canvas;
    RectTransform skillRectTransform;
    public Vector3 offset;

    Image image;

    string setSkillName = string.Empty;

    GameObject _slotOption;

    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }
    //생성될 때 포지션 캔버스등 사용할 변수 설정하는 함수
    public void Init(Canvas _canvas, RectTransform _transform, string _name, GameObject _slot)
    {
        canvas = _canvas;
        skillRectTransform = _transform;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvas.planeDistance));
        offset = skillRectTransform.position - mouseWorldPosition;
        setSkillName = _name;
        _slotOption = _slot;
    }

    void OnSlotUIChange(int num)
    {
        _slotOption.transform.GetChild(num).transform.GetChild(0).GetComponent<Image>().sprite = null;
        Utils.OnOff(_slotOption.transform.GetChild(num).transform.GetChild(0).gameObject, false);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (skillRectTransform != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, canvas.planeDistance));
            worldPosition.z = 0f;

            // offset을 고려하여 정확한 위치 유지
            skillRectTransform.position = worldPosition + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // UI 슬롯 감지 (EventSystem 활용)
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);      

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("SkillSlot"))
            {
                // 슬롯에 배치
                Utils.OnOff(result.gameObject.transform.GetChild(0).gameObject, true);
                result.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = image.sprite;

                //accquisitionSkillDataList 에서 등록된 스킬 찾기
                SkillData skillToEquip = PlayerManager.instance.player.skill.accquisitionSkillDataList.Find(skill => skill.nameKor == setSkillName);

                if(skillToEquip != null)
                {
                    SkillEquipPosition newPosition = SkillEquipPosition.None;

                    switch (result.gameObject.transform.name)
                    {
                        case "Slot_2":
                            newPosition = SkillEquipPosition.three;
                            break;
                        case "Slot_3":
                            newPosition = SkillEquipPosition.four;
                            break;
                        case "Slot_4":
                            newPosition = SkillEquipPosition.five;
                            break;
                        case "Slot_5":
                            newPosition = SkillEquipPosition.six;
                            break;
                        case "Slot_6":
                            newPosition = SkillEquipPosition.seven;
                            break;
                    }

                     //기존에 같은 슬롯에 배치된 스킬이 있다면 제거 해야함
                     SkillData existingSkill = PlayerManager.instance.player.skill.accquisitionSkillDataList
                    .Find(skill => skill.equipPostion == newPosition);

                    if (existingSkill != null)
                    {
                        switch(existingSkill.equipPostion)
                        {
                            case SkillEquipPosition.three:
                                OnSlotUIChange(2);
                                break;
                            case SkillEquipPosition.four:
                                OnSlotUIChange(3);
                                break;
                            case SkillEquipPosition.five:
                                OnSlotUIChange(4);
                                break;
                            case SkillEquipPosition.six:
                                OnSlotUIChange(5);
                                break;
                            case SkillEquipPosition.seven:
                                OnSlotUIChange(6);
                                break;
                        }
                        existingSkill.equipPostion = SkillEquipPosition.None;
                    }

                    skillToEquip.equipPostion = newPosition;

                    Destroy(this.gameObject);
                }
                break;
            }
        }
    }
}

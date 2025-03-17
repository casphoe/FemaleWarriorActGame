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

    public GameObject _slotOption;

    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
    }
    //생성될 때 포지션 캔버스등 사용할 변수 설정하는 함수
    public void Init(Canvas _canvas, RectTransform _transform, string _name, GameObject _slot)
    {
        canvas = _canvas;
        skillRectTransform = _transform;
        //현재 가져운 마우스 위치를 월드 좌표로 변환 하는 작업
        //마우스 위치가 픽셀 단위 이기 때문에 UI 오브젝트는 월드 좌표를 사용하기 때문에 월드 좌표로 변환하기 위해서 사용
        //canvas.planeDistance : 카메라로 부터 UI 캔버스 까지의 거리를 z값으로 사용해서 2d ui를 정확한 월드 좌표로 변환
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, canvas.planeDistance));
        offset = skillRectTransform.position - mouseWorldPosition;
        setSkillName = _name;
        _slotOption = _slot;
    }

    //스킬 프리팹 드래그중 실행되는 함수
    public void OnDrag(PointerEventData eventData)
    {
        if (skillRectTransform != null)
        {
            //마우스 좌표를 월드 좌표로 변환 ,eventData : 현재 마우스 좌표, canvas.planeDistance : UI 캔버스와 카메라 사이의 거리
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, canvas.planeDistance));
            worldPosition.z = 0f;

            // offset을 고려하여 정확한 위치 유지
            skillRectTransform.position = worldPosition + offset;
        }
    }

    //드래그 끝난 후 실행 되는 함수
    public void OnEndDrag(PointerEventData eventData)
    {
        // UI 슬롯 감지 (EventSystem 활용)
        //마우스 클릭 위치를 설정해서 UI 감지 하기 위해서 사용
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        //마우스 클릭한 위치 화면 좌표
        pointerEventData.position = eventData.position;
        //RayCast을 통해서 UI 오브젝트를 감지 합니다.
        List<RaycastResult> results = new List<RaycastResult>();
        //현재 마우스 클릭 위치에서 모든 UI 요소를 감지해서 results 에 저장하는 기능
        EventSystem.current.RaycastAll(pointerEventData, results);      

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("SkillSlot"))
            {
                //accquisitionSkillDataList 에서 등록된 스킬 찾기
                SkillData skillToEquip = PlayerManager.instance.player.skill.accquisitionSkillDataList.Find(skill => skill.nameKor == setSkillName);

                if(skillToEquip != null)
                {
                    SkillEquipPosition newPosition = GetSkillEquipPosition(result.gameObject.transform.name);

                    //기존에 같은 슬롯에 배치된 스킬이 있다면 제거 해야함(다른 위치에 잇는 중복된 스킬도 제거해야함)
                    foreach (SkillData existingSkill in PlayerManager.instance.player.skill.accquisitionSkillDataList)
                    {
                        if (existingSkill.nameKor == skillToEquip.nameKor && existingSkill.equipPostion != SkillEquipPosition.None)
                        {
                            // 기존 등록된 위치 초기화
                            ClearSlotUI(existingSkill.equipPostion);
                            existingSkill.equipPostion = SkillEquipPosition.None;
                        }
                    }
                    // 새로운 슬롯에 스킬 등록
                    skillToEquip.equipPostion = newPosition;

                    UpdateSlotUI(result.gameObject, image.sprite);
                 
                    Destroy(this.gameObject);
                }
                break;
            }
        }
    }

    // 슬롯 이름에 따라 SkillEquipPosition 값 반환
    private SkillEquipPosition GetSkillEquipPosition(string slotName)
    {
        switch (slotName)
        {
            case "Slot_2": return SkillEquipPosition.three;
            case "Slot_3": return SkillEquipPosition.four;
            case "Slot_4": return SkillEquipPosition.five;
            case "Slot_5": return SkillEquipPosition.six;
            case "Slot_6": return SkillEquipPosition.seven;
            default: return SkillEquipPosition.None;
        }
    }

    // 기존 스킬 UI 제거(기존 슬롯을 초기화)
    private void ClearSlotUI(SkillEquipPosition position)
    {
        int slotIndex = GetSlotIndex(position);
        if (slotIndex != -1)
        {
            Transform slotTransform = _slotOption.transform.GetChild(slotIndex).transform.GetChild(0);
            slotTransform.GetComponent<Image>().sprite = null;
            Utils.OnOff(slotTransform.gameObject, false);
        }
    }

    // 새 슬롯 UI 업데이트 함수
    void UpdateSlotUI(GameObject slot, Sprite skillSprite)
    {
        Transform slotTransform = slot.transform.GetChild(0);
        slotTransform.gameObject.SetActive(true);
        slotTransform.GetComponent<Image>().sprite = skillSprite;
    }

    // SkillEquipPosition 값에 따라 해당하는 슬롯 인덱스 반환
    private int GetSlotIndex(SkillEquipPosition position)
    {
        switch (position)
        {
            case SkillEquipPosition.three: return 2;
            case SkillEquipPosition.four: return 3;
            case SkillEquipPosition.five: return 4;
            case SkillEquipPosition.six: return 5;
            case SkillEquipPosition.seven: return 6;
            default: return -1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerClickHandler
{
    public string skillName = string.Empty;
    int slotIndex = 0;

    Image img;
    private void Awake()
    {
        img = transform.GetChild(0).GetComponent<Image>();
        slotIndex = transform.GetSiblingIndex();
        SkillSlotNum(slotIndex);
    }


    void SkillSlotNum(int index)
    {
        switch(index)
        {
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }

    void Update()
    {
        SkillSlotInput(slotIndex);
    }

    void SkillSlotInput(int index)
    {
        if (skillName != string.Empty)
        {
            switch (index)
            {
                case 2:
                    if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey3]))
                    {
                        Debug.Log(2);
                    }
                    break;
                case 3:
                    if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey4]))
                    {
                        Debug.Log(3);
                    }
                    break;
                case 4:
                    if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey5]))
                    {

                    }
                    break;
                case 5:
                    if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey6]))
                    {

                    }
                    break;
                case 6:
                    if (Input.GetKeyDown(GameManager.data.keyMappings[CustomKeyCode.ShortcutKey7]))
                    {

                    }
                    break;
            }
        }
    }

    public void SkillUIZero()
    {
        img.sprite = null;
        Utils.OnOff(img.gameObject, false);
        skillName = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // 마우스 오른쪽 클릭
        {
            if(skillName != string.Empty)
            {
                SkillData skillToEquip = PlayerManager.instance.player.skill.accquisitionSkillDataList.Find(skill => skill.nameKor == skillName);
                if(skillToEquip != null)
                {
                    skillToEquip.equipPostion = SkillEquipPosition.None;
                    SkillUIZero();
                }
            }
        }
    }
}

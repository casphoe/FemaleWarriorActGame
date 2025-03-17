using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public string skillName;
    int slotIndex = 0;

    Image img;
    private void Awake()
    {
        skillName = string.Empty;
        img = transform.GetChild(0).GetComponent<Image>();
        slotIndex = transform.GetSiblingIndex();
        SkillSlotNum(slotIndex);
    }


    void SkillSlotNum(int index)
    {

    }

    public void SkillUIZero()
    {
        img.sprite = null;
        Utils.OnOff(img.gameObject, false);
        skillName = string.Empty;
    }

    private void Update()
    {
        
    }
}

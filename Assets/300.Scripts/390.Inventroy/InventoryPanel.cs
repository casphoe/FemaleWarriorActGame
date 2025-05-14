using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;

    [SerializeField] Button[] btnSelectPanel;

    public float blinkDuration = 0.5f; //깜빡임 주기

    private Coroutine currentBlinkCoroutine;

    public int selectIndex = -1;

    public int selectItemIndex = 0;
    public int selectWeaponIndex = 0;
    public int selectArmorIndex = 0;
    public int selectAccelyIndex = 0;

    public bool[] isOnce = new bool[4];

    public bool[] isSelect = new bool[4];

    public int selectOrder = 0;

    public static InventoryPanel instance;

    [SerializeField] GameObject[] selectInventory;

    [SerializeField] Button[] btnOrder;

    public Sprite[] PotionSpriteList;

    public Sprite[] weaponSpirteList;

    public Sprite[] aromrSpriteList;

    public Sprite[] accelySpriteList;

    private void Awake()
    {
        Utils.OnOff(panel, false);
        PlayerManager.instance.isInventroy = false;
        instance = this;
        selectOrder = 0;
        selectIndex = 0;
        for(int i = 0; i < isSelect.Length; i++)
        {
            isSelect[i] = false;
        }
        selectItemIndex = 0;
        selectWeaponIndex = 0;
        selectArmorIndex = 0;
        selectAccelyIndex = 0;
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false)
        {
            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Inventory))
            {              
                PlayerManager.instance.isInventroy = !PlayerManager.instance.isInventroy;
                if(PlayerManager.instance.isInventroy == true)
                {
                    Utils.OnOff(panel, true);
                    OnSelectOrderImageChange(selectOrder);
                    OnInventroySelectButton(selectIndex);                 
                }
                else
                {
                    Utils.OnOff(panel, false);
                }
            }

            if (PlayerManager.instance.isInventroy == true)
            {
                if (isSelect.All(select => select == false)) // 또는 !isSelect.Any(select => select)
                {
                    // 왼쪽 화살표 키로 이전 버튼 선택
                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Left))
                    {
                        if (selectIndex > 0)
                        {
                            selectIndex--;
                            OnInventroySelectButton(selectIndex);
                        }
                    }

                    // 오른쪽 화살표 키로 다음 버튼 선택
                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Right))
                    {
                        if (selectIndex < btnSelectPanel.Length - 1)
                        {
                            selectIndex++;
                            OnInventroySelectButton(selectIndex);
                        }
                    }
                }

                // Z 키로 아이템 선택
                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
                {
                    StopAllBinking(btnSelectPanel);
                    isSelect[selectIndex] = true;
                    if (isSelect[0] == true)
                    {
                        if (isOnce[0] == false)
                        {
                            InventoryList.instance.OnItemUiSelect(selectItemIndex);
                            isOnce[0] = true;
                        }                       
                    }
                    else if (isSelect[1] == true)
                    {
                        if (isOnce[1] == false)
                        {
                            isOnce[1] = true;
                        }
                    }
                    else if (isSelect[2] == true)
                    {
                        if (isOnce[2] == false)
                        {
                            isOnce[2] = true;
                        }
                    }
                    else if (isSelect[3] == true)
                    {
                        if (isOnce[3] == false)
                        {
                            isOnce[3] = true;
                        }
                    }                    
                }
                if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Canel))
                {
                    for (int i = 0; i < isSelect.Length; i++)
                        isSelect[i] = false;

                    for (int i = 0; i < isOnce.Length; i++)
                        isOnce[i] = false;

                    OnInventroySelectButton(selectIndex);
                }
            }
        }
    }

    public void OnSelectOrder(int num)
    {
        OnSelectOrderImageChange(num);
        switch (selectIndex)
        {
            case 0:
                InventoryList.instance.OnItemInventroyData();
                break;
            case 1:
                InventoryList.instance.OnWeaponInventoryData();
                break;
            case 2:
                InventoryList.instance.OnArmorInventoryData();
                break;
            case 3:
                InventoryList.instance.OnAccelyInventoryData();
                break;
        }
    }

    void OnSelectOrderImageChange(int num)
    {
        selectOrder = num;
        for (int i = 0; i < btnOrder.Length; i++)
        {
            Utils.ImageColorChange(btnOrder[i].image, Color.white);
            Utils.TextColorChange(btnOrder[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
        }
        Utils.ImageColorChange(btnOrder[num].image, Color.red);
        Utils.TextColorChange(btnOrder[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
    }

    public void OnInventroySelectButton(int num)
    {
        selectIndex = num;
        // 모든 버튼의 깜빡임 중지
        StopAllBinking(btnSelectPanel);
        //선택된 버튼 깜빡임
        StartBlinking(num, btnSelectPanel);

        for (int i = 0; i < btnSelectPanel.Length; i++)
        {
            Utils.OnOff(selectInventory[i], false);
        }
        Utils.OnOff(selectInventory[num], true);

        switch (num)
        {
            case 0:
                InventoryList.instance.OnItemInventroyData();
                break;
            case 1:
                InventoryList.instance.OnWeaponInventoryData();
                break;
            case 2:
                InventoryList.instance.OnArmorInventoryData();
                break;
            case 3:
                InventoryList.instance.OnAccelyInventoryData();
                break;
        }
    }

    private void StartBlinking(int index, Button[] btn)
    {
        if (currentBlinkCoroutine != null)
        {
            StopCoroutine(currentBlinkCoroutine);
        }
        currentBlinkCoroutine = StartCoroutine(Blink(index, btn));
    }

    private IEnumerator Blink(int index, Button[] btn)
    {
        Image buttonImage = btn[index].GetComponent<Image>();
        Color originalColor = buttonImage.color;

        while (true)
        {
            buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            yield return new WaitForSeconds(blinkDuration);

            buttonImage.color = originalColor;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private void StopAllBinking(Button[] btn)
    {
        if (currentBlinkCoroutine != null)
        {
            StopCoroutine(currentBlinkCoroutine);
        }

        for (int i = 0; i < btn.Length; i++)
        {
            ResetButtonAlpha(btn[i]);
        }
    }

    private void ResetButtonAlpha(Button button)
    {
        Image buttonImage = button.GetComponent<Image>();
        Color originalColor = buttonImage.color;
        buttonImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
    }
}

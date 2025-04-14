using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using UnityEngine.UI;

public class ItemInventoryList : MonoBehaviour
{
    public InfiniteScroll itemInvenytoryScrollList;
    public GameObject itemInventoryListContent;

    public int index = 0;
    public int selectIndex = 0;

    private List<InventoryItemPrefabData> dataList = new List<InventoryItemPrefabData>();

    public float blinkDuration = 0.5f; //깜빡임 주기

    private Coroutine currentBlinkCoroutine;

    public Button[] btnClick;

    float itemCount = 0;

    private void Awake()
    {
        itemInvenytoryScrollList.AddSelectCallback((data) =>
        {
            InventoryPanel.instance.selectItemIndex = ((InventoryItemPrefabData)data).index;
            selectIndex = InventoryPanel.instance.selectItemIndex;
        });
    }

    private void Update()
    {
        if (InventoryPanel.instance.isSelect[0] == true)
        {
            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Left))
            {
                if(selectIndex > 0)
                {
                    selectIndex--;
                    OnListSlot(selectIndex);
                }
            }

            if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Right))
            {
                if(selectIndex < btnClick.Length - 1)
                {
                    selectIndex++;
                    OnListSlot(selectIndex);
                }
            }

            if(PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
            {
                itemCount = PlayerManager.instance.player.inventory.GetItemCountByName(InventoryList.instance.itemInventoryList[selectIndex].nameKor);
                if (itemCount > 0)
                {
                    if (InventoryList.instance.itemInventoryList[selectIndex].db == ItemDb.HpPotion)
                    {
                        if (Player.instance.currentHp < PlayerManager.instance.player.hp)
                        {
                            switch (InventoryList.instance.itemInventoryList[selectIndex].nameKor)
                            {
                                case "체력포션":
                                    PlayerManager.instance.player.inventory.RemoveItemByName("체력포션", 1);
                                    break;
                                case "중간체력포션":
                                    PlayerManager.instance.player.inventory.RemoveItemByName("중간체력포션", 1);
                                    break;
                                case "상급체력포션":
                                    PlayerManager.instance.player.inventory.RemoveItemByName("상급체력포션", 1);
                                    break;
                            }
                            Player.instance.currentHp = Player.instance.currentHp + PlayerManager.instance.player.hp * InventoryList.instance.itemInventoryList[selectIndex].hpRestoration;
                            if (Player.instance.currentHp > PlayerManager.instance.player.hp)
                            {
                                Player.instance.currentHp = PlayerManager.instance.player.hp;
                            }
                            Player.instance.lastHpPotionUseTime = Time.time;
                            GameCanvas.instance.PotionSetting();
                            GameCanvas.instance.SliderChange(0, 0, 30);
                        }
                        else
                            return;
                    }
                    else if(InventoryList.instance.itemInventoryList[selectIndex].db == ItemDb.StaminaPotion)
                    {
                        if (Player.instance.currentStamina < PlayerManager.instance.player.stamina)
                        {
                            switch (InventoryList.instance.itemInventoryList[selectIndex].nameKor)
                            {
                                case "스태미나포션":
                                    PlayerManager.instance.player.inventory.RemoveItemByName("스태미나포션", 1);
                                    break;
                                case "중간스태미나포션":
                                    PlayerManager.instance.player.inventory.RemoveItemByName("중간스태미나포션", 1);
                                    break;
                                case "상급스태미나포션":
                                    PlayerManager.instance.player.inventory.RemoveItemByName("상급스태미나포션", 1);
                                    break;
                            }
                            Player.instance.currentStamina = Player.instance.currentStamina + PlayerManager.instance.player.stamina * InventoryList.instance.itemInventoryList[selectIndex].stminaRestoration;
                            if (Player.instance.currentStamina > PlayerManager.instance.player.stamina)
                            {
                                Player.instance.currentStamina = PlayerManager.instance.player.stamina;
                            }
                            Player.instance.lastStaminaPotionUseTime = Time.time;
                            GameCanvas.instance.PotionSetting();
                            GameCanvas.instance.SliderChange(1, 0, 15);
                        }
                        else
                            return;
                    }
                    InventoryList.instance.OnItemInventroyData();

                }
            }            
        }
        else
        {
            StopAllBinking(btnClick);
        }
    }

    void itemListClear()
    {
        dataList.Clear();
        itemInvenytoryScrollList.ClearData();
        index = dataList.Count;
        InfinteScrollReboot();
    }

    void InfinteScrollReboot()
    {
        int count = dataList.Count;
        for (int i = 0; i < count; i++) // 이 부분 확인할 것
        {
            InventoryItemPrefabData data = dataList[i];
            data.index = i;
            data.number = i + 1;
        }
    }

    public void itemInventoryLoadList()
    {
        itemListClear();
        if (dataList.Count != InventoryList.instance.itemInventoryList.Count)
        {
            int difference = Mathf.Abs(dataList.Count - InventoryList.instance.itemInventoryList.Count);
            for (int i = 0; i < difference; i++)
            {
                ItemInventoryInsertData();
            }
        }
        AllUpdate();

        btnClick = new Button[itemInventoryListContent.transform.childCount];
        for (int i = 0; i < btnClick.Length; i++)
        {
            btnClick[i] = itemInventoryListContent.transform.GetChild(i).GetComponent<Button>();
        }
    }

    void ItemInventoryInsertData()
    {
        InventoryItemPrefabData data = new InventoryItemPrefabData();
        data.index = index++;
        data.number = itemInvenytoryScrollList.GetItemCount() + 1;
        dataList.Add(data);
        itemInvenytoryScrollList.InsertData(data);
    }

    private void OnEnable()
    {
        AllUpdate();
    }

    void AllUpdate()
    {
        itemInvenytoryScrollList.UpdateAllData();
    }

    public void OnListSlot(int num)
    {
        if(num == -1)
        {
            return;
        }
        InventoryPanel.instance.selectItemIndex = num;

        StopAllBinking(btnClick);

        StartBlinking(num, btnClick);
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

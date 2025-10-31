using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public int selectQuestNum = 0;

    public List<QuestData> acceptQuest = new List<QuestData>();

    public List<QuestData> ongoingQuest = new List<QuestData>();

    public List<QuestData> completeQuest = new List<QuestData>();

    public List<QuestPrefabData> questPrefabList = new List<QuestPrefabData>();

    public QuestDataReader dataReader;

    [SerializeField] GameObject qusetPanel;

    [SerializeField] GameObject questUiPanel;

    [SerializeField] GameObject uiSetObject;

    [SerializeField] Button[] btnQuest;

    [SerializeField] Button[] btnQuestSetting;

    [SerializeField] Button[] btnQuestPanel;

    [SerializeField] Text txtQuestTitle;

    [SerializeField] GameObject[] questObject;

    [SerializeField] QuestAcceptList accpetDataList;

    [SerializeField] QuestOngoingList currentDataList;

    [SerializeField] QuestFinsishList finishDataList;

    [SerializeField] QuestRemoveList removeDataList;

    [SerializeField] Text[] txtQuestUiSet;

    bool[] isQuestPanelSelect = new bool[4];
    bool[] isQuestSelect;

    Coroutine currentBlinkOneCoroutine;
    Coroutine currentBlinkTwoCoroutine;

    float blinkDuration = 0.5f; //깜빡임 주기

    int questNum = 0;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Utils.OnOff(qusetPanel, false);
        questNum = 0;
        selectQuestNum = 0;
        btnQuest[0].onClick.AddListener(() => OnQuestBtnClickEvent(0));
        btnQuest[1].onClick.AddListener(() => OnQuestBtnClickEvent(1));
        btnQuest[2].onClick.AddListener(() => OnQuestBtnClickEvent(2));
        btnQuest[3].onClick.AddListener(() => OnQuestBtnClickEvent(3));
        btnQuest[4].onClick.AddListener(() => OnQuestBtnClickEvent(4));
        Utils.OnOff(uiSetObject, false);
        for(int i = 0; i < isQuestPanelSelect.Length; i++)
        {
            isQuestPanelSelect[i] = false;
        }

        btnQuestPanel[0].onClick.AddListener(() => OnQuestPanelBtnClickEvent(0));
        btnQuestPanel[1].onClick.AddListener(() => OnQuestPanelBtnClickEvent(1));
        btnQuestPanel[2].onClick.AddListener(() => OnQuestPanelBtnClickEvent(2));
        btnQuestPanel[3].onClick.AddListener(() => OnQuestPanelBtnClickEvent(3));

        ongoingQuest = GetOngoingQuests(PlayerManager.instance.player.questList);
        completeQuest = GetCompletedQuests(PlayerManager.instance.player.questList);
    }

    private void Update()
    {
        if(PlayerManager.instance.IsDead == false)
        {
            if(PlayerManager.instance.isQuest == true)
            {
                if(isQuestPanelSelect.All(select => select == false))
                {
                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Left))
                    {
                        if(questNum > 0)
                        {
                            questNum -= 1;
                            OnQuestImageChange(questNum);
                        }
                    }

                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Right))
                    {
                        if(questNum < btnQuestPanel.Length - 1)
                        {
                            questNum += 1;
                            OnQuestImageChange(questNum);
                        }
                    }

                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
                    {
                        StopAllBinking(btnQuestPanel);
                        OnQuestPanelBtnClickEvent(questNum);                    
                    }
                }
                else
                {
                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Up))
                    {
                        if (questNum != 1)
                        {
                            if (selectQuestNum > 0)
                            {
                                selectQuestNum -= 1;
                                OnQuestPanelImageChange(selectQuestNum);
                            }
                        }
                    }

                    if (PlayerManager.GetCustomKeyDown(CustomKeyCode.Down))
                    {
                        switch (questNum)
                        {
                            case 0:
                                if (selectQuestNum < acceptQuest.Count - 1)
                                {
                                    selectQuestNum += 1;
                                }
                                break;
                            case 2:
                                if (selectQuestNum < completeQuest.Count - 1)
                                {
                                    selectQuestNum += 1;
                                }
                                break;
                            case 3:
                                if (selectQuestNum < ongoingQuest.Count - 1)
                                {
                                    selectQuestNum += 1;
                                }
                                break;
                        }
                        if (questNum != 1)
                        {
                            OnQuestPanelImageChange(selectQuestNum);
                        }
                    }

                    if(PlayerManager.GetCustomKeyDown(CustomKeyCode.Attack))
                    {
                        if (questNum != 1)
                        {
                            if(questNum == 2)
                            {
                                if (completeQuest[selectQuestNum].isComplete == true)
                                    return;
                                
                            }
                            isQuestSelect[selectQuestNum] = true;
                            OnOffUiSetObject(true);
                            OnUiSetTextSetting(questNum);
                        }
                    }
                }

                if(PlayerManager.GetCustomKeyDown(CustomKeyCode.Canel))
                {
                    if (isQuestPanelSelect.Any(select => select == true) && (isQuestSelect == null || isQuestSelect.All(select => select == false))) //isQuestPanelSelect 배열중 하나라도 true 이면 true를 반환
                    {
                        for (int i = 0; i < isQuestPanelSelect.Length; i++)
                        {
                            isQuestPanelSelect[i] = false;
                            Utils.OnOff(questObject[i], false);
                        }

                        for (int i = 0; i < btnQuestPanel.Length; i++)
                        {
                            Utils.ImageColorChange(btnQuestPanel[i].image, Color.white);
                            Utils.TextColorChange(btnQuestPanel[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
                        }
                        txtQuestTitle.text = "";
                        OnQuestImageChange(questNum);
                    }

                    if (questNum != 1 && isQuestSelect != null && isQuestSelect.Any(select => select == true)) //isQuestSelect 배열중 하나라도 true 이면 true를 반환
                    {
                        if(questNum == 2)
                        {
                            if (completeQuest[selectQuestNum].isComplete == true)
                            {
                                StopAllBinking(btnQuestSetting);
                                return;
                            }
                            else
                            {
                                for (int i = 0; i < isQuestSelect.Length; i++)
                                {
                                    isQuestSelect[i] = false;
                                }
                                OnQuestPanelImageChange(selectQuestNum);
                                OnOffUiSetObject(false);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < isQuestSelect.Length; i++)
                            {
                                isQuestSelect[i] = false;
                            }
                            OnQuestPanelImageChange(selectQuestNum);
                            OnOffUiSetObject(false);
                        }
                    }
                }
            }
        }
    }

    #region 수락, 제거, 완료
    public void AcceptQuest(PlayerData playerData, QuestData originalQuest)
    {
        // 이미 같은 퀘스트를 수락 중인지 확인
        var existingQuest = playerData.questList.Find(q => q.questId == originalQuest.questId);

        // 반복 퀘스트가 아니고, 이미 완료했다면 다시 수락 불가
        if (existingQuest != null && !originalQuest.isRepeat)
        {
            Debug.Log($"퀘스트 {originalQuest.questId}는 이미 수락되었거나 완료되어 재수락 불가.");
            return;
        }

        // 반복 퀘스트인 경우: 현재 수락 중이면 다시 수락 불가
        if (originalQuest.isRepeat && existingQuest != null && !existingQuest.isComplete)
        {
            Debug.Log($"반복 퀘스트 {originalQuest.questId}는 이미 수락 중입니다.");
            return;
        }

        QuestData newQuest = new QuestData(
        originalQuest.questId,
        originalQuest.titleKor,
        originalQuest.titleEng,
        originalQuest.descriptionKor,
        originalQuest.descriptionEng,
        originalQuest.rewardExp,
        originalQuest.rewardMoney,
        originalQuest.requiredAmount,
        0,                      // currentAmount는 새 퀘스트이므로 0부터 시작
        true,                  // 수락 상태
        false,                 // 완료 버튼 누르기 전
        false,                  // 클리어 전
        originalQuest.isRepeat //반복 퀘스트
    );
        playerData.questList.Add(newQuest);
        ongoingQuest.Add(newQuest);
        Debug.Log($"퀘스트 {newQuest.questId} 수락됨 (새로 생성)");
    }

    public void RemoveQuest(PlayerData playerData, int questId)
    {
        QuestData quest = playerData.questList.Find(q => q.questId == questId);
        if (quest != null)
        {
            playerData.questList.Remove(quest);
            Debug.Log($"퀘스트 {questId} 삭제됨");
        }

        QuestData ongoing = ongoingQuest.Find(q => q.questId == questId);
        {
            if (ongoing != null)
            {
                ongoingQuest.Remove(ongoing);
            }
        }
    }

    public void CompleteQuest(PlayerData playerData, int questId)
    {
        QuestData quest = ongoingQuest.Find(q => q.questId == questId && q.isCleared && !q.isComplete);
        if (quest != null)
        {
            // 완료 조건 검사: currentAmount >= requiredAmount
            if (quest.currentAmount >= quest.requiredAmount)
            {
                quest.isCleared = true;
                quest.isComplete = true;
               
                PlayerManager.instance.AddExp(quest.rewardExp);
                PlayerManager.instance.AddMoney(quest.rewardMoney);

                Debug.Log($"퀘스트 {quest.questId} 클리어됨! 보상: EXP {quest.rewardExp}, Money {quest.rewardMoney}");

                // 반복 퀘스트면 완료 후 제거 (재수락 가능)
                if (quest.isRepeat)
                {
                    //반복 퀘스트는 완료 후 삭제
                    completeQuest.Remove(quest);

                    playerData.questList.Remove(quest);
                    Debug.Log($"반복 퀘스트 {quest.questId} 완료 후 제거되어 재수락 가능함.");
                }
                else
                {
                    if (completeQuest.Contains(quest))
                    {
                        quest.isComplete = true;
                    }

                    int index = playerData.questList.FindIndex(q => q.questId == quest.questId);
                    if(index != -1)
                    {
                        playerData.questList[index].isCleared = true;
                        playerData.questList[index].isComplete = true;
                    }
                }

                // 진행 중 퀘스트에서 제거
                ongoingQuest.Remove(quest);
            }
        }
    }
    #endregion

    #region 퀘스트들 가져오기
    List<QuestData> GetAcceptableQuests(List<QuestData> allQuests)
    {
        return allQuests.Where(q =>
            !q.isAccepted &&                             // 수락되지 않았고
            (!q.isComplete || q.isRepeat)           // 완료됐더라도 반복 퀘스트면 OK
        ).ToList();
    }

    public List<QuestData> GetOngoingQuests(List<QuestData> questList)
    {
        return questList.Where(q =>
            q.isAccepted &&               // 수락된 상태이며
            !q.isComplete                 // 아직 완료되지 않은 퀘스트
        ).ToList();
    }

    List<QuestData> GetCompletedQuests(List<QuestData> questList)
    {
        return questList.Where(q =>
            q.isAccepted &&    // 수락했으며
            q.isComplete &&    // 완료되었고
            !q.isRepeat        // 반복 퀘스트는 제외
        ).ToList();
    }
    #endregion

    #region 적이 죽었을 때 퀘스트 값 변화
    public void EnemyDeathQuestChange(int num)
    {     
        if(num == 0)
        {
            foreach (QuestData quest in ongoingQuest)
            {
                if (quest.questId == 0 || quest.questId == 1)
                {
                    quest.currentAmount += 1;
                }
            }
        }
        else if(num == 6)
        {
            foreach (QuestData quest in ongoingQuest)
            {
                if (quest.questId == 1 || quest.questId == 2)
                {
                    quest.currentAmount += 1;
                }
            }
        }
        else
        {
            foreach (QuestData quest in ongoingQuest)
            {
                if (quest.questId == 0)
                {
                    quest.currentAmount += 1;
                }
            }
        }
    }
    #endregion

    #region UI
    public void OnOffQuestUI(bool isActive)
    {
        Utils.OnOff(qusetPanel, isActive);
    }

    public void OnOffUiSetObject(bool isActive)
    {
        Utils.OnOff(uiSetObject, isActive);
    }

    void OnQuestBtnClickEvent(int num)
    {
        questNum = num;
        if(num != 4)
        {
            OnOffQuestUI(false);
            Utils.OnOff(questUiPanel, true);
            OnQuestPanelBtnClickEvent(questNum);
        }
        switch (questNum)
        {
            case 4: //취소
                PlayerManager.instance.isQuest = false;
                OnOffQuestUI(false);
                break;
        }
    }

    void OnQuestPanelBtnClickEvent(int num)
    {
        if (questNum != num)
        {
            questNum = num;
        }
        for (int i = 0; i < isQuestPanelSelect.Length; i++)
        {
            isQuestPanelSelect[i] = false;
            Utils.OnOff(questObject[i], false);
        }
        Utils.OnOff(questObject[num], true);
        isQuestPanelSelect[num] = true;
        for (int i = 0; i < btnQuestPanel.Length; i++)
        {
            Utils.ImageColorChange(btnQuestPanel[i].image, Color.white);
            Utils.TextColorChange(btnQuestPanel[i].transform.GetChild(0).GetComponent<Text>(), Color.black);
        }
        Utils.ImageColorChange(btnQuestPanel[num].image, Color.red);
        Utils.TextColorChange(btnQuestPanel[num].transform.GetChild(0).GetComponent<Text>(), Color.white);
        selectQuestNum = 0;
        OnQuestTitleTextSetting(num);
        questPrefabList.Clear();
        switch (num)
        {
            case 0:
                if(PlayerManager.instance.player.questList.Count == 0)
                {
                    acceptQuest = GetAcceptableQuests(dataReader.QuestDataList);
                }
                else
                {
                    //게임에 존재하는 모든 퀘스트 데이터를 가져옵니다.
                    List<QuestData> allQuests = dataReader.QuestDataList;
                    //현재 플레이어가 수락한 퀘스트 리스트를 가져옵니다
                    List<QuestData> acceptedQuests = PlayerManager.instance.player.questList;
                    //플레이어가 아직 수락하지 않은 퀘스트만 필터링
                    //acceptedQuests 와 같은 questId 있으면 제외 하고 가져온다는 의미
                    List<QuestData> notAcceptedQuests = allQuests
                        .Where(q => !acceptedQuests.Any(aq => aq.questId == q.questId))
                        .ToList();

                    //acceptQuest 에 실제로 수락 가능한 퀘스트만 걸러내는 함수를 넘겨서 처리
                    acceptQuest = GetAcceptableQuests(notAcceptedQuests);
                }
                btnQuestSetting = new Button[acceptQuest.Count];
                isQuestSelect = new bool[acceptQuest.Count];
                for(int i = 0; i < isQuestSelect.Length; i++)
                {
                    isQuestSelect[i] = false;
                }
                break;
            case 1:
                if(btnQuestSetting.Length != 0)
                {
                    StopAllBinking(btnQuestSetting);
                }             
                //배열 초기화 현재 진행중인 퀘스트는 클릭 되는것이 없고 어디까지 진행 중인 상태만 확인하기 위해서 보는 용도
                btnQuestSetting = null;
                isQuestSelect = null;
                break;
            case 2:
                foreach (var quest in ongoingQuest)
                {
                    quest.CheckClearStatus();
                    if(quest.isCleared)
                    {
                        if(!completeQuest.Contains(quest))
                        {
                            completeQuest.Add(quest);
                        }                    
                    }
                }
                btnQuestSetting = new Button[completeQuest.Count];
                isQuestSelect = new bool[completeQuest.Count];
                for (int i = 0; i < isQuestSelect.Length; i++)
                {
                    isQuestSelect[i] = false;
                }
                break;
            case 3:
                btnQuestSetting = new Button[ongoingQuest.Count];
                isQuestSelect = new bool[ongoingQuest.Count];
                for (int i = 0; i < isQuestSelect.Length; i++)
                {
                    isQuestSelect[i] = false;
                }
                break;
        }
        StartCoroutine(QuestLoadList(num));
    }

    IEnumerator QuestLoadList(int num)
    {
        yield return null;
        switch(num)
        {
            case 0:
                accpetDataList.AcceptLoadList();               
                break;
            case 1:
                currentDataList.CurrentQuestLoadList();
                break;
            case 2:
                finishDataList.CompeteLoadList();               
                break;
            case 3:
                removeDataList.RemoveLoadList();                        
                break;
        }

        if(num != 1)
        {
            for (int i = 0; i < questPrefabList.Count; i++)
            {
                btnQuestSetting[i] = questPrefabList[i].prefab.GetComponent<Button>();
            }

            if (btnQuestSetting.Length > 0)
            {
                if(num == 2)
                {
                    if (completeQuest[selectQuestNum].isComplete == true)
                    {
                        StopAllBinking(btnQuestSetting);
                    }
                    else
                    {
                        OnQuestPanelImageChange(selectQuestNum);
                    }
                }
                else
                {
                    OnQuestPanelImageChange(selectQuestNum);
                }
            }
        }
    }

    void OnQuestTitleTextSetting(int num)
    {
        switch(num)
        {
            case 0:
                switch(GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txtQuestTitle.text = "수락 가능한 퀘스트를 선택해주세요";
                        break;
                    case LANGUAGE.ENG:
                        txtQuestTitle.text = "Please select an acceptable quest";
                        break;
                }
                break;
            case 1:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txtQuestTitle.text = "현재 진행중인 퀘스트를 확인해주세요";
                        break;
                    case LANGUAGE.ENG:
                        txtQuestTitle.text = "Please check the quests currently in progress";
                        break;
                }
                break;
            case 2:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txtQuestTitle.text = "완료 가능한 퀘스틀 선택후 완료 해주세요";
                        break;
                    case LANGUAGE.ENG:
                        txtQuestTitle.text = "Please select a quest that can be completed and complete it";
                        break;
                }
                break;
            case 3:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txtQuestTitle.text = "수락중인 퀘스트 중에 필요 없는 퀘스트를 제거해주세요";
                        break;
                    case LANGUAGE.ENG:
                        txtQuestTitle.text = "Please remove any unnecessary quests from the quests you are accepting";
                        break;
                }
                break;
        }
    }

    void OnQuestImageChange(int num)
    {
        StopAllBinking(btnQuestPanel);

        StartBlinking(num, btnQuestPanel, 0);
    }

    void OnQuestPanelImageChange(int num)
    {
        StopAllBinking(btnQuestSetting);

        StartBlinking(num, btnQuestSetting, 1);
    }


    private void StartBlinking(int index, Button[] btn, int num)
    {
        switch(num)
        {
            case 0:
                if (currentBlinkOneCoroutine != null)
                {
                    StopCoroutine(currentBlinkOneCoroutine);
                }
                currentBlinkOneCoroutine = StartCoroutine(Blink(index, btn));
                break;
            case 1:
                if (currentBlinkTwoCoroutine != null)
                {
                    StopCoroutine(currentBlinkTwoCoroutine);
                }
                currentBlinkTwoCoroutine = StartCoroutine(Blink(index, btn));
                break;
        }       
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
        if (currentBlinkOneCoroutine != null)
        {
            StopCoroutine(currentBlinkOneCoroutine);
        }

        if (currentBlinkTwoCoroutine != null)
        {
            StopCoroutine(currentBlinkTwoCoroutine);
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

    public void QuestUIClose()
    {
        Utils.OnOff(questUiPanel, false);
        PlayerManager.instance.isQuest = false;
    }

    public void OnUiSetTextSetting(int num)
    {
        switch(num)
        {
            case 0:
                switch(GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:                       
                        txtQuestUiSet[0].text = acceptQuest[selectQuestNum].titleKor;
                        txtQuestUiSet[1].text = acceptQuest[selectQuestNum].descriptionKor + "\n퀘스트를 수락하시겠습니까?";
                        break;
                    case LANGUAGE.ENG:                     
                        txtQuestUiSet[0].text = acceptQuest[selectQuestNum].titleEng;
                        txtQuestUiSet[1].text = acceptQuest[selectQuestNum].descriptionKor + "\nWould you like to accept the quest?";
                        break;
                }
                break; 
            case 2:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txtQuestUiSet[0].text = completeQuest[selectQuestNum].titleKor;
                        txtQuestUiSet[1].text = completeQuest[selectQuestNum].descriptionKor + "\n퀘스트를 완료하시겠습니까?";
                        break;
                    case LANGUAGE.ENG:
                        txtQuestUiSet[0].text = completeQuest[selectQuestNum].titleEng;
                        txtQuestUiSet[1].text = completeQuest[selectQuestNum].descriptionKor + "\nWould you like to complete the quest?";
                        break;
                }
                break;
            case 3:
                switch (GameManager.data.lanauge)
                {
                    case LANGUAGE.KOR:
                        txtQuestUiSet[0].text = ongoingQuest[selectQuestNum].titleKor;
                        txtQuestUiSet[1].text = ongoingQuest[selectQuestNum].descriptionKor + "\n수락한 퀘스트를 제거하시겠습니까?\n현재 진행중인 퀘스트 데이터가 초기화 됩니다.";
                        break;
                    case LANGUAGE.ENG:
                        txtQuestUiSet[0].text = ongoingQuest[selectQuestNum].titleEng;
                        txtQuestUiSet[1].text = ongoingQuest[selectQuestNum].descriptionKor + "\nAre you sure you want to remove the accepted quest?\n\r\nThe quest data currently in progress will be reset.";
                        break;
                }
                break;
        }
    }

    public void OnUiSetBtnClickEvent(int num)
    {
        Debug.Log(questNum);
        switch(questNum)
        {
            case 0:
                switch(num)
                {
                    case 0: //퀘스트 수락
                        OnOffUiSetObject(false);
                        AcceptQuest(PlayerManager.instance.player, acceptQuest[selectQuestNum]);
                        OnQuestPanelBtnClickEvent(0);
                        break;
                    case 1:
                        OnOffUiSetObject(false);
                        break;
                }
                break;
            case 2:
                switch (num)
                {
                    //퀘스트 완료
                    case 0:
                        OnOffUiSetObject(false);
                        CompleteQuest(PlayerManager.instance.player, completeQuest[selectQuestNum].questId);
                        OnQuestPanelBtnClickEvent(2);
                        break;
                    case 1:
                        OnOffUiSetObject(false);
                        break;
                }
                break;
            case 3:
                switch (num)
                {
                    //퀘스트 제거
                    case 0:
                        OnOffUiSetObject(false);
                        RemoveQuest(PlayerManager.instance.player, ongoingQuest[selectQuestNum].questId);
                        OnQuestPanelBtnClickEvent(3);
                        break;
                    case 1:
                        OnOffUiSetObject(false);
                        break;
                }
                break;
        }
    }
    #endregion
}
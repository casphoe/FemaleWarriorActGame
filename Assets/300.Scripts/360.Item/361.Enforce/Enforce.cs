using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]

public class Enforce
{
    // 강화 확률 설정(방어구,무기)
    public Dictionary<int, int> enforceRates = new Dictionary<int, int>
    {
        { 0, 100 }, // 0강 → 1강
        { 1, 100 }, // 1강 → 2강
        { 2, 50 },  // 2강 → 3강
        { 3, 50 },  // 3강 → 4강
        { 4, 25 },  // 4강 → 5강
        { 5, 25 },  // 5강 → 6강
        { 6, 10 },  // 6강 → 7강
        { 7, 10 },  // 7강 → 8강
        { 8, 5 },   // 8강 → 9강
    };

    //강화 확률 설정 (액세서리)
    public Dictionary<int, int> accelyEnforceRates = new Dictionary<int, int>
    {
        { 0, 30 }, // 0강 → 1강
        { 1, 15 }, // 1강 → 2강
        { 2, 5 },  // 2강 → 3강
    };

    public int TryReinforce(ItemData item)
    {     
        int currentEnforce = item.itemEnforce;

        int successRate = 0;

        // 강화 단계 초과 확인
        if (currentEnforce >= item.itemMaxEnforce)
        {
            Debug.Log("최대 강화 단계에 도달했습니다.");
            return 2;
        }
        if(item.db == ItemDb.Weapon || item.db == ItemDb.Armor)
        {
            // 강화 확률 가져오기
            if (!enforceRates.TryGetValue(currentEnforce, out successRate))
            {
                Debug.LogError("강화 확률 데이터를 찾을 수 없습니다.");
                return 3;
            }
        }
        else
        {
            if (!accelyEnforceRates.TryGetValue(currentEnforce, out successRate))
            {
                Debug.LogError("강화 확률 데이터를 찾을 수 없습니다.");
                return 3;
            }
        }

        int cost = item.GetReinforcementCost();
        if(PlayerManager.instance.player.money < cost)
        {
            return cost;
        }

        //강화 비용 차감
        PlayerManager.instance.player.money -= cost;
        // 강화 성공 여부 결정
        int randomChance = UnityEngine.Random.Range(0, 100);
        if (randomChance < successRate)
        {
            int newEnforce = Mathf.Clamp(currentEnforce + 1, 0, item.itemMaxEnforce);        
            //동일한 강화아이템이 있는지 확인하기 위한 함수
            ItemData exisitingItem = PlayerManager.instance.player.inventory.FindItemByNameAndEnforceLevel(item.nameKor + "(" + newEnforce + ")", newEnforce);        
            if (exisitingItem != null)
            {
                // 동일한 강화 등급 아이템이 존재하면 haveCount 증가
                exisitingItem.haveCount += 1;
                exisitingItem.unequippedCount += 1;
            }
            else
            {
                ItemData newItem = (ItemData)item.Clone(); // 아이템 복사
                newItem.itemEnforce = newEnforce; // 강화된 단계 적용
                newItem.haveCount = 1;
                newItem.unequippedCount = 1;

                // 강화된 아이템의 이름 변경
                newItem.nameKor = GetEnhancedName(newItem.nameKor, newEnforce);
                newItem.name = GetEnhancedName(newItem.name, newEnforce);
                //강화후 스텟 업그레이드
                UpdateEnforce(newItem, newItem.itemEnforce);
                //현재날짜를 원하는 형식으로 변경 (T가 포함되지 않도록)               
                string formattedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                PlayerManager.instance.player.inventory.AddItemToInventory(PlayerManager.instance.player.inventory.inventoryDataList.Count + 1, formattedDateTime, newItem); //강화된 아이템을 인벤토리에 새롭게 추가
            }

            PlayerManager.instance.player.inventory.RemoveItemByName(item.nameKor, 1); //강화성공 되면 강화 되기전 아이템을 인벤토리에서 삭제
            EnforceList.instance.AddToEnforceList();
            Debug.Log($"강화 성공! 현재 강화 단계: {item.itemEnforce}");
            return 0;
        }
        else
        {
            Debug.Log("강화 실패!");
            return 1;
        }
    }

    // 강화된 이름을 설정하는 함수
    private string GetEnhancedName(string originalName, int enforceLevel)
    {
        // 이름에 괄호가 이미 있는지 확인
        int startIndex = originalName.LastIndexOf('(');
        int endIndex = originalName.LastIndexOf(')');

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            // 괄호 안의 숫자만 변경
            string nameWithoutEnforce = originalName.Substring(0, startIndex); // 괄호 앞 부분
            return nameWithoutEnforce + "(" + enforceLevel + ")";
        }
        else
        {
            // 괄호가 없으면 새로운 괄호를 추가
            return originalName + "(" + enforceLevel + ")";
        }
    }

    public void UpdateEnforce(ItemData item,int newEnforce)
    {
        item.itemEnforce = Mathf.Clamp(newEnforce, 0, item.itemMaxEnforce);
        ApplyReinforcementStats(item);
        Debug.Log($"아이템 강화 단계가 {item.itemEnforce}로 변경되었습니다.");
    }

    // 강화 단계에 따른 스탯 증가 계산
    private void ApplyReinforcementStats(ItemData item)
    {

        float reinforcementMultiplier = 1 + (0.1f * item.itemEnforce); // 강화 단계당 10% 증가(공격력 방어력,크리티컬데미지,크리티컬 확률은 다 10%이고) 운은 1씩 증가

        // 기본 스탯에 강화 배율 적용 후 반올림 (소수점 첫째짜리만 표시 둘째 자리부터 반올림)
        item.HpUp = MathF.Round(item.HpUp * reinforcementMultiplier * 10) / 10;
        item.StaminaUp = MathF.Round(item.StaminaUp * reinforcementMultiplier * 10) / 10;
        item.attackUp = Mathf.Round(item.attackUp * reinforcementMultiplier * 10) / 10f;
        item.defenceUp = Mathf.Round(item.defenceUp * reinforcementMultiplier * 10) / 10f;
        item.crictleDmgUp = Mathf.Round(item.crictleDmgUp * reinforcementMultiplier * 10) / 10f;
        item.crictleRateUp = Mathf.Round(item.crictleRateUp * reinforcementMultiplier * 10) / 10f;

        // 운이 양수일 경우 1씩 증가, 음수일 경우 -1씩 증가
        if (item.lukUp > 0)
        {
            // 운이 양수일 때는 강화 단계만큼 1씩 증가
            item.lukUp += 1;
        }
        else if(item.lukUp < 0)
        {
            // 운이 음수일 때는 강화 단계만큼 -1씩 감소
            item.lukUp -= 1;
        }
        item.HpUp = Mathf.Round(item.HpUp * reinforcementMultiplier * 10) / 10f;
        item.StaminaUp = Mathf.Round(item.StaminaUp * reinforcementMultiplier * 10) / 10f;       
    }
}

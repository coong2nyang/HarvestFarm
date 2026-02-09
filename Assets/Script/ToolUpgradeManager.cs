using UnityEngine;
using System.Collections.Generic;

public class ToolUpgradeManager : MonoBehaviour
{
    // 도구 등급 정의
    public enum ToolGrade { Stone, Copper, Iron, Gold }

    [System.Serializable]
    public class ToolData
    {
        public string toolName;
        public ToolGrade grade;
        public float efficiency; // 등급이 높을수록 작업 속도 향상
    }

    // 1. 용광로 시스템: 원광석을 주괴로 변환
    public void SmeltOre(string oreName)
    {
        string barName = oreName + "Bar"; // 예: Copper + Bar = CopperBar

        // 재료 확인: 광석 5개와 석탄 1개가 필요하다고 가정
        if (InventoryManager.Instance.HasItem(oreName, 5) && InventoryManager.Instance.HasItem("Coal", 1))
        {
            InventoryManager.Instance.RemoveItem(oreName, 5);
            InventoryManager.Instance.RemoveItem("Coal", 1);
            InventoryManager.Instance.AddItem(barName, 1);
            Debug.Log($"{barName} 제작 완료!");
        }
        else
        {
            Debug.Log("재료(광석 5개 또는 석탄 1개)가 부족합니다.");
        }
    }

    // 2. 도구 강화 시스템 (돌 -> 구리 -> 철 -> 금)
    public void UpgradeTool(string toolType)
    {
        // 현재 인벤토리에서 해당 도구의 등급을 확인
        string currentToolName = InventoryManager.Instance.GetOwnedToolName(toolType);
        ToolGrade currentGrade = GetGradeFromID(currentToolName);

        if (currentGrade == ToolGrade.Gold)
        {
            Debug.Log("이미 최고 등급입니다.");
            return;
        }

        ToolGrade nextGrade = currentGrade + 1;
        string ingredientBar = nextGrade.ToString() + "Bar";

        // 강화 재료 확인 (주괴 5개 필요)
        if (InventoryManager.Instance.HasItem(ingredientBar, 5))
        {
            // 기존 도구 제거 및 주괴 소모
            InventoryManager.Instance.RemoveItem(currentToolName, 1);
            InventoryManager.Instance.RemoveItem(ingredientBar, 5);

            // 상위 등급 도구 추가
            string newToolName = nextGrade.ToString() + toolType;
            InventoryManager.Instance.AddItem(newToolName, 1);

            Debug.Log($"{newToolName}으로 강화 성공!");
        }
        else
        {
            Debug.Log($"{ingredientBar} 5개가 필요합니다.");
        }
    }

    // 아이템 이름에서 등급을 추출하는 보조 함수
    ToolGrade GetGradeFromID(string toolName)
    {
        if (toolName.Contains("Copper")) return ToolGrade.Copper;
        if (toolName.Contains("Iron")) return ToolGrade.Iron;
        if (toolName.Contains("Gold")) return ToolGrade.Gold;
        return ToolGrade.Stone;
    }
}
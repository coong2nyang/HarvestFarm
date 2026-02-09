using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 에러의 원인이었던 Ingredient 클래스를 정의합니다.
[System.Serializable]
public class Ingredient
{
    public string itemName; // 재료 이름 (예: "Stone")
    public int amount;      // 필요한 개수 (예: 5)
}

[System.Serializable]
public class Recipe
{
    public string resultItem;
    public List<Ingredient> ingredients;
}

public class CraftingManager : MonoBehaviour
{
    // 2. 돌 도구 제작: 돌 5 + 나무 10
    public void CraftStoneTool()
    {
        // 재료가 있는지 확인하고 있으면 소모 후 아이템 추가
        if (CheckIngredients("Stone", 5) && CheckIngredients("Wood", 10))
        {
            RemoveIngredients("Stone", 5);
            RemoveIngredients("Wood", 10);

            // 인벤토리 매니저에 아이템 추가 (싱글톤 패턴 가정)
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.AddItem("StoneTool", 1);
                Debug.Log("돌 도구 제작 완료!");
            }
        }
        else
        {
            Debug.Log("재료가 부족합니다.");
        }
    }

    // 3. 재료 확인 로직 (InventoryManager와 연동)
    private bool CheckIngredients(string name, int amount)
    {
        // 인벤토리 매니저에서 해당 아이템의 개수를 파악하는 함수를 호출해야 합니다.
        // 현재는 예시로 true를 반환하게 되어있으므로, 실제 인벤토리 시스템과 연결하세요.
        if (InventoryManager.Instance != null)
        {
            return InventoryManager.Instance.HasItem(name, amount);
        }
        return false;
    }

    // 4. 재료 소모 로직
    private void RemoveIngredients(string name, int amount)
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(name, amount);
        }
    }
}
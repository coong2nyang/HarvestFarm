using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    // 싱글톤 패턴: 어디서든 접근 가능하게 설정
    public static InventoryManager Instance;

    [System.Serializable]
    public class ItemSlot
    {
        public string itemName;
        public int count;
        public Sprite icon;

        public ItemSlot() { itemName = ""; count = 0; icon = null; }
    }

    [Header("Inventory Settings")]
    public List<ItemSlot> slots = new List<ItemSlot>(); // 50칸의 슬롯
    public int maxSlotCount = 50;
    public int maxStackSize = 100; // 최대 100개까지 겹침

    [Header("UI References")]
    public Transform inventoryGrid; // 메인 인벤토리 그리드 (5x10)
    public Transform quickSlotGrid; // 하단 퀵슬롯 그리드 (1x10)
    public GameObject slotPrefab;   // 슬롯 프리팹

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 1. 50개의 빈 슬롯 초기화
        for (int i = 0; i < maxSlotCount; i++)
        {
            slots.Add(new ItemSlot());
        }

        RefreshUI();
    }

    // 2. 아이템 추가 로직 (중복 겹치기 포함)
    public void AddItem(string name, int amount, Sprite icon = null)
    {
        // 먼저 이미 있는 아이템인지 확인 (겹치기)
        foreach (var slot in slots)
        {
            if (slot.itemName == name && slot.count < maxStackSize)
            {
                int canAdd = maxStackSize - slot.count;
                int toAdd = Mathf.Min(canAdd, amount);
                slot.count += toAdd;
                amount -= toAdd;

                if (amount <= 0) { RefreshUI(); return; }
            }
        }

        // 남은 양이 있다면 빈 슬롯에 추가
        while (amount > 0)
        {
            ItemSlot emptySlot = slots.Find(s => s.itemName == "");
            if (emptySlot != null)
            {
                emptySlot.itemName = name;
                emptySlot.icon = icon;
                int toAdd = Mathf.Min(maxStackSize, amount);
                emptySlot.count = toAdd;
                amount -= toAdd;
            }
            else
            {
                Debug.Log("인벤토리가 가득 찼습니다!");
                break;
            }
        }
        RefreshUI();
    }

    // 3. 아이템 존재 확인 (제작/강화용)
    public bool HasItem(string name, int amount)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.itemName == name) total += slot.count;
        }
        return total >= amount;
    }

    // 4. 아이템 제거 로직
    public void RemoveItem(string name, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemName == name)
            {
                if (slots[i].count > amount)
                {
                    slots[i].count -= amount;
                    amount = 0;
                }
                else
                {
                    amount -= slots[i].count;
                    slots[i].itemName = "";
                    slots[i].count = 0;
                    slots[i].icon = null;
                }
            }
            if (amount <= 0) break;
        }
        RefreshUI();
    }

    // 5. 도구 이름 가져오기 (강화용)
    public string GetOwnedToolName(string toolType)
    {
        var tool = slots.Find(s => s.itemName.Contains(toolType));
        return tool != null ? tool.itemName : "Stone" + toolType;
    }

    // 6. UI 갱신 (메인 인벤토리 + 퀵슬롯 연동)
    void RefreshUI()
    {
        // 그리드 청소 (기존 UI 오브젝트 삭제)
        foreach (Transform child in inventoryGrid) Destroy(child.gameObject);
        foreach (Transform child in quickSlotGrid) Destroy(child.gameObject);

        for (int i = 0; i < slots.Count; i++)
        {
            // 메인 인벤토리에 슬롯 생성
            GameObject newSlot = Instantiate(slotPrefab, inventoryGrid);
            UpdateSlotUI(newSlot, slots[i]);

            // 첫 10칸(0~9)은 퀵슬롯에도 생성
            if (i < 10)
            {
                GameObject quickSlot = Instantiate(slotPrefab, quickSlotGrid);
                UpdateSlotUI(quickSlot, slots[i]);
            }
        }
    }

    void UpdateSlotUI(GameObject slotObj, ItemSlot data)
    {
        Image iconImage = slotObj.transform.Find("Icon").GetComponent<Image>();
        Text countText = slotObj.transform.Find("Count").GetComponent<Text>();

        if (data.itemName != "")
        {
            iconImage.sprite = data.icon;
            iconImage.color = Color.white;
            countText.text = data.count > 1 ? data.count.ToString() : "";
        }
        else
        {
            iconImage.color = new Color(0, 0, 0, 0); // 투명
            countText.text = "";
        }
    }
}
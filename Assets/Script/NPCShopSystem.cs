using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPCShopSystem : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject dialoguePanel;
    public GameObject shopPanel;
    public GameObject interactionButtons;

    [Header("Texts & UI Elements")]
    public Text dialogueText;
    public Text itemCountText;      // 선택 수량
    public Text itemPriceText;      // 아이템 단가
    public Text totalPriceText;     // 총액 (단가 * 수량)
    public Text playerGoldText;     // 보유 소지금
    public Text shopTitleText;      // "구매" 또는 "판매" 상태 표시용

    [Header("Player Data")]
    public int playerGold = 1000;

    [Header("Item Settings")]
    public string currentItemName = "감자";
    public int buyPrice = 50;       // 살 때 가격
    public int sellPrice = 25;      // 팔 때 가격 (구매가의 50%)

    private int selectedCount = 0;
    private bool isSellingMode = false;

    void Start()
    {
        dialoguePanel.SetActive(false);
        shopPanel.SetActive(false);
        UpdateShopUI();
    }

    // 1. NPC 상호작용 시작
    public void StartInteraction()
    {
        dialoguePanel.SetActive(true);
        interactionButtons.SetActive(true);
        dialogueText.text = "어서오게! 물건을 사고 싶나, 아니면 팔러 왔나?";
    }

    // 2. 모드 선택 (구매 버튼 / 판매 버튼 클릭 시)
    public void OpenShop(bool isSellMode)
    {
        isSellingMode = isSellMode;
        interactionButtons.SetActive(false);
        shopPanel.SetActive(true);
        selectedCount = 0;

        shopTitleText.text = isSellingMode ? "상점 (판매)" : "상점 (구매)";
        UpdateShopUI();
    }

    // 3. 수량 조절 (+1, -1)
    public void ChangeCount(int amount)
    {
        selectedCount += amount;
        if (selectedCount < 0) selectedCount = 0;
        UpdateShopUI();
    }

    // 4. UI 갱신
    void UpdateShopUI()
    {
        int currentUnitPrice = isSellingMode ? sellPrice : buyPrice;
        itemCountText.text = selectedCount.ToString();
        itemPriceText.text = $"{currentUnitPrice} Gold";
        totalPriceText.text = $"총액: {currentUnitPrice * selectedCount} Gold";
        playerGoldText.text = $"보유액: {playerGold} Gold";
    }

    // 5. 확정 버튼 (구매/판매 실행)
    public void OnClickConfirm()
    {
        if (selectedCount <= 0) return;

        if (isSellingMode) ExecuteSell();
        else ExecuteBuy();
    }

    // --- 구매 로직 ---
    void ExecuteBuy()
    {
        int totalCost = buyPrice * selectedCount;
        if (playerGold >= totalCost)
        {
            playerGold -= totalCost;
            dialogueText.text = $"{currentItemName} {selectedCount}개를 구매했군. 고마워!";
            selectedCount = 0;
            UpdateShopUI();
        }
        else
        {
            dialogueText.text = "돈이 부족하구만. 다음에 다시 오게.";
        }
    }

    // --- 판매 로직 ---
    void ExecuteSell()
    {
        bool hasItem = true;

        if (hasItem)
        {
            int earnGold = sellPrice * selectedCount;
            playerGold += earnGold;

            dialogueText.text = $"상태가 좋군! {earnGold} Gold에 사겠네.";
            selectedCount = 0;
            UpdateShopUI();
        }
        else
        {
            dialogueText.text = "팔 물건이 없는 것 같은데? 다시 확인해보게.";
        }
    }

    // 6. 상점 닫기 및 종료
    public void CloseShopOrExit()
    {
        shopPanel.SetActive(false);
        interactionButtons.SetActive(false);
        StartCoroutine(ExitDialogueRoutine());
    }

    IEnumerator ExitDialogueRoutine()
    {
        dialogueText.text = "원하는 물건은 다 샀어? 조심히 가!";
        yield return new WaitForSeconds(1.5f);
        dialoguePanel.SetActive(false);
    }
}
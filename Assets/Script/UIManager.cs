using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject inventoryPanel;
    public GameObject questPanel;
    public GameObject equipmentPanel;
    public GameObject menuPanel;

    public PlayerController player;

    // UI가 하나라도 열려있는지 체크
    bool IsAnyWindowOpen()
    {
        return inventoryPanel.activeSelf || questPanel.activeSelf ||
               equipmentPanel.activeSelf || menuPanel.activeSelf;
    }

    public void ToggleInventory() => ToggleWindow(inventoryPanel);
    public void ToggleQuestWindow() => ToggleWindow(questPanel);
    public void ToggleEquipmentWindow() => ToggleWindow(equipmentPanel);
    public void ToggleMainMenu() => ToggleWindow(menuPanel);

    void ToggleWindow(GameObject panel)
    {
        bool isActive = !panel.activeSelf;
        panel.SetActive(isActive);

        // UI가 열리면 플레이어 상태를 MenuOpen으로, 다 닫히면 Idle로 변경
        if (IsAnyWindowOpen())
        {
            player.SetState(PlayerController.PlayerState.MenuOpen);
        }
        else
        {
            player.SetState(PlayerController.PlayerState.Idle);
        }
    }
}
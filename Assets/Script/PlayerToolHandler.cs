using UnityEngine;

public class PlayerToolHandler : MonoBehaviour
{
    // 도구 등급 정의
    public enum ToolGrade { Stone, Copper, Iron, Gold }

    [Header("Current Equipment")]
    public string currentToolName = "StonePickaxe";
    public ToolGrade currentGrade = ToolGrade.Stone;

    [Header("Interaction Settings")]
    public float interactRange = 1.5f; // 도구 사거리
    public LayerMask resourceLayer;    // 광물/나무/작물 레이어 설정

    void Update()
    {
        // 시각적인 디버그 라인 (에디터 뷰에서 사거리 확인용)
        Debug.DrawRay(transform.position, transform.right * interactRange, Color.red);
    }

    // [중요] public 키워드가 있어야 PlayerController에서 호출 가능합니다.
    public void UseTool()
    {
        // 1. 도구 등급에 따른 데미지(파워) 결정
        int damage = GetToolPower();

        // 2. 레이캐스트를 사용하여 전방의 오브젝트 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, interactRange, resourceLayer);

        if (hit.collider != null)
        {
            // 상호작용 대상 1: 광물 (채광)
            Mineral mineral = hit.collider.GetComponent<Mineral>();
            if (mineral != null)
            {
                mineral.TakeDamage(damage);
                Debug.Log($"{currentToolName}로 {mineral.mineralName}에 {damage} 데미지!");
                return;
            }

            // 상호작용 대상 2: 작물 (물 주기)
            Crop crop = hit.collider.GetComponent<Crop>();
            if (crop != null)
            {
                if (currentToolName.Contains("WateringCan"))
                {
                    crop.WaterCrop();
                    Debug.Log("작물에 물을 주었습니다.");
                }
                return;
            }
        }
        else
        {
            Debug.Log("허공에 도구를 휘둘렀습니다.");
        }
    }

    // 도구 등급에 따른 파워 계산
    private int GetToolPower()
    {
        switch (currentGrade)
        {
            case ToolGrade.Stone: return 1;
            case ToolGrade.Copper: return 2;
            case ToolGrade.Iron: return 4;
            case ToolGrade.Gold: return 8;
            default: return 1;
        }
    }

    // [중요] 도구 강화 시 ToolUpgradeManager에서 호출하여 정보를 갱신함
    public void SetUpgrade(ToolGrade newGrade, string newName)
    {
        currentGrade = newGrade;
        currentToolName = newName;
        Debug.Log($"도구가 강화되었습니다: {newName} (등급: {newGrade})");
    }
} // 이 마지막 중괄호가 클래스를 닫아주는 아주 중요한 괄호입니다!
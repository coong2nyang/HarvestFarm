using UnityEngine;

public class Crop : MonoBehaviour
{
    public float growthTime;
    public int currentStage = 0;
    private bool isWatered = false;
    private float timer;

    [Header("Visuals")]
    public SpriteRenderer spriteRenderer; // 작물 이미지를 보여줄 렌더러
    public Sprite[] growthSprites;       // 0: 씨앗, 1: 떡잎, 2: 성장, 3: 수확가능 (총 4개 필요)

    void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateCropVisual(); // 초기 이미지 설정
    }

    void Update()
    {
        // 물을 준 상태이고, 아직 다 자라지 않았다면 성장
        if (isWatered && currentStage < 3)
        {
            timer += Time.deltaTime;
            if (timer >= growthTime / 3)
            {
                currentStage++;
                UpdateCropVisual(); // 성장할 때마다 이미지 업데이트
                timer = 0;
                isWatered = false; // 다음 단계 성장을 위해 물을 다시 줘야 함
            }
        }
    }

    // 에러 해결: 호출되던 함수를 정의함
    void UpdateCropVisual()
    {
        if (growthSprites != null && currentStage < growthSprites.Length)
        {
            spriteRenderer.sprite = growthSprites[currentStage];
        }
    }

    public void WaterCrop() => isWatered = true;
}
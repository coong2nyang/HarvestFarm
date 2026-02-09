using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Move, Interact, MenuOpen }
    public PlayerState currentState = PlayerState.Idle;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator anim; // 애니메이션이 있다면 연결

    [Header("References")]
    public UIManager uiManager;
    public PlayerToolHandler toolHandler; // 이전 단계에서 만든 도구 핸들러

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 메뉴가 열려있으면 이동 입력을 받지 않음
        if (currentState == PlayerState.MenuOpen)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        HandleInput();
    }

    void FixedUpdate()
    {
        if (currentState != PlayerState.MenuOpen)
        {
            Move();
        }
    }

    void HandleInput()
    {
        // 1. WASD - 움직이기
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero) currentState = PlayerState.Move;
        else currentState = PlayerState.Idle;

        // 2. F / 좌클릭 - 상호작용 및 도구 사용
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            toolHandler.UseTool(); // 앞서 만든 도구 사용 로직 호출
        }

        // 3. Q - 퀘스트 정보
        if (Input.GetKeyDown(KeyCode.Q)) uiManager.ToggleQuestWindow();

        // 4. E - 장비창
        if (Input.GetKeyDown(KeyCode.E)) uiManager.ToggleEquipmentWindow();

        // 5. I / Tab - 인벤토리
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab)) uiManager.ToggleInventory();

        // 6. ESC - 메뉴 (게임 일시정지 등)
        if (Input.GetKeyDown(KeyCode.Escape)) uiManager.ToggleMainMenu();
    }

    void Move()
    {
        rb.velocity = moveInput.normalized * moveSpeed;

        // 애니메이터 파라미터 설정 (있을 경우)
        // anim.SetFloat("Speed", moveInput.magnitude);
    }

    public void SetState(PlayerState newState)
    {
        currentState = newState;
    }
}
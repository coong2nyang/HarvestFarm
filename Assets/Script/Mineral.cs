using UnityEngine;

public class Mineral : MonoBehaviour
{
    public string mineralName;
    public int health = 10;        // 광물 체력
    public GameObject dropItemPrefab; // 파괴 시 떨어질 아이템 프리팹

    public void TakeDamage(int amount)
    {
        health -= amount;

        // 흔들림 효과나 파티클을 여기서 재생할 수 있습니다.

        if (health <= 0)
        {
            Break();
        }
    }

    void Break()
    {
        // 아이템 드랍
        Instantiate(dropItemPrefab, transform.position, Quaternion.identity);
        // 광물 제거
        Destroy(gameObject);
    }
}
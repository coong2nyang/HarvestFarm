using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNode : MonoBehaviour
{
    public string itemName;
    public int amount = 1;
    public float pullSpeed = 5f;
    private Transform player;

    void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * pullSpeed);
            if (Vector3.Distance(transform.position, player.position) < 0.2f)
            {
                InventoryManager.Instance.AddItem(itemName, amount);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) player = collision.transform;
    }
}

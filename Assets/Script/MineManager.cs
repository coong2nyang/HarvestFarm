using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    public GameObject[] orePrefabs; // 0:µ¹, 1:¼®Åº, 2:±¸¸®, 3:Ã¶, 4:±İ
    public int mineCount = 50;

    void Start() => GenerateOres();

    void GenerateOres()
    {
        for (int i = 0; i < mineCount; i++)
        {
            float rand = Random.value * 100;
            int index = 0;

            if (rand < 7) index = 4;      // ±İ 7%
            else if (rand < 15) index = 3; // Ã¶ 8%
            else if (rand < 25) index = 2; // ±¸¸® 10%
            else if (rand < 50) index = 1; // ¼®Åº 25%
            else index = 0;                // µ¹ 50%

            Vector2 spawnPos = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
            Instantiate(orePrefabs[index], spawnPos, Quaternion.identity);
        }
    }
}
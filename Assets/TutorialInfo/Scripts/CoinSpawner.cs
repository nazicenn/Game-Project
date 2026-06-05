using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 3f;

    private float[] lanePositions = { -2.5f, 0f, 2.5f };
    private List<GameObject> coins = new List<GameObject>();
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCoinLine();
            timer = 0;
            spawnInterval = Random.Range(2f, 4f);
        }

        for (int i = coins.Count - 1; i >= 0; i--)
        {
            if (coins[i] != null)
            {
                coins[i].transform.Translate(Vector3.back * GroundSpawner.moveSpeed * Time.deltaTime);

                if (coins[i].transform.position.z < -15f)
                {
                    Destroy(coins[i]);
                    coins.RemoveAt(i);
                }
            }
            else
            {
                coins.RemoveAt(i);
            }
        }
    }

    void SpawnCoinLine()
    {
        if (coinPrefab == null) return;

        int lane = Random.Range(0, 3);

        int coinCount = Random.Range(4, 10);

        float startZ = 28f;
        float spacing = 2f;

        for (int i = 0; i < coinCount; i++)
        {
            float zPos = startZ + (i * spacing);
            Vector3 pos = new Vector3(lanePositions[lane], 0.5f, zPos);

            GameObject newCoin = Instantiate(coinPrefab, pos, Quaternion.identity);
            newCoin.tag = "Coin";
            coins.Add(newCoin);
        }
    }
}
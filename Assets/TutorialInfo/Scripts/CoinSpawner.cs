using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;

    private float[] lanePositions = { -2.5f, 0f, 2.5f };
    private List<GameObject> coins = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + Random.Range(1f, 2f);
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCoins();
            nextSpawnTime = Time.time + Random.Range(3f, 5f);
        }

        for (int i = coins.Count - 1; i >= 0; i--)
        {
            if (coins[i] != null)
            {
                coins[i].transform.Translate(Vector3.back * GroundSpawner.moveSpeed * Time.deltaTime);

                if (coins[i].transform.position.z < -10f)
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

    void SpawnCoins()
    {
        if (coinPrefab == null) return;

        int totalCoinCount = Random.Range(3, 9);
        List<int> availableLanes = new List<int> { 0, 1, 2 };
        int laneCount = Random.Range(1, 3);
        List<int> usedLanes = new List<int>();

        for (int i = 0; i < laneCount; i++)
        {
            int randomIndex = Random.Range(0, availableLanes.Count);
            usedLanes.Add(availableLanes[randomIndex]);
            availableLanes.RemoveAt(randomIndex);
        }

        int remainingCoins = totalCoinCount;

        for (int l = 0; l < usedLanes.Count; l++)
        {
            int lane = usedLanes[l];
            int coinsInThisLane;

            if (l == usedLanes.Count - 1)
            {
                coinsInThisLane = remainingCoins;
            }
            else
            {
                coinsInThisLane = Random.Range(1, remainingCoins - (usedLanes.Count - l - 1));
            }

            remainingCoins -= coinsInThisLane;
            float startZ = 28f;
            float spacing = 2.2f;

            for (int i = 0; i < coinsInThisLane; i++)
            {
                float zPos = startZ + (i * spacing);
                Vector3 pos = new Vector3(lanePositions[lane], 0.7f, zPos);

                Collider[] hitColliders = Physics.OverlapSphere(pos, 0.8f);
                bool isSafe = true;

                foreach (Collider col in hitColliders)
                {
                    if (col.CompareTag("Obstacle"))
                    {
                        isSafe = false;
                        break;
                    }
                }

                if (isSafe)
                {
                    GameObject newCoin = Instantiate(coinPrefab, pos, Quaternion.identity);
                    newCoin.tag = "Coin";
                    coins.Add(newCoin);
                }
            }
        }
    }
}
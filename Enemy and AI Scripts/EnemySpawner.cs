using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float lastEnemySpawned;
    public float enemySpawnRate = 5;
    public GameObject enemy;

    private void Start()
    {
        lastEnemySpawned = Time.time;
    }

    void Update()
    {
        if (Time.time > lastEnemySpawned + enemySpawnRate)
        {
            Debug.Log("Enemy Spawned");
            Instantiate(enemy, Vector3.up * 3, Quaternion.Euler(0, 0, 0));
            lastEnemySpawned = Time.time;
        }
    }
}

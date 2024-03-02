using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private EnemySpawnerDataScriptableObject _enemySpawnerData;
    public int CurrentWave { get; private set; } = 0;

    public void StartNextWave()
    {
        if (_enemySpawnerData.EnemiesOfWaves.Length - 1 <= CurrentWave)
        {
            Debug.Log("Can't start next wave. All waves completed.");
        }
        else
        {
            CurrentWave++;
            StartCoroutine(_enemySpawner.StartSpawner());
        }
    }
}

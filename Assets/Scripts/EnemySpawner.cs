using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player _player;
    [SerializeField] private EnemySpawnerDataScriptableObject _enemySpawnerData;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameManager _gameManager;
    [Header("Settings")]
    [SerializeField] private float _spawnRate = 3f;
    [SerializeField] private bool _isSpawnerActive = true;
    [SerializeField] private List<Transform> _spawnLocations;
    private Queue<BaseEnemy> _spawnQueue;

    private void Start()
    {
        StartCoroutine(StartSpawner());
    }

    public IEnumerator StartSpawner()
    {
        EnqueueEnemies();
        _isSpawnerActive = true;

        while (_isSpawnerActive && !IsAllEnemiesSpawned())
        {
            yield return new WaitForSeconds(_spawnRate);
            SpawnEnemyFromQueue();
        }

        if (IsAllEnemiesSpawned())
        {
            _isSpawnerActive = false;
            StopCoroutine(StartSpawner());
            Debug.Log("Spawner deactivated.");


            //TODO: DELETE HERE AND IMPLEMENT TO UI (START NEXT WAVE OR SOMETHING)
            Debug.Log("Starting next wave...");
            _gameManager.StartNextWave();
        }
    }

    private bool IsAllEnemiesSpawned()
    {
        return _spawnQueue.Count <= 0;
    }

    private void EnqueueEnemies()
    {
        _spawnQueue = new Queue<BaseEnemy>();
        _enemySpawnerData.EnemiesOfWaves[_gameManager.CurrentWave]._spawnList.ForEach(spawnList => _spawnQueue.Enqueue(spawnList._enemyPrefab));

        foreach (var item in _spawnQueue)
        {
            Debug.Log("Queue" + item.gameObject.name);
        }
    }

    private void SpawnEnemyFromQueue()
    {
        BaseEnemy _enemy = _spawnQueue.Peek();
        Transform _randomSpawnLocation = _spawnLocations[Random.Range(0, _spawnLocations.Count)];
        BaseEnemy _newEnemy = Instantiate(_enemy, _randomSpawnLocation.position, _enemy.gameObject.transform.rotation, _parent);
        _newEnemy.Player = _player;
        _spawnQueue.Dequeue();
    }
}

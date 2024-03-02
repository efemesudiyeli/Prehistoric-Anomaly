using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _spawnRate = 3f;
    [SerializeField] private bool _isSpawnerActive = true;
    [SerializeField] private EnemySpawnerDataScriptableObject _enemySpawnerData;
    [SerializeField] private List<Transform> _spawnLocations;
    [SerializeField] private Transform _parent;

    private Queue<BaseEnemy> _spawnQueue;

    private void Awake()
    {
        EnqueueEnemies();
    }

    private void Start()
    {
        StartCoroutine(StartSpawner());
    }

    private IEnumerator StartSpawner()
    {
        while (_isSpawnerActive && _spawnQueue.Count > 0)
        {
            yield return new WaitForSeconds(_spawnRate);
            SpawnEnemyFromQueue();
        }
    }

    private void EnqueueEnemies()
    {
        _spawnQueue = new Queue<BaseEnemy>();
        _enemySpawnerData.EnemyListToSpawn[0]._spawnList.ForEach(spawnList => _spawnQueue.Enqueue(spawnList._enemyPrefab));
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

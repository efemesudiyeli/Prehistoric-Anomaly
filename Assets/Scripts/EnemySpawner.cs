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
    [SerializeField] private HitReceiveFlashEffect _hitReceiver;
    [Header("Settings")]
    [SerializeField] private float _spawnRate = 3f;
    [SerializeField] private bool _isSpawnerActive = true;
    [SerializeField] private List<Transform> _spawnLocations;
    private Queue<BaseEnemy> _spawnQueue;
    private int _firstWaveEnemyCount;
    private int _deadEnemyCount = 0;
    public int CurrentWave { get; private set; } = 0;
    private void Start()
    {
        StartCoroutine(StartSpawner());
    }

    public IEnumerator StartSpawner()
    {
        _isSpawnerActive = true;
        EnqueueEnemies();
        Debug.Log("Starting wave " + CurrentWave + " with " + _enemySpawnerData.EnemiesOfWaves[CurrentWave]._spawnList.Count + " enemies.");

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
        }
    }

    private bool IsAllEnemiesSpawned()
    {
        return _spawnQueue.Count <= 0;
    }

    public void StartNextWave()
    {
        if (_enemySpawnerData.EnemiesOfWaves.Length - 1 <= CurrentWave)
        {
            Debug.Log("Can't start next wave. All waves completed.");
            //TODO: PLAY FINAL CUTSCENE 
        }
        else
        {
            CurrentWave++;
            StartCoroutine(StartSpawner());
        }
    }

    private void EnqueueEnemies()
    {
        _spawnQueue = new Queue<BaseEnemy>();
        _enemySpawnerData.EnemiesOfWaves[CurrentWave]._spawnList.ForEach(spawnList => _spawnQueue.Enqueue(spawnList._enemyPrefab));

        AssignEnemyCountOnFirstWave();
    }

    private void AssignEnemyCountOnFirstWave()
    {
        if (_firstWaveEnemyCount == 0)
        {
            _firstWaveEnemyCount = _spawnQueue.Count;
        }
    }

    private void SpawnEnemyFromQueue()
    {
        BaseEnemy _enemy = _spawnQueue.Peek();
        Transform _randomSpawnLocation = _spawnLocations[Random.Range(0, _spawnLocations.Count)];
        BaseEnemy _newEnemy = Instantiate(_enemy, _randomSpawnLocation.position, _enemy.gameObject.transform.rotation, _parent);
        _newEnemy.Player = _player;
        _newEnemy.HitReceiver = _hitReceiver;
        TrackDeadEnemiesForNextWave(_newEnemy);
        _spawnQueue.Dequeue();
    }

    private void TrackDeadEnemiesForNextWave(BaseEnemy _newEnemy)
    {
        _newEnemy.OnDie += () =>
               {
                   _deadEnemyCount++;
                   //Debug.Log("deadenemycount: " + _deadEnemyCount + "total enemy count: " + _enemySpawnerData.EnemiesOfWaves[CurrentWave]._spawnList.Count);
                   if (CurrentWave == 0 && _deadEnemyCount == _firstWaveEnemyCount)
                   {
                       _gameManager.StartFirstTimelineOnce();
                       _deadEnemyCount = 0;
                   }
                   else if (_deadEnemyCount == _enemySpawnerData.EnemiesOfWaves[CurrentWave]._spawnList.Count)
                   {
                       Debug.Log("Next wave started.");
                       _deadEnemyCount = 0;
                       StartNextWave();
                   }
               };
    }
}

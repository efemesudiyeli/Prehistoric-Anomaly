using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerDataScriptableObject", menuName = "EnemySpawnerDataScriptableObject", order = 0)]
public class EnemySpawnerDataScriptableObject : ScriptableObject
{
    public EnemySpawnerData[] EnemiesOfWaves;

    [Serializable]
    public class EnemySpawnerData
    {
        public List<SpawnList> _spawnList;

        [Serializable]
        public class SpawnList
        {
            public BaseEnemy _enemyPrefab;
        }
    }
}
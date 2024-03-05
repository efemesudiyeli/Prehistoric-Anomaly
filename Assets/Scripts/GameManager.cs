using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private EnemySpawnerDataScriptableObject _enemySpawnerData;
    [SerializeField] private PlayableDirector _firstTimeline;
    public int CurrentWave { get; private set; } = 0;
    public bool IsInputsEnabled { get; set; } = true;

    public enum GameStates
    {
        TUTORIAL,
        GAMEPLAY,
    }
    public GameStates GameState { get; private set; } = GameStates.TUTORIAL;

    private void OnEnable()
    {
        _firstTimeline.stopped += (PlayableDirector director) =>
               {
                   IsInputsEnabled = true;
                   StartNextWave();
               };
    }

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

    public void StartFirstTimelineOnce()
    {
        if (GameState == GameStates.TUTORIAL)
        {
            Debug.Log("Start first timeline.");
            _firstTimeline.Play();
            IsInputsEnabled = false;
            GameState = GameStates.GAMEPLAY;
        }
    }
}

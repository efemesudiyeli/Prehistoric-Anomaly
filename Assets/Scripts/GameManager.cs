using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private EnemySpawnerDataScriptableObject _enemySpawnerData;
    [SerializeField] private PlayableDirector _firstTimeline;

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
                   _enemySpawner.StartNextWave();
               };
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartFirstTimelineOnce()
    {
        if (GameState == GameStates.TUTORIAL)
        {
            _firstTimeline.Play();
            IsInputsEnabled = false;
            GameState = GameStates.GAMEPLAY;
        }
    }
}

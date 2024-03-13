using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
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
    [SerializeField] private PlayableDirector _firstTimeline, _finalTimeline;

    public bool IsInputsEnabled { get; set; } = true;

    public enum GameStates
    {
        BAT,
        BOW,
        AK47,
        RPG,
        LIGHTSABER,
        ATOMICBOMB,
    }
    public GameStates GameState { get; set; } = GameStates.BAT;

    private void OnEnable()
    {
        if (_firstTimeline == null || _finalTimeline == null) return;

        _firstTimeline.stopped += (PlayableDirector director) => PrepareNextWave();
        _finalTimeline.stopped += (director) => LoadCredits();
    }

    private void OnDisable()
    {
        if (_firstTimeline == null || _finalTimeline == null) return;

        _firstTimeline.stopped -= (PlayableDirector director) => PrepareNextWave();
        _finalTimeline.stopped -= (director) => LoadCredits();
    }

    private void PrepareNextWave()
    {
        IsInputsEnabled = true;
        _enemySpawner.StartNextWave();
        Debug.Log("Game started.");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayFinalCutscene()
    {
        _finalTimeline.Play();
    }


}

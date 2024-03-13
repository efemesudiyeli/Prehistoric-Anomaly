using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private PlayableDirector _creditsTimeline;

    private void OnEnable()
    {
        _creditsTimeline.stopped += (PlayableDirector director) => { SceneManager.LoadScene(0); };
    }
}

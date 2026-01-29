using System;
using System.Collections;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SlidingTransition transition;
    private AsyncOperation asyncOperation;
    private Coroutine loadageCheckCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator CheckForSceneLoadage()
    {
        while (asyncOperation.progress < 0.89)
        {
            yield return null;
        }

        OnSceneLoaded();
    }

    public void LoadSceneAsync(string name)
    {
        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        asyncOperation.allowSceneActivation = false;
        StartTransition(PlayDirection.Forward);
        StartCoroutine(CheckForSceneLoadage());
    }

    private void StartTransition(PlayDirection direction)
    {
        transition.StartTransitionCoroutine(direction);
    }

    private void OnSceneLoaded()
    {
        if (transition.isRunning)
        {
            transition.OnTransitionFinished += ActivateScene;
        }
        else
        {
            ActivateScene(PlayDirection.Backward);
        }
    }

    private void ActivateScene(PlayDirection direction)
    {
        if (direction != PlayDirection.Forward) return;
        transition.OnTransitionFinished -= ActivateScene;
        asyncOperation.allowSceneActivation = true;
        StartTransition(PlayDirection.Backward);
    }
}

using System;
using System.Collections;
using EditorAttributes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SlidingTransition transition;
    private AsyncOperation asyncOperation;
    private Coroutine loadageCheckCoroutine;

    [SerializeField] private SODilema startDilema;
    [SerializeField] private SOActions startAction;
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;

    public static SceneManager instance;


    #region Get Positions
    
    public Transform GetPcTransform()
    {
        return pcTransform;
    }
    
    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator CheckForSceneLoadage()
    {
        while (asyncOperation.progress < 0.9)
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
        loadageCheckCoroutine = StartCoroutine(CheckForSceneLoadage());
    }

    private void StartTransition(PlayDirection direction)
    {
        transition.StartTransitionCoroutine(direction);
    }

    private void OnSceneLoaded()
    {
        StopCoroutine(loadageCheckCoroutine);
        
        if (transition.isRunning) {
            transition.OnTransitionFinished += () => 
            { 
                asyncOperation.allowSceneActivation = true;
                StartTransition(PlayDirection.Backward);
            };
        }
        else
        {
            asyncOperation.allowSceneActivation = true;
            StartTransition(PlayDirection.Backward);
        }
    }
}

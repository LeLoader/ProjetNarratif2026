using EditorAttributes;
using System;
using System.Collections;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

public class SlidingTransition : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float duration;
    [ReadOnly] public bool isRunning;

    public event Action<PlayDirection> OnTransitionFinished;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public Coroutine StartTransitionCoroutine(PlayDirection direction)
    {
        return StartCoroutine(StartTransition(direction));
    }

    private IEnumerator StartTransition(PlayDirection direction)
    {
        isRunning = true;

        float currentTime = 0;
        Vector3 startScale = direction == PlayDirection.Forward ? new Vector3(1,0,1) : Vector3.one;
        Vector3 endScale = direction == PlayDirection.Forward ? Vector3.one : new Vector3(1, 0, 1);

        if (rectTransform)
        {
            while (currentTime <= duration)
            {
                float alpha = currentTime / duration;
                currentTime += Time.deltaTime;
                rectTransform.localScale = Vector3.Lerp(startScale, endScale, alpha);
                yield return null;
            }
            rectTransform.localScale = endScale;
        }

        isRunning = false;
        OnTransitionFinished?.Invoke(direction);
    }
}

public enum PlayDirection
{
    Forward,
    Backward
}

using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action OnTimerPaused;
    public Action OnTimerUnpaused;
    public Action OnTimerElapsed;

    private bool bPause;
    private bool bAutoDestroy = true;

    private IEnumerator Internal_Timer(float duration)
    {
        float time = 0;
        while (time < duration && !bPause)
        {
            time += Time.deltaTime;
            yield return null;
        }
        OnTimerElapsed?.Invoke();

        if (bAutoDestroy)
        {
            Destroy(this);
        }
    }

    public void Internal_Start(float duration, bool bAutoDestroy)
    {
        this.bAutoDestroy = bAutoDestroy;
        StartCoroutine(Internal_Timer(duration));
    }

    public void Internal_Pause(bool bPause)
    {
        if (this.bPause == bPause) return;
        this.bPause = bPause;

        if (bPause) OnTimerPaused?.Invoke();
        else OnTimerUnpaused?.Invoke();
    }
}

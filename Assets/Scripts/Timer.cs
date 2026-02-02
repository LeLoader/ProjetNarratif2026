using EditorAttributes;
using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action OnTimerPaused;
    public Action OnTimerUnpaused;
    public Action OnTimerElapsed;

    private bool bPause = true;
    private bool bAutoDestroy = true;

    [SerializeField, ReadOnly] float actualTime = 0;
    public float duration;

    private void Update()
    {
        if (actualTime < duration && !bPause)
        {
            actualTime += Time.deltaTime;
        }
        else
        {
            OnTimerElapsed?.Invoke();
            if (bAutoDestroy)
            {
                Destroy(this);
            }
        }
    }

    public void Internal_Start(float duration, bool bAutoDestroy)
    {
        this.bAutoDestroy = bAutoDestroy;
        actualTime = 0;
        this.duration = duration;
    }

    public void Internal_Pause(bool bPause)
    {
        if (this.bPause == bPause) return;
        this.bPause = bPause;

        if (bPause) OnTimerPaused?.Invoke();
        else OnTimerUnpaused?.Invoke();
    }
}

using EditorAttributes;
using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action OnTimerPaused;
    public Action OnTimerUnpaused;
    public Action OnTimerElapsed;

    private bool bPause = false;
    private bool bAutoDestroy = true;

    [SerializeField, ReadOnly] float actualTime = 0;
    public float duration;

    private void Update()
    {
        if (actualTime < duration)
        {
            if (!bPause)
            {
                actualTime += Time.deltaTime;
            }
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

    private Timer Internal_Start(float duration, bool bAutoDestroy)
    {
        actualTime = 0;
        this.bAutoDestroy = bAutoDestroy;
        this.duration = duration;
        return this;
    }

    private void Internal_Pause(bool bPause)
    {
        if (this.bPause == bPause) return;
        this.bPause = bPause;

        if (bPause) OnTimerPaused?.Invoke();
        else OnTimerUnpaused?.Invoke();
    }

    public void SetPause(bool newPause)
    {
        Internal_Pause(newPause);
    }

    public void SetPlay()
    {
        Internal_Start(duration, bAutoDestroy);
    }

    public static Timer SetTimer(GameObject ctx, float time, bool autoDestroy)
    {
        Timer timer = ctx.AddComponent<Timer>();
        timer.Internal_Start(time, autoDestroy);
        return timer;
    }
}

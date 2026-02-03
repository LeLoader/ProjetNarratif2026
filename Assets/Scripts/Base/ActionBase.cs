using System;
using UnityEngine;
using System.Collections;

public enum EReturnState
{
    FAILED = 0,
    SUCCEEDED = 1
}

public class ActionBase : MonoBehaviour
{
    public string _componentName = "ActionBase";
    protected BehaviorController _behaviorController;
    public bool bHasReachedDestination = false;

    public virtual void Initialize(BehaviorController bh, int i)
    {
        _behaviorController = bh;
        if (_behaviorController == null) { Debug.LogError("BehaviorController is null in ActionBase Initialize"); }

        _componentName = this.GetType().Name + i;
        
        SubscribeToEvents();
        ExecuteAction();
    }
    private void SubscribeToEvents()
    {
        _behaviorController.OnDestinationReached += OnActionDestinationReached;
    }
    public virtual void ExecuteAction() { }
    public void ValidationAction(EReturnState returnState)
    { 
        _behaviorController.ActionCompleted();
    }
    public virtual void OnActionDestinationReached()
    {
        bHasReachedDestination = true;
    }
    private void OnDestroy()
    {
        if (_behaviorController != null)
        {
            _behaviorController.OnDestinationReached -= OnActionDestinationReached;
        }
    }
    
    public virtual bool StopAction()
    {
        return true;
    }
}

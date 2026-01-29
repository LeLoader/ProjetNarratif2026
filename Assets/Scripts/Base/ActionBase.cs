using System;
using UnityEngine;

public class ActionBase : MonoBehaviour
{
    public string _componentName = "ActionBase";
    protected BehaviorController _behaviorController;
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
    public void ValidationAction()
    { 
        _behaviorController.ActionCompleted();
    }
    public virtual void OnActionDestinationReached()
    {

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

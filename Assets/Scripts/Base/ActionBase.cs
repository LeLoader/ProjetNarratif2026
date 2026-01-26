using UnityEngine;

public class ActionBase : MonoBehaviour
{
    protected BehaviorController _behaviorController;
    public virtual void Initialize(BehaviorController bh)
    {
        _behaviorController = bh;
        if (_behaviorController == null) { Debug.LogError("BehaviorController is null in ActionBase Initialize"); }
        
        SubscribeToEvents();
        ExecuteAction();
    }
    
    private void SubscribeToEvents()
    {
        _behaviorController.OnActionCompleted += OnActionCompleted;
        _behaviorController.OnDestinationReached += OnActionDestinationReached;
    }
    
    public virtual void ExecuteAction() { }
    public virtual void OnActionCompleted() { }
    public virtual void OnActionDestinationReached() { }
    
    

}

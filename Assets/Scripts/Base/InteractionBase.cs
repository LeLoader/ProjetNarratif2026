using UnityEngine;

public class InteractionBase : MonoBehaviour
{
    public virtual void ExecuteInteraction(BehaviorController behaviorController)
    {
    }
    
    public virtual void OnContactWithOtherBehaviour(BehaviorController otherBehaviour)
    {
    }
}

using UnityEngine;

public abstract class ACT_GetObjectBase : ActionBase
{
    abstract protected GameObject Prefab { get; }

    public override void ExecuteAction()
    {
        ValidationAction(_behaviorController.SetObject(Instantiate(Prefab)) ? EReturnState.SUCCEEDED : EReturnState.FAILED);
    }
}

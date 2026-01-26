using UnityEngine;

public class ACT_GoToPc : ActionBase
{
    public override void ExecuteAction()
    {
        _behaviorController.MoveToPosition(SceneManager.instance.GetPcTransform().position, "Walk");
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        Debug.Log("PC reached");
        base.OnActionDestinationReached();
    }
}

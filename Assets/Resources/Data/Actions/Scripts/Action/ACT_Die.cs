using UnityEngine;

public class ACT_Die : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        _behaviorController.CallTriggerAnimation("die");

        // _behaviorController.GetAnimator()

        ValidationAction(EReturnState.SUCCEEDED);
    }
}

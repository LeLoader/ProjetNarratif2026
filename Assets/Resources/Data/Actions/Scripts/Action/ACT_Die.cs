using UnityEngine;

[CreateAssetMenu(fileName = "ACT_Die", menuName = "Scriptable Objects/ACT_Die")]
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

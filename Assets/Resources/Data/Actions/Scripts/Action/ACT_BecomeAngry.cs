using UnityEngine;

public class ACT_BecomeAngry : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.ChangeMetricState(EMetricType.VIOLENCE, EMetricState.NEGATIVE);
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

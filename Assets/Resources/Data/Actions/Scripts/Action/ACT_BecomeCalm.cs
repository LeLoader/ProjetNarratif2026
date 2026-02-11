using UnityEngine;

public class ACT_BecomeCalm : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.ChangeMetricState(EMetricType.VIOLENCE, EMetricState.POSITIVE);
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

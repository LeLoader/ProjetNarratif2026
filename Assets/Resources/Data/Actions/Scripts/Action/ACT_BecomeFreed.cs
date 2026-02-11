using UnityEngine;

public class ACT_BecomeFreed : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.ChangeMetricState(EMetricType.INDOCTRINATED, EMetricState.POSITIVE);
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

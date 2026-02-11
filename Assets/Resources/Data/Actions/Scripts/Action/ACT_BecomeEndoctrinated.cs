using UnityEngine;

public class ACT_BecomeEndoctrinated : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.ChangeMetricState(EMetricType.INDOCTRINATED, EMetricState.NEGATIVE);
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

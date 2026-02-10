using UnityEngine;

public class ACTI_Kill : ActionBase
{
    public override void ExecuteAction()
    {
        Debug.Log("Killing");
        base.ExecuteAction();
        if (_behaviorController.metrics[EMetricType.VIOLENCE] != EMetricState.NEUTRAL)
        {
            _behaviorController.GetOtherBehavior().Die();
            ValidationAction(EReturnState.SUCCEEDED);
        }

    }

}

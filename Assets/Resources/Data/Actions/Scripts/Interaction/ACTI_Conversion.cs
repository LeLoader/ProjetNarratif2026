using System.Collections;
using UnityEngine;

public class ACTI_Conversion : ActionBase
{
    public override void ExecuteAction()
    {
        Debug.Log("Converting");
        base.ExecuteAction();
        if (_behaviorController.metrics[EMetricType.INDOCTRINATED] != EMetricState.NEUTRAL)
        {
            _behaviorController.GetOtherBehavior().metrics[EMetricType.INDOCTRINATED] = _behaviorController.metrics[EMetricType.INDOCTRINATED];
            ValidationAction(EReturnState.SUCCEEDED);
        }
    }
}

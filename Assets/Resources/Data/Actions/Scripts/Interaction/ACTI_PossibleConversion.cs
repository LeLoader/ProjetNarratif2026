using UnityEngine;

public class ACTI_PossibleConversion : ActionBase
{
    public override void ExecuteAction()
    {
        Debug.Log("May convert");
        base.ExecuteAction();
        if (_behaviorController.metrics[EMetricType.INDOCTRINATED] != EMetricState.NEUTRAL)
        {
            if (Random.Range(0, 2) == 1)
            {
                _behaviorController.GetOtherBehavior().metrics[EMetricType.INDOCTRINATED] = _behaviorController.metrics[EMetricType.INDOCTRINATED];
            } else
            {
                Debug.Log("[POSSIBLE CONVERSION] no conversion happened");
            }
                ValidationAction(EReturnState.SUCCEEDED);
        }

    }
}

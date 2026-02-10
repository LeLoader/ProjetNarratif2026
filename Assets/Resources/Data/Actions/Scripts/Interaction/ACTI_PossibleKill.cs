using UnityEngine;

public class ACTI_PossibleKill : ActionBase
{
    public override void ExecuteAction()
    {
        Debug.Log("May Kill");
        base.ExecuteAction();
        if (_behaviorController.metrics[EMetricType.VIOLENCE] != EMetricState.NEUTRAL)
        {
            if (Random.Range(0, 2) == 1)
            {
                _behaviorController.GetOtherBehavior().Die();
            } else
            {
                Debug.Log("[POSSIBLE KILL] kill failed");
            }
                ValidationAction(EReturnState.SUCCEEDED);
        }

    }
}

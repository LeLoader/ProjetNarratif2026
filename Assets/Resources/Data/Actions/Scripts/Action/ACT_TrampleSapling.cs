using System;
using System.Collections;
using UnityEngine;

public class ACT_TrampleSapling : ActionBase
{
    GameObject targetSapling;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameObject[] saplings = GameObject.FindGameObjectsWithTag("Sapling");
        if (saplings.Length >= 1)
        {
            targetSapling = saplings[0];
            _behaviorController.MoveToPosition(targetSapling.transform.position);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();

        Destroy(targetSapling);
        _behaviorController.CallTriggerAnimation("stomp");

        Action action = () => { ValidationAction(EReturnState.SUCCEEDED); };
        StartCoroutine(WaitForEndOfAnimation(action, _behaviorController));
    }

    public IEnumerator WaitForEndOfAnimation(Action action, BehaviorController controller)
    {
        while (controller.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        action?.Invoke();
    }

    public override bool StopAction(EStopActionReason reason)
    {
        return false;
    }
}

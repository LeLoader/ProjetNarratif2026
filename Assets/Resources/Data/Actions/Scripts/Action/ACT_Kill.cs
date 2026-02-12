using System.Collections;
using UnityEngine;

public class ACT_Kill : ActionBase
{
    BehaviorController target;
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.SetInteractState(false);
        target = CharacterBuilderManager.Instance.GetRandomBehaviorControllerNotInteracting();
        target.SetInteractState(false);
        _behaviorController.MoveToPosition(target.transform.position);
        target.StopAi();
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        StartCoroutine(KillHuman());
    }

    private IEnumerator KillHuman()
    {
        Coroutine coroutineFirst = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(target.transform));
        Coroutine coroutineSecond = target.StartCoroutine(target.RotateTowardsTarget(_behaviorController.transform));
        yield return coroutineFirst;
        yield return coroutineSecond;
        if (_behaviorController.metrics[EMetricType.VIOLENCE] == EMetricState.NEGATIVE && ActionLogger.GetActionCount("ACT_GetWeapon") >= 1)
        {
            _behaviorController.CallTriggerAnimation("killPistol");
            target.CallTriggerAnimation("diePistol");
        } else
        {
            _behaviorController.CallTriggerAnimation("killHand");
            target.CallTriggerAnimation("dieHand");
        }
        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && !_behaviorController.GetAnimator().IsInTransition(0))
        {
            yield return null;
        }
        target.Die();
        ValidationAction(EReturnState.SUCCEEDED);
        _behaviorController.SetInteractState(true);
    }
}

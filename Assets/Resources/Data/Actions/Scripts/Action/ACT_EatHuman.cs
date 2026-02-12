using System.Collections;
using UnityEngine;

public class ACT_EatHuman : ActionBase
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
        StartCoroutine(EatHuman());
    }

    private IEnumerator EatHuman()
    {
        Coroutine coroutineFirst = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(target.transform));
        Coroutine coroutineSecond = target.StartCoroutine(target.RotateTowardsTarget(_behaviorController.transform));
        yield return coroutineFirst;
        yield return coroutineSecond;
        _behaviorController.CallTriggerAnimation("Cannibalism");
        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        _behaviorController.GetOtherBehavior().Die();
        ValidationAction(EReturnState.SUCCEEDED);
        _behaviorController.SetInteractState(true);
    }
}

using System.Collections;
using UnityEngine;

public class ACT_Talk : ActionBase
{
    BehaviorController target;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        target = CharacterBuilderManager.Instance.GetRandomBehaviorControllerNotInteracting();
        target.SetInteractState(false);
        _behaviorController.FollowTarget(target.transform);
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        StartCoroutine(Talk());
    }

    private IEnumerator Talk()
    {
        Coroutine coroutineFirst = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(target.transform));
        Coroutine coroutineSecond = target.StartCoroutine(target.RotateTowardsTarget(_behaviorController.transform));
        yield return coroutineFirst;
        yield return coroutineSecond;
        _behaviorController.CallTriggerAnimation("Talk");
        target.CallTriggerAnimation("Talk");
        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        ValidationAction(EReturnState.SUCCEEDED);
        _behaviorController.SetInteractState(true);
        target.SetInteractState(true);
    }
}

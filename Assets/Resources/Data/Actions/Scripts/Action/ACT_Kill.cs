using System.Collections;
using UnityEngine;

public class ACT_Kill : ActionBase
{
    BehaviorController target;
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.SetInteractState(false);
        for (int i = 0; i <= 5; i++) {
            target = CharacterBuilderManager.Instance.GetRandomBehaviorControllerNotInteracting();
            if (target != null && target != _behaviorController) break;
        }
        if (target == null || target == _behaviorController) ValidationAction(EReturnState.FAILED);
        target.SetInteractState(false);
        Vector3 unitVector = target.transform.position - transform.position;
        float magnitude = unitVector.magnitude;
        unitVector.Normalize();
        _behaviorController.MoveToPosition(target.transform.position * 0.99f);
        target.StopAi();
        _behaviorController.ChangeMetricState(EMetricType.VIOLENCE, EMetricState.NEGATIVE);
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
            SoundManager.Instance.PlaySound("SFX_Shoot");
        } else
        {
            _behaviorController.CallTriggerAnimation("killHand");
            SoundManager.Instance.PlaySound("SFX_Strangle");
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

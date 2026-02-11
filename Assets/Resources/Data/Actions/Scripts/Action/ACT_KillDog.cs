using System.Collections;
using UnityEngine;

public class ACT_KillDog : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.FollowTarget(CharacterBuilderManager.Instance.dog.transform);
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        StartCoroutine(KillDog());
    }

    private IEnumerator KillDog()
    {
        BehaviorController dog = CharacterBuilderManager.Instance.dog;
        base.OnActionDestinationReached();
        Vector3 finalDestination = _behaviorController.transform.position - dog.transform.position;
        finalDestination.Normalize();
        finalDestination = Quaternion.Euler(0, 90, 0) * finalDestination;
        Transform tempTransform = dog.transform;
        tempTransform.position += finalDestination;
        Coroutine coroutineFirst = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(dog.transform));
        Coroutine coroutineSecond = dog.StartCoroutine(dog.RotateTowardsTarget(tempTransform));
        yield return coroutineFirst;
        yield return coroutineSecond;
        _behaviorController.CallTriggerAnimation("killPistol");
        dog.CallTriggerAnimation("die");
        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        ValidationAction(EReturnState.SUCCEEDED);

    }

    public override bool StopAction()
    {
        return false;
    }
}

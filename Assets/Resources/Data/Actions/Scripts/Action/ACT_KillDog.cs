using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ACT_KillDog : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.MoveToPosition(CharacterBuilderManager.Instance.dog.transform.position * 0.99f);
        CharacterBuilderManager.Instance.dog.StopAi();
        _behaviorController.ChangeMetricState(EMetricType.VIOLENCE, EMetricState.NEGATIVE);
        _behaviorController.SetObject(Instantiate(PrefabStaticRef.so.animationPistolPrefab), "BNS_R_Arm_end");
        SoundManager.Instance.PlaySound("SFX_Get_Weapon");
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
        // DOG
        Vector3 finalDestination = _behaviorController.transform.position - dog.transform.position;
        finalDestination.Normalize();
        finalDestination = Quaternion.Euler(0, 90, 0) * finalDestination;
        Transform tempTransform = dog.transform;
        tempTransform.position += finalDestination;
        Coroutine coroutineSecond = dog.StartCoroutine(dog.RotateTowardsTarget(tempTransform));
        // HUMAN
        Vector3 finalDestinationHuman = _behaviorController.transform.position - dog.transform.position;
        finalDestinationHuman.Normalize();
        finalDestinationHuman = Quaternion.Euler(0, 180, 0) * finalDestinationHuman;
        Transform tempTransformHuman = _behaviorController.transform;
        tempTransformHuman.position += finalDestinationHuman;
        Coroutine coroutineFirst = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(tempTransformHuman));
        //
        yield return coroutineFirst;
        yield return coroutineSecond;
        _behaviorController.CallTriggerAnimation("killPistol");
        dog.CallTriggerAnimation("die");
        SoundManager.Instance.PlaySound("SFX_Shoot");

        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5 && !_behaviorController.GetAnimator().IsInTransition(0))
        {
            yield return null;
        }

        _behaviorController.SetObject(null);
        dog.Die();
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

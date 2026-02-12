using UnityEngine;

public class ACT_Sleep : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(5f, 15f, out Vector3 position))
        {
            _behaviorController.MoveToPosition(position);
        }
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        _behaviorController.CallTriggerAnimation("Sleep");
        Timer.SetTimer(gameObject, 10f).OnTimerElapsed += () =>
        {
            ValidationAction(EReturnState.SUCCEEDED);
        };
    }
}

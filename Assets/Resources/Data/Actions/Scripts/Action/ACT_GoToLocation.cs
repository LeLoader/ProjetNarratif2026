using System.Collections;
using UnityEngine;

public class ACT_GoToLocation : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        Vector3 targetDestination;
        if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(0f, 10f, out targetDestination))
        {
            _behaviorController.MoveToPosition(targetDestination, "Walk");
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        StartCoroutine(LookAtPc());
    }

    private IEnumerator LookAtPc()
    {
        Coroutine Turn = StartCoroutine(_behaviorController.RotateTowardsTarget(SceneManager.instance.GetPcTransform()));
        yield return Turn;
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

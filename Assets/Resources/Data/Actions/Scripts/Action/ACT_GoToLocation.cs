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

        ValidationAction(EReturnState.SUCCEEDED);
    }
}

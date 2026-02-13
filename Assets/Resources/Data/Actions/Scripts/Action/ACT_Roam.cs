using UnityEngine;

public class ACT_Roam : ActionBase
{
    private GameObject _debugRoamPoint;
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (_behaviorController.GameOver)
        {
            return;
        }

        Vector3 targetDestination;
        if (SceneManager.instance.GetRandomPointInNavMesh(_behaviorController.transform.position, out targetDestination)){ 
            _debugRoamPoint = SceneManager.instance.SpawnDebugRoamPoint(targetDestination);
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

        if (_debugRoamPoint != null) { Destroy(_debugRoamPoint); }
        ValidationAction(EReturnState.SUCCEEDED);
    }

    public override bool StopAction(EStopActionReason reason)
    {
        return true;
    }
}

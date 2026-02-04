using UnityEngine;

public class ACT_Roam : ActionBase
{
    private GameObject _debugRoamPoint;
    public override void ExecuteAction()
    {
        Vector3 targetDestination;
        SceneManager.instance.GetRandomPointInNavMesh(_behaviorController.transform.position, out targetDestination);
        while (!_behaviorController.CanReachDestination(targetDestination))
        {
            SceneManager.instance.GetRandomPointInNavMesh(_behaviorController.transform.position, out targetDestination);
        }
        
        _debugRoamPoint = SceneManager.instance.SpawnDebugRoamPoint(targetDestination);
        _behaviorController.MoveToPosition(targetDestination, "Walk");
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();

        if (_debugRoamPoint != null) { Destroy(_debugRoamPoint); }
        ValidationAction(EReturnState.SUCCEEDED);
    }

    public override bool StopAction()
    {
        if (_debugRoamPoint != null) { Destroy(_debugRoamPoint); }
        return base.StopAction();
    }
}

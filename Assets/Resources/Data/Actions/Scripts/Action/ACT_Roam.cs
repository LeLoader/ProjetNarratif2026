using UnityEngine;

public class ACT_Roam : ActionBase
{
    private GameObject _debugRoamPoint;
    public override void ExecuteAction()
    {
        var targetDestination = SceneManager.instance.GetRandomRoamPoint();
        while (!_behaviorController.CanReachDestination(targetDestination))
        {
            targetDestination = SceneManager.instance.GetRandomRoamPoint();
        }
        
        _debugRoamPoint = SceneManager.instance.SpawnDebugRoamPoint(targetDestination);
        _behaviorController.MoveToPosition(targetDestination, "Walk");
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        if (_debugRoamPoint != null) { Destroy(_debugRoamPoint); }
        ValidationAction();
    }

    public override bool StopAction()
    {
        if (_debugRoamPoint != null) { Destroy(_debugRoamPoint); }
        return base.StopAction();
    }
}

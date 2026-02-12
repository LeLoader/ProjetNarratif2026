using System;
using UnityEngine;

public class ACT_GoToPc : ActionBase
{
    public static event Action<Vector3, Action> OnComputerReached;
    public override void ExecuteAction()
    {
        _behaviorController.MoveToPosition(SceneManager.instance.GetPcTransform().position, "Walk");
        base.ExecuteAction();

        _behaviorController.GetNavMeshAgent().avoidancePriority = 0;
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        var myDilemma = DilemmaManager.instance.GetCurrentDilema();
        Action ShowDilemma = () =>
        {
            CanvasManager.Instance.ShowDilemma(myDilemma, _behaviorController);
            CanvasManager.Instance.OnDilemmaEnded += () => { ValidationAction(EReturnState.SUCCEEDED); };
            _behaviorController.GetNavMeshAgent().avoidancePriority = 50;
        };
        OnComputerReached?.Invoke(_behaviorController.transform.position, ShowDilemma);
        _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(SceneManager.instance.GetPcTransform()));
    }

    public override bool StopAction(EStopActionReason reason)
    {
        _behaviorController.ShowSpecialTextAboveHead("DONT CARE !");
        return false;
    }
}

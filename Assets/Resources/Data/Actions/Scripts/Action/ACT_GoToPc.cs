using System;
using UnityEngine;

public class ACT_GoToPc : ActionBase
{
    public static event Action<Vector3, Action> OnComputerReached;
    public override void ExecuteAction()
    {
        _behaviorController.MoveToPosition(SceneManager.instance.GetPcTransform().position, "Walk");
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        var myDilema = DilemaManager.instance.GetCurrentDilema();
        Action ShowDilemma = () =>
        {
            CanvasManager.Instance.ShowDilemma(myDilema);
            ValidationAction();
            CanvasManager.Instance.OnDilemmaEnded += () => { ValidationAction(); };
        };
        OnComputerReached?.Invoke(_behaviorController.transform.position, ShowDilemma);
    }

    public override bool StopAction()
    {
        _behaviorController.ShowSpecialTextAboveHead("DONT CARE !");
        return false;
    }
}

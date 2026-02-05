using UnityEngine;

public class ACT_GoToPc : ActionBase
{
    public override void ExecuteAction()
    {
        _behaviorController.MoveToPosition(SceneManager.instance.GetPcTransform().position, "Walk");
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();

        var myDilema = DilemmaManager.instance.GetCurrentDilema();
        CanvasManager.Instance.ShowDilemma(myDilema, _behaviorController);
        CanvasManager.Instance.OnDilemmaEnded += () => { ValidationAction(EReturnState.SUCCEEDED); };
    }

    public override bool StopAction()
    {
        _behaviorController.ShowSpecialTextAboveHead("DONT CARE !");
        return false;
    }
}

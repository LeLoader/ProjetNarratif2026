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
        var myDilema = DilemaManager.GetRandomDilema();
        CanvasManager.Instance.ShowDilemma(myDilema);
        ValidationAction();
    }

    public override bool StopAction()
    {
        _behaviorController.ShowSpecialTextAboveHead("DONT CARE !");
        return false;
    }
}

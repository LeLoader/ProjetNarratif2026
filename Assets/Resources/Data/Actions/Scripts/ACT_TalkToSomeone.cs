using UnityEngine;

public class ACT_TalkToSomone : ActionBase
{
    public override void ExecuteAction()
    {
        var targetBehavior = CharacterBuilderManager.Instance.GetRandomBehaviorController();
        _behaviorController.MoveToPosition(targetBehavior.gameObject.transform.position, "Walk");
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
    }
}

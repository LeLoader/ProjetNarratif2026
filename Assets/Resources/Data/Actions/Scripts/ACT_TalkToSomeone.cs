using UnityEngine;

public class ACT_TalkToSomeone : ActionBase
{
    public override void ExecuteAction()
    {
        var targetBehavior = CharacterBuilderManager.Instance.GetRandomBehaviorController();
        _behaviorController.FollowTarget(targetBehavior.gameObject.transform);
        base.ExecuteAction();
    }

    public override void OnActionDestinationReached()
    {
        ValidationAction();
    }
}

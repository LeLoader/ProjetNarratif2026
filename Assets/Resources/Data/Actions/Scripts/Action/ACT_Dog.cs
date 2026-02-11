using UnityEngine;

public class ACT_Dog : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        CharacterBuilderManager.Instance.BuildDog(_behaviorController);
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

using UnityEngine;

public class ACT_Dog : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        CharacterBuilderManager.Instance.BuildDog(_behaviorController);
        SoundManager.Instance.PlaySound("SFX_Dog");
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

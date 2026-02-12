using UnityEngine;

public class ACT_Music : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        SoundManager.Instance.PlaySound("MUS_Musique");
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

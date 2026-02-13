using UnityEngine;

public class ACT_Fart : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameManager.instance.StartFarting();

        ValidationAction(EReturnState.SUCCEEDED);
    }
}

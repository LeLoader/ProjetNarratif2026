using UnityEngine;

public class ACT_Thief : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

using UnityEngine;

public class ACTI_Thief : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

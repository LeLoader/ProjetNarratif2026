using UnityEngine;

public class ACTI_Flowers : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

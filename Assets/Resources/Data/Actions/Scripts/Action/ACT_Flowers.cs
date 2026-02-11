using UnityEngine;

public class ACT_Flowers : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

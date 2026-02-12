using UnityEngine;

public class ACTI_LoveInterest : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

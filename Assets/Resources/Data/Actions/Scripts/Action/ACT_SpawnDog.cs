using UnityEngine;

public class ACT_SpawnDog : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

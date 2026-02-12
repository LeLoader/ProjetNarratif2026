using UnityEngine;

public class ACT_Die : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        Timer.SetTimer(gameObject, 5).OnTimerElapsed += () =>
        {
            Destroy(gameObject);
        };
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

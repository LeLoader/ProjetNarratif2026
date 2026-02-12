using UnityEngine;

public class ACT_Die : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();

        Timer.SetTimer(gameObject, 5).OnTimerElapsed += () =>
        {
            if (ActionLogger.GetActionCount("ACT_Grave") >= 1)
            {
                Instantiate(PrefabStaticRef.so.GravePrefab, gameObject.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        };

        ValidationAction(EReturnState.SUCCEEDED);
    }
}

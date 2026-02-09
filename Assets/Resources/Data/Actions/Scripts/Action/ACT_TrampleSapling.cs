using UnityEngine;

public class ACT_TrampleSapling : ActionBase
{
    GameObject targetSapling;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameObject[] saplings = GameObject.FindGameObjectsWithTag("Sapling");
        if (saplings.Length >= 1)
        {
            targetSapling = saplings[0];
            _behaviorController.MoveToPosition(targetSapling.transform.position);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();

        Destroy(targetSapling);
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

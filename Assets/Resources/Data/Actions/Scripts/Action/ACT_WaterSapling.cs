using UnityEngine;

public class ACT_WaterSapling : ActionBase
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

        for (int i = 1; i <= 3; i++)
        {
            _behaviorController.AddAction(ActionDataDrop.GetActionByID("ACT_SpawnSapling"), i);
        }
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

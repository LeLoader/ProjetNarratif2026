using UnityEngine;

public class ACT_WaterSapling : ActionBase
{
    GameObject targetSapling;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameObject[] saplings = GameObject.FindGameObjectsWithTag("Sapling");
        GameObject nearestSapling = SceneManager.instance.GetNearestObjects(_behaviorController.gameObject, saplings);
        if (nearestSapling)
        {
            _behaviorController.MoveToPosition(nearestSapling.transform.position);
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

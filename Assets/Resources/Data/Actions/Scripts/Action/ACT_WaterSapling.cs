using UnityEngine;

public class ACT_WaterSapling : ActionBase
{
    GameObject targetSapling;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameObject[] saplings = GameObject.FindGameObjectsWithTag("Sapling");
        targetSapling = SceneManager.instance.GetNearestObjects(_behaviorController.gameObject, saplings);
        if (targetSapling)
        {
            _behaviorController.SetObject(Instantiate(PrefabStaticRef.so.wateringCanPrefab));
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

        targetSapling.GetComponent<Sapling>().Water();
            // @TODO Add to a list, to create new saplings each time characters are created

        Timer.SetTimer(gameObject, 3f).OnTimerElapsed += () =>
        {
            _behaviorController.SetObject(null);
            ValidationAction(EReturnState.SUCCEEDED);
        };
    }

    public override bool StopAction(EStopActionReason reason)
    {
        switch (reason)
        {
            case EStopActionReason.INTERACTION:
                return false;
            case EStopActionReason.DEATH:
                return false;

            default:
                return true;

        }
    }
}

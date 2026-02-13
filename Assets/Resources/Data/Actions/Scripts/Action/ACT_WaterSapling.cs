using System.Collections;
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
            _behaviorController.SetObject(Instantiate(PrefabStaticRef.so.wateringCanPrefab), "BNS_R_Arm_end");
            _behaviorController.MoveToPosition(targetSapling.transform.position * 0.99f);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();

        StartCoroutine(Water());
    }

    private IEnumerator Water()
    {
        Coroutine rot = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(targetSapling.transform));
        yield return rot;

        _behaviorController.CallTriggerAnimation("waterSapling");
        SoundManager.Instance.PlaySound("SFX_Watering");
        targetSapling.GetComponent<Sapling>().Water();

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

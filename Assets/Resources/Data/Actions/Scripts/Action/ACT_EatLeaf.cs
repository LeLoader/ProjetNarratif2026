using System.Collections;
using UnityEngine;

public class ACT_EatLeaf : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameObject target = SceneManager.instance.GetNearestObjects(_behaviorController.gameObject, GameObject.FindGameObjectsWithTag("Sapling"));
        _behaviorController.MoveToPosition(target.transform.position);
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        _behaviorController.CallTriggerAnimation("eatLeaf");
        StartCoroutine(EatLeaf());
    }

    private IEnumerator EatLeaf()
    {
        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && !_behaviorController.GetAnimator().IsInTransition(0))
        {
            yield return null;
        }
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

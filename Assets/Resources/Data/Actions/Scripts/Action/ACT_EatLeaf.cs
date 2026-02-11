using System.Collections;
using UnityEngine;

public class ACT_EatLeaf : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        GameObject target = SceneManager.instance.GetNearestObjects(_behaviorController.gameObject, GameObject.FindGameObjectsWithTag("Sapling"));
        _behaviorController.FollowTarget(target.transform);
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        _behaviorController.CallTriggerAnimation("EatLeaf");
        StartCoroutine(EatLeaf());
    }

    private IEnumerator EatLeaf()
    {
        if (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            ValidationAction(EReturnState.SUCCEEDED);
        } else
        {
            yield return null;
        }
    }
}

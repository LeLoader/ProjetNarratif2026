using System.Collections;
using UnityEngine;

public class ACTI_Thief : ActionBase
{
    BehaviorController target;
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
        StartCoroutine(Thief());
        target = _behaviorController.GetOtherBehavior();
    }

    private IEnumerator Thief()
    {
        Coroutine coroutine = _behaviorController.StartCoroutine(_behaviorController.RotateTowardsTarget(target.transform));
        yield return coroutine;
        if (_behaviorController.currentObject == null)
        {
            target.currentObject.transform.parent = null;
            _behaviorController.SetObject(target.currentObject);
            target.currentObject = null;
        }
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

using System.Collections;
using UnityEngine;

public class ACT_PickFlower : ActionBase
{
    public override void ExecuteAction()
    {
        if (_behaviorController.currentObject == null)
        {
            _behaviorController.CallTriggerAnimation("takeFlower");
            StartCoroutine(TakeFlower());
        }
    }

    private IEnumerator TakeFlower()
    {
        while (_behaviorController.GetAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            yield return null;
        }
        _behaviorController.SetObject(_behaviorController._pickedFlower.gameObject);
        _behaviorController._pickedFlower = null;
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

using UnityEngine;

public class ACTI_Kill : ActionBase
{
    public override void ExecuteAction()
    {
        Debug.Log("Killing");
        base.ExecuteAction();
        if (_behaviorController.metrics[EMetricType.VIOLENCE] != EMetricState.NEUTRAL)
        {
            _behaviorController.SetObject(Instantiate(PrefabStaticRef.so.animationPistolPrefab), "BNS_R_Arm_end");
            SoundManager.Instance.PlaySound("SFX_Get_Weapon");
            _behaviorController.CallTriggerAnimation("killPistol");
            _behaviorController.GetOtherBehavior().Die();
            _behaviorController.GetOtherBehavior().CallTriggerAnimation("diePistol");
            ValidationAction(EReturnState.SUCCEEDED);
        }

    }

}

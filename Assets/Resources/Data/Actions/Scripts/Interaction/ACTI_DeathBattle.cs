using UnityEngine;

public class ACTI_DeathBattle : ActionBase
{
    public bool ManagingFight;
    public override void ExecuteAction()
    {
        _behaviorController.SetObject(Instantiate(PrefabStaticRef.so.animationPistolPrefab), "BNS_R_Arm_end");
        _behaviorController.GetOtherBehavior().SetObject(Instantiate(PrefabStaticRef.so.animationPistolPrefab), "BNS_R_Arm_end");
        SoundManager.Instance.PlaySound("SFX_Get_Weapon");
        Debug.Log("DeathBattle");
        base.ExecuteAction();
        if (_behaviorController.GetOtherBehavior().gameObject.GetComponent<ACTI_DeathBattle>().ManagingFight)
        {
            return;
        } else
        {
            ManagingFight = true;
            if (Random.Range(0,2) == 0)
            {
                _behaviorController.GetOtherBehavior().CallTriggerAnimation("killPistol");
                _behaviorController.CallTriggerAnimation("diePistol");
                _behaviorController.Die();
            } else
            {
                _behaviorController.CallTriggerAnimation("killPistol");
                _behaviorController.GetOtherBehavior().CallTriggerAnimation("diePistol");
                _behaviorController.GetOtherBehavior().Die();
            }
        }
        ValidationAction(EReturnState.SUCCEEDED);
    }
}
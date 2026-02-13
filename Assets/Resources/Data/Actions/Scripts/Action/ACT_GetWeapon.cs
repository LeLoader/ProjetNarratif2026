using UnityEngine;

public class ACT_GetWeapon : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        foreach (BehaviorController controller in CharacterBuilderManager.Instance.GetCharacters())
        {
            if (controller.metrics[EMetricType.VIOLENCE] == EMetricState.NEGATIVE)
            {
                controller.SetObject(Instantiate(PrefabStaticRef.so.animationPistolPrefab), "BNS_R_Arm_end");
            }
        }
        SoundManager.Instance.PlaySound("SFX_Get_Weapon");
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

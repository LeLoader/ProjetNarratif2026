using UnityEngine;

public class ACT_GetWeapon : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        _behaviorController.SetObject(Instantiate(PrefabStaticRef.so.pistolPrefab));
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

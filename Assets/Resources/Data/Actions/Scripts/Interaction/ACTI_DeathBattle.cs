using UnityEngine;

public class ACTI_DeathBattle : ActionBase
{
    public bool ManagingFight;
    public override void ExecuteAction()
    {
        Debug.Log("DeathBattle");
        base.ExecuteAction();
        ValidationAction(EReturnState.SUCCEEDED);
        if (_behaviorController.GetOtherBehavior().gameObject.GetComponent<ACTI_DeathBattle>().ManagingFight)
        {
            return;
        } else
        {
            ManagingFight = true;
            if (Random.Range(0,2) == 0)
            {
                _behaviorController.Die();
            } else
            {
                _behaviorController.GetOtherBehavior().Die();
            }
        }
    }
}
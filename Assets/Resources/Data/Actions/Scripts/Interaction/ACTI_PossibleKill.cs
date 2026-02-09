using UnityEngine;

public class ACTI_PossibleKill : ActionBase
{
    public override void ExecuteAction()
    {
        Debug.Log("May Kill");
        base.ExecuteAction();
    }
}

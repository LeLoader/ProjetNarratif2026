using UnityEngine;

public class ACT_EndOfGame : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        Debug.LogWarning("ending game");
        CharacterBuilderManager.Instance.EndGame();
        ValidationAction(EReturnState.SUCCEEDED);
        
    }
}

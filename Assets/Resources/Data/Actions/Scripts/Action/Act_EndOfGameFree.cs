using UnityEngine;

public class ACT_EndOfGameFree : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        Debug.LogWarning("ending game");
        CharacterBuilderManager.Instance.EndGame();
        CanvasManager.Instance.FadeToBlack(false);
        ValidationAction(EReturnState.SUCCEEDED);
        
    }
}

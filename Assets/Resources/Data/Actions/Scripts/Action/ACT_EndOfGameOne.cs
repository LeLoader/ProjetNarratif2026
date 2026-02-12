using UnityEngine;

public class ACT_EndOfGameOne : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        Debug.LogWarning("ending game");
        CharacterBuilderManager.Instance.EndGame();
        CanvasManager.Instance.FadeToBlack(true);
        ValidationAction(EReturnState.SUCCEEDED);
        
    }
}

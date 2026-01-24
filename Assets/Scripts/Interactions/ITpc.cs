using UnityEngine;

public class ITpc : InteractionBase
{
    public override void ExecuteInteraction(BehaviorController bh)
    {
        bh.StopAi();
        bh.StopHumanAnimation();
        base.ExecuteInteraction(bh);
    }
}

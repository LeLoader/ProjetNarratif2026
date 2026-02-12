using UnityEngine;

[CreateAssetMenu(fileName = "SomeoneHasDied", menuName = "Scriptable Objects/SpecialConditions/SomeoneHasDied")]
public class SomeoneHasDied : SpecialCondition
{
    override public bool IsConditionReached()
    {
        return ActionLogger.GetActionCount("ACT_Death") >= 1;
    }
}

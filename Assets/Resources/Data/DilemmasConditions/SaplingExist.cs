using UnityEngine;

[CreateAssetMenu(fileName = "SaplingExist", menuName = "Scriptable Objects/SpecialConditions/SaplingExist")]
public class SaplingExist : SpecialCondition
{
    override public bool IsConditionReached()
    {
        return GameObject.FindGameObjectsWithTag("Sapling").Length >= 1;
    }
}

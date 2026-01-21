using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DIL_000", menuName = "ScriptableObjects/Dilema")]
public class SODilema : ScriptableObject
{
    [SerializeField] LocalizedString question;
    public List<Condition> AppearanceConditions { get; }
    [SerializeField] Choice firstChoice;
    [SerializeField] Choice secondChoice;
    [SerializeField] bool bTimed = false;
    [SerializeField] Choice timeChoice;
    [SerializeField] bool bOneTime = false;

    public bool IsDilemaAvalaible()
    {
        foreach (Condition condition in AppearanceConditions)
        {
            if (!condition.IsConditionReached()) return false;
        }
        return true;
    }
}

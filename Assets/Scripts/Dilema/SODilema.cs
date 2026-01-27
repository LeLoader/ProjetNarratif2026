using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DIL_000", menuName = "ScriptableObjects/Dilema")]
public class SODilema : ScriptableObject
{
    [SerializeField] public LocalizedString question;
    [SerializeField] public List<Condition> appearanceConditions = new();
    [SerializeField] public Choice firstChoice;
    [SerializeField] public Choice secondChoice;
    [SerializeField] List<SODilema> NewDilemas;
    [SerializeField] bool bTimed = false;
    [SerializeField] Choice timeChoice;
    [SerializeField] bool bOneTime = false;

    public bool IsDilemaAvalaible()
    {
        foreach (Condition condition in appearanceConditions)
        {
            if (!condition.IsConditionReached()) return false;
        }
        return true;
    }
    
    public string GetQuestionText()
    {
        return question.GetLocalizedString();
    }

    public void Choose(Choice choice)
    {
        if (bOneTime)
        {
            DilemaManager.dilemaDatabase.RemoveDilema(this);
        }

        DilemaManager.dilemaDatabase.AddDilema(NewDilemas);
        choice.Activate();
    }
}

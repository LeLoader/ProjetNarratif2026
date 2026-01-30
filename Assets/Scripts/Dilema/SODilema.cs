using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DIL_000", menuName = "ScriptableObjects/Dilema")]
public class SODilema : ScriptableObject
{
    [SerializeField, ReadOnly] public string key;
    [SerializeField] public bool bRepeatable = false;
    [SerializeField] public LocalizedString question;
    [SerializeField] public List<Condition> appearanceConditions = new();
    [SerializeField] public List<SODilema> newDilemas = new();
    [SerializeField] public int npcToSpawn = 0;
    [SerializeField] public Choice firstChoice;
    [SerializeField] public Choice secondChoice;

  
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
        if (!bRepeatable)
        {
            DilemaManager.instance.RemoveDilemaInPool(this);
        }

        DilemaManager.instance.AddDilemaInPool(newDilemas);
        choice.Activate();
        CharacterBuilderManager.Instance.BuildCharacters(npcToSpawn);
    }
}

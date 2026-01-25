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
    [SerializeField] public Choice firstChoice;
    [SerializeField] public Choice secondChoice;
    [SerializeField] public List<SODilema> newDilemas;
  
    public bool IsDilemaAvalaible()
    {
        foreach (Condition condition in appearanceConditions)
        {
            if (!condition.IsConditionReached()) return false;
        }
        return true;
    }

    public void Chose(Choice choice)
    {
        if (!bRepeatable)
        {
            DilemaManager.dilemaDatabase.RemoveDilema(this);
        }

        DilemaManager.dilemaDatabase.AddDilemaInPool(newDilemas);
        choice.Activate();
    }
}

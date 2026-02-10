using EditorAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DIL_000", menuName = "Scriptable Objects/Dilemma")]
public class SODilemma : ScriptableObject
{
    [SerializeField, ReadOnly] public string key;
    [SerializeField] public bool bRepeatable = false;
    [SerializeField] public LocalizedString question;
    [SerializeField] public List<SpecialCondition> appearanceSpecialConditions = new();
    [SerializeField] public List<Condition> appearanceConditions = new();
    [SerializeField] public List<SODilemma> newDilemas = new();
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

    public void Choose(Choice choice, BehaviorController controller)
    {
        if (!bRepeatable)
        {
            DilemmaManager.instance.RemoveDilemaInPool(this);
        }

        DilemmaManager.instance.AddDilemaInPool(newDilemas);
        choice.Activate(controller);
        CharacterBuilderManager.Instance.BuildCharacters(npcToSpawn);
    }
}

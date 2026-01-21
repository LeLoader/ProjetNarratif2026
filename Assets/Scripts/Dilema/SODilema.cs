using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DIL_000", menuName = "ScriptableObjects/Dilema")]
public class SODilema : ScriptableObject
{
    [SerializeField] LocalizedString question;
    List<Condition> AppearanceConditions;
    [SerializeField] Choice firstChoice;
    [SerializeField] Choice secondChoice;
    [SerializeField] bool bTimed = false;
    [SerializeField] Choice timeChoice;
    [SerializeField] bool bOneTime = false;
}

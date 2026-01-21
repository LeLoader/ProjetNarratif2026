using UnityEngine;

public class SOActions : ScriptableObject
{
    [Tooltip("Name of the action to display in the UI")]
    [SerializeField] private string _actionName;
    
    [Tooltip("Key used to identify the action in the system and found it in folder")]
    [SerializeField] private string _actionKey;
}

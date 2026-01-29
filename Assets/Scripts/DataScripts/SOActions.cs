using UnityEngine;

[CreateAssetMenu(fileName = "SOActions", menuName = "Scriptable Objects/SOActions")]
public class SOActions : ScriptableObject
{
    [Tooltip("Name of the action to display in the UI")]
    public string _actionName;
    
    [Tooltip("Key used to identify the action in the system and found it in folder")]
    public string _actionKey;
    
    public bool _canBeRepeated = true;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSound", menuName = "Scriptable Objects/SOSound")]
public class SOSound : ScriptableObject
{
    private Dictionary<string, Sounds> AllSounds = new Dictionary<string, Sounds>();

}

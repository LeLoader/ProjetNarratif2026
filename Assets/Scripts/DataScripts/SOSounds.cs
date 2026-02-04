using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSound", menuName = "Scriptable Objects/SOSound")]
public class SOSounds : ScriptableObject
{
    [SerializeField] public string DebugText;

    [SerializeField] private List<Sound> ListSound = new List<Sound>();


    public void Add(Sound inSound)
    {
        if (inSound.IsNull())
        {
            Debug.LogError("[SO SOUNDS] sound not loaded");
        }
        else
        {
            ListSound.Add(inSound);
            Debug.Log($"[SO SOUNDS] new entry added : key is {inSound.GetClip().name}");
        }
    }

    public void ShowAllValues()
    {
        Debug.Log($"[SO SOUNDS] number of sounds is {ListSound.Count}");
        foreach (var item in ListSound)
        {
            Debug.Log($"[SO SOUND] Sound is {item.GetClip().name}");
        }
    }

    public void Reset()
    {
        ListSound.Clear();
    }

    public List<Sound> GetSounds()
    {
        return ListSound;
    }
}

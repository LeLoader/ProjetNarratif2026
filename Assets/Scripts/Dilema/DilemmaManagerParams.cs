using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "DilemmaManagerParams", menuName = "Scriptable Objects/Dilemma Manager Params")]
public class DilemmaManagerParams : ScriptableObject
{
    [SerializedDictionary("At what dilemma count should play", "this dilemma")] public SerializedDictionary<int, SODilema> specialsDilemmas;

    public bool FindDilemmaForCount(int count, out SODilema dilemma)
    {
        return specialsDilemmas.TryGetValue(count, out dilemma);
    }

    public int GetLastCount()
    {
        return specialsDilemmas.Last().Key;
    }
}

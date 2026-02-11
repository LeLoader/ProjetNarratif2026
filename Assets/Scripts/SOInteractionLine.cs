using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInteractionLine", menuName = "Scriptable Objects/SOInteractionLine")]
public class SOInteractionLine : ScriptableObject
{
    [SerializeField] private SerializedDictionary<MetricsWrapper, SOActions> Line = new SerializedDictionary<MetricsWrapper, SOActions>();

    public SOActions GetDictionary(MetricsWrapper metrics)
    {
        return Line[metrics];
    }

}

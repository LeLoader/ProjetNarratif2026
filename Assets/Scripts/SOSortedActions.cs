using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSortedActions", menuName = "Scriptable Objects/SOSortedActions")]
public class SOSortedActions : ScriptableObject
{
    [SerializeField] private SerializedDictionary<MetricsWrapper, SOInteractionLine> AllAvailableActions = new SerializedDictionary<MetricsWrapper, SOInteractionLine>();
    
    public SOInteractionLine GetInteractionLine(MetricsWrapper metrics)
    {
        return AllAvailableActions[metrics];
    }
}


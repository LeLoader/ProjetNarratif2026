using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SOSortedActions", menuName = "Scriptable Objects/SOSortedActions")]
public class SOSortedActions : ScriptableObject
{
    [SerializeField] private SerializedDictionary<MetricsWrapper, SOInteractionLine> AllAvailableActions = new SerializedDictionary<MetricsWrapper, SOInteractionLine>();

    [SerializeField] private List<SOActions> DefaultActions = new List<SOActions>();

    [SerializeField] public SOActions Thief;


    public SOInteractionLine GetInteractionLine(MetricsWrapper metrics)
    {
        return AllAvailableActions[metrics];
    }

    public SOActions GetDefaultAction()
    {
        List<SOActions> temp = DefaultActions.FindAll((x) => x.IsAllowed());
        return temp[Random.Range(0, temp.Count)];
    }

    public SOActions PotentialThief()
    {
        if (Thief.IsAllowed())
        {
            return Thief;
        } else
        {
            return GetDefaultAction();
        }
    }
}


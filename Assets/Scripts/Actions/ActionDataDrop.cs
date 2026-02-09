using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ActionDataDrop
{
    private static List<SOActions> actions = new List<SOActions>();

    private static SOSortedActions _actions = Resources.Load<SOSortedActions>("SOSortedActions");

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterAllDatas()
    {
        actions = Resources.LoadAll<SOActions>("Data/Actions/SO").ToList();
    }
    
    public static SOActions GetActionByID(string key)
    {
        return actions.Find(action => action._actionKey == key);
    }
    
    public static SOActions GetActionGoToPc()
    {
        return actions.Find(action => action._actionKey == "ACT_GoToPc");
    }
    
    public static SOActions GetActionRoam()
    {
        return actions.Find(action => action._actionKey == "ACT_Roam");
    }
    
    public static SOActions GetBasicGreetActions()
    {
        return actions.Find(action => action._actionKey == "ACTI_Greet");
    }

    public static SOActions GetKillAction()
    {
        return actions.Find(action => action._actionKey == "ACTI_Kill");
    }

    public static SOActions GetActionAvailable(Dictionary<EMetricType, EMetricState> FirstMetrics, Dictionary<EMetricType, EMetricState> SecondMetrics)
    {
        MetricsWrapper FirstWrapper = new MetricsWrapper(FirstMetrics[EMetricType.INDOCTRINATED], FirstMetrics[EMetricType.VIOLENCE]);
        MetricsWrapper SecondWrapper = new MetricsWrapper(SecondMetrics[EMetricType.INDOCTRINATED], SecondMetrics[EMetricType.VIOLENCE]);
        return _actions.GetInteractionLine(FirstWrapper).GetDictionary(SecondWrapper);
    }
}

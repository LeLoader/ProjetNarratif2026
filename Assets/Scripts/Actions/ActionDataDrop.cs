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

    public static SOActions GetActionAvailable(BehaviorController FirstBehavior, BehaviorController SecondBehavior)
    {
        SOActions ActionFound = _actions.GetInteractionLine(FirstBehavior.GetMetricsWrapper()).GetDictionary(SecondBehavior.GetMetricsWrapper());
        if (ActionFound == null)
        {
            if ((FirstBehavior.currentObject == null && SecondBehavior.currentObject == null) || (FirstBehavior.currentObject != null && SecondBehavior.currentObject != null))
            {
                return _actions.GetDefaultAction();
            } else
            {
                return _actions.PotentialThief();
            }
        } else
        {
            return ActionFound;
        }
    }
}

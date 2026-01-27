using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ActionDataDrop
{
    private static List<SOActions> actions = new List<SOActions>();
    
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
}

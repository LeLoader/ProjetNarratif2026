using System.Collections.Generic;
using UnityEngine;

public static class ActionLogger
{
    private static Dictionary<string, int> actionsLog; 
    
    public static void LogAction(string key)
    {
        if (actionsLog.ContainsKey(key))
        {
            actionsLog[key]++;
        }
        else
        {
            actionsLog[key] = 1;
        }
    }

    public static int GetActionCount(string key)
    {
        if (actionsLog.ContainsKey(key))
        {
            return actionsLog[key];
        }
        else
        {
            return 0;
        }
    }
}

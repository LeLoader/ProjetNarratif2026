using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DilemaManager
{
    public readonly static DBDilema dilemaDatabase = (DBDilema)Resources.Load("Databases/DBDilema");
    public readonly static SOGlobalMetrics globalMetrics = (SOGlobalMetrics)Resources.Load("SOGlobalMetrics");

    public static List<SODilema> GetAllDilemas()
    {
        return dilemaDatabase.dilemas;
    }

    public static List<SODilema> GetAllAvalaibleDilemas()
    {
        return dilemaDatabase.GetAllAvalaibleDilemas();
    }

    public static SODilema GetRandomDilema()
    {
        foreach (var dilema in dilemaDatabase.dilemas)
        {
            if (dilema == null) continue;
            Debug.Log("Dilema: " + dilema.key + " Avalaible: " + dilema.IsDilemaAvalaible());
        }
        return dilemaDatabase.GetRandomDilema(); 
    }

    public static SODilema GetDilema(string key)
    {
        return dilemaDatabase.GetDilema(key);
    }
}

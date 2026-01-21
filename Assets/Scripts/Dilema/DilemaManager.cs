using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class DilemaManager
{
    [field: SerializeField] public readonly static DBDilema dilemaDatabase = (DBDilema)Resources.Load("Databases/DBDilema");

    public static List<SODilema> GetAllDilemas()
    {
        return dilemaDatabase.Dilemas;
    }

    public static List<SODilema> GetAllAvalaibleDilemas()
    {
        // GameManager.SOGlobalMetrics.Metrics
        // dilemaDatabase.Dilemas.FindAll((dilema) => dilema.)
        return dilemaDatabase.Dilemas;
    }
}

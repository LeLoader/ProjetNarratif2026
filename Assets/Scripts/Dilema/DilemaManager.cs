using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class DilemaManager
{
    public readonly static DBDilema dilemaDatabase = (DBDilema)Resources.Load("Databases/DBDilema");

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
        return dilemaDatabase.GetRandomDilema(); 
    }

    public static SODilema GetDilema(string key)
    {
        return dilemaDatabase.GetDilema(key);
    }
}

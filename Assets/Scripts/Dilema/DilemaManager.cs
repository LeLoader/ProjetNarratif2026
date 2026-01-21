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
}

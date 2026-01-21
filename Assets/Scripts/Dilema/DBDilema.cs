using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBDilema", menuName = "DataTables/Dilemas Data Table")]
public class DBDilema : ScriptableObject
{
    public List<SODilema> dilemas;

    public List<SODilema> GetAllAvalaibleDilemas()
    {
        return dilemas.FindAll((dilema) => dilema.IsDilemaAvalaible());
    }
}

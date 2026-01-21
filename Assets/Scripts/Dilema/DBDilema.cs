using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBDilema", menuName = "DataTables/Dilemas Data Table")]
public class DBDilema : ScriptableObject
{
    [field: SerializeField] public List<SODilema> Dilemas { get; }
}

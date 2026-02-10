using System;
using UnityEngine;

public class ACT_GetCandle : ACT_GetObjectBase
{
    protected override GameObject Prefab { get => PrefabStaticRef.so.candlePrefab; }
}

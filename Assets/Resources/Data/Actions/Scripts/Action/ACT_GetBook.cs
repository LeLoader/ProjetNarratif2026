using System;
using UnityEngine;

public class ACT_GetBook : ACT_GetObjectBase
{
    protected override GameObject Prefab { get => PrefabStaticRef.so.bookPrefab; }
}

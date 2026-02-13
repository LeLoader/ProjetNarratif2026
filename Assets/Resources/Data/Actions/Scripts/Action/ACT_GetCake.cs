using System;
using UnityEngine;

public class ACT_GetCake : ACT_GetObjectBase
{
    protected override GameObject Prefab { get => PrefabStaticRef.so.cakePrefab; }

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        SoundManager.Instance.PlaySound("SFX_Birthday");    
    }
}

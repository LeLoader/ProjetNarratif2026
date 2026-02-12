using UnityEngine;

public class ACT_SpawnLake : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(20f, 40f, out Vector3 LakePosition))
        {
            GameObject prefab = PrefabStaticRef.so.lakePrefab;
            GameObject Lake = Instantiate(prefab, LakePosition, prefab.transform.rotation);
            ValidationAction(EReturnState.SUCCEEDED);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }
}

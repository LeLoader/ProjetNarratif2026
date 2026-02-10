using UnityEngine;

public class ACT_SpawnSapling : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(20f, 30f, out Vector3 saplingPosition))
        {
            GameObject prefab = PrefabStaticRef.so.saplingPrefab;
            GameObject sapling = Instantiate(prefab, saplingPosition, prefab.transform.rotation); //@TODO Random rotation?
            //@TODO Camera movement toward plant
            ValidationAction(EReturnState.SUCCEEDED);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }
}

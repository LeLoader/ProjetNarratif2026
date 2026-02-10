using UnityEngine;

public class ACT_SpawnSapling : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(20f, 30f, out Vector3 saplingPosition))
        {
            GameObject prefab = PrefabStaticRef.so.saplingPrefab;
            Vector3 RandomRotation = new Vector3(0, Random.Range(0, 359), 0);
            GameObject sapling = Instantiate(prefab, saplingPosition, Quaternion.Euler(RandomRotation));
            //@TODO Camera movement toward plant
            ValidationAction(EReturnState.SUCCEEDED);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }
}

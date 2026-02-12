using UnityEngine;

public class ACT_Flowers : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        for (int i = 0; i < 10; i++)
        {
            if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(10f, 40f, out Vector3 spawnPos))
            {
                Transform spawnTransform = PrefabStaticRef.so.flowerPrefab.transform;
                spawnTransform.position = spawnPos;
                Instantiate(PrefabStaticRef.so.flowerPrefab, spawnTransform);
            }
        }
        ValidationAction(EReturnState.SUCCEEDED);
    }
}

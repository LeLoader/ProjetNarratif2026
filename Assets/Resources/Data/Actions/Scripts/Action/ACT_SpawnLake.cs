using System.Collections;
using UnityEngine;

public class ACT_SpawnLake : ActionBase
{
    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(20f, 40f, out Vector3 LakePosition))
        {
            _behaviorController.MoveToPosition(LakePosition);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();
        _behaviorController.CallTriggerAnimation("dig");
        StartCoroutine(DigLake());
    }

    private IEnumerator DigLake()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2f);
        GameObject prefab = PrefabStaticRef.so.lakePrefab;
        Vector3 LakePosition = gameObject.transform.position;
        LakePosition.y = -3;
        GameObject Lake = Instantiate(prefab, LakePosition, prefab.transform.rotation);
        ValidationAction(EReturnState.SUCCEEDED);

    }
}

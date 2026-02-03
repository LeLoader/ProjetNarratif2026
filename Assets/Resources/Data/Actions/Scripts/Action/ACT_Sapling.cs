using UnityEngine;

public class ACT_Sapling : ActionBase
{
    [SerializeField] GameObject saplingPrefab;

    public override void ExecuteAction()
    {
        base.ExecuteAction();
        if (SceneManager.instance.GetRandomPointInNavMesh(out Vector3 saplingPosition))
        {
            GameObject sapling = Instantiate(saplingPrefab, saplingPosition, Quaternion.identity); //@TODO Random rotation?
            ValidationAction(EReturnState.SUCCEEDED);
        }
        else
        {
            ValidationAction(EReturnState.FAILED);
        }
    }
}

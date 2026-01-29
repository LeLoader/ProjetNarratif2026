using System.Collections;
using UnityEngine;

public class ACTI_Greet : ActionBase
{
    public override void ExecuteAction()
    {
        StartCoroutine(WaitForActionEnd());
        base.ExecuteAction();
    }
    
    private IEnumerator WaitForActionEnd()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        _behaviorController.ShowSpecialTextAboveHead("Hey there !");
        yield return new WaitForSeconds(Random.Range(0.7f, 1.5f));
        ValidationAction();
    }
}

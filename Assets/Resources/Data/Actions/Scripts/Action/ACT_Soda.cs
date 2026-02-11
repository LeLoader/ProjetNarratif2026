using UnityEngine;
using System;

public class ACT_Soda : ActionBase
{
    private GameObject soda;

    public override void ExecuteAction()
    {
        base.ExecuteAction();

        ITpc pc = FindAnyObjectByType<ITpc>();
        pc.SpawnObject(Instantiate(PrefabStaticRef.so.canPrefab));
        pc.OnAnimationEnded += OnSodaReadyToPickup;
    }

    private void OnSodaReadyToPickup(GameObject gameObject)
    {
        soda = gameObject;
        _behaviorController.MoveToPosition(soda.transform.position);

        FindAnyObjectByType<ITpc>().OnAnimationEnded -= OnSodaReadyToPickup;
    }

    public override void OnActionDestinationReached()
    {
        base.OnActionDestinationReached();

        // @TODO grab animation, sound

        _behaviorController.SetObject(soda);

        // Destroy the soda after 30sec
        Timer.SetTimer(gameObject, 30, true).OnTimerElapsed += () => { _behaviorController.SetObject(null); };
    }
}

using Newtonsoft.Json.Bson;
using System;
using UnityEngine;

public class ITpc : InteractionBase
{
    [SerializeField] public GameObject spawnSlot;
    [SerializeField] public Animation moveAnimation;

    public Action<GameObject> OnAnimationEnded;

    private bool checkForAnimation;

    public override void ExecuteInteraction(BehaviorController bh)
    {
       // bh.StopAi();
       // bh.StopHumanAnimation();
        base.ExecuteInteraction(bh);
    }

    public void SpawnObject(GameObject objectToSpawn)
    {
        objectToSpawn.transform.SetParent(spawnSlot.transform, false);
        StartAnimation();
    }

    private void StartAnimation()
    {
        moveAnimation.Play();
        checkForAnimation = true;
    }

    private void Update()
    {
        if (checkForAnimation && !moveAnimation.isPlaying)
        {
            EndAnimation();
        }
    }

    private void EndAnimation()
    {
        checkForAnimation = false;
        OnAnimationEnded?.Invoke(spawnSlot.transform.GetChild(0).gameObject);
    }
}

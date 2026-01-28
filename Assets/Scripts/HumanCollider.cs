using UnityEngine;

public class HumanCollider : InteractionBase
{
    [SerializeField] private BehaviorController behaviorController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractionBase>(out var ib))
        {
            // THE OTHER //
            ib.OnContactWithOtherBehaviour(behaviorController);
        }
    }

    public override void OnContactWithOtherBehaviour(BehaviorController otherBehaviour)
    {
        behaviorController.ContactOntoOtherHuman(otherBehaviour);
    }
}

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
        } else if (other.TryGetComponent<Flower>(out Flower flower))
        {
            behaviorController._pickedFlower = flower;
            behaviorController.AddAction(ActionDataDrop.GetActionByID("ACT_PickFlower"), 0);
        }
    }
    
    public override void OnContactWithOtherBehaviour(BehaviorController otherBehaviour)
    {
        behaviorController.ContactOntoOtherHuman(otherBehaviour);
    }
}

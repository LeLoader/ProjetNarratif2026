using UnityEngine;

public class HumanCollider : MonoBehaviour
{
    [SerializeField] private BehaviorController behaviorController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractionBase>(out var ib))
        {
            ib.ExecuteInteraction(behaviorController);
        }
    }
}

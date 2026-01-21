using System;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorController : MonoBehaviour
{
    
    [Header("EXPOSED VARIABLE")]
    [SerializeField] private NavMeshAgent agentComponent;
    
    #region Human States
    
    [SerializeField] private HumanState _currentState = HumanState.Roaming;

    public HumanState GetCurrentState()
    {
        return _currentState;
    }

    public void SetCurrentState(HumanState newState)
    {
        _currentState = newState;
    }

    private void TransitionToState(HumanState newState)
    {
        Debug.Log($"Transitioning from {_currentState} to {newState}");
        _currentState = newState;
    }
    
    #endregion

    #region Lyfe Cycle Methods

    private void Start()
    {
        UpdateStateActions();
    }

    #endregion

    #region AI Methods

    public void StopAi()
    {
        agentComponent.isStopped = true;
    }
    
    public void ResumeAi()
    {
        agentComponent.isStopped = false;
    }

    #endregion

    #region Animation

    [SerializeField] private Animator animator;

    public void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }
    
    public void CallTriggerAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    
    
    public void StartHumanAnimation()
    {
        CallTriggerAnimation("Walk");
    }
    public void StopHumanAnimation()
    {
        CallTriggerAnimation("Stop");
    }

    #endregion

    private void UpdateStateActions()
    {
        switch (_currentState)
        {
            case HumanState.Questioning:
                
                agentComponent.SetDestination(SceneManager.instance.GetPcTransform().position);
                StartHumanAnimation();
                
                break;
            case HumanState.Answering:
                break;
            case HumanState.Roaming:
                break;
            case HumanState.DoingTask:
                break;
        }
    }
}

public enum HumanState
{
    Questioning,
    Answering,
    Roaming,
    DoingTask,
}

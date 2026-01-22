using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorController : MonoBehaviour
{
    // DILEM ACTUEL DE LA CHAINE //
    private SODilema currentDilema;
    
    private bool _inAction = false;
    // LISTE D'ACTIONS DISPONIBLES A EFFECTUER A LA CHAINE //
    private List<SOActions> availableActions = new List<SOActions>();
    
    // LIST D'INTERACTIONS POSSIBLES //
    private List<SOInteraction> availableInteractions = new List<SOInteraction>(); 
    
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

    public void Intialize(SODilema newDilema)
    {
        currentDilema = newDilema;
    }
    
    private void CheckActions()
    {
        if (_inAction)
            return;
        
        _inAction = true;
        
    }

    #endregion

    #region AI Methods

    public void MoveToPosition(Vector3 targetPosition)
    {
        agentComponent.SetDestination(targetPosition);
    }
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
    
}

public enum HumanState
{
    Questioning,
    Answering,
    Roaming,
    DoingTask,
}

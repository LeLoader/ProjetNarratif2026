using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorController : MonoBehaviour
{
    // DILEM ACTUEL //
    private SODilema currentDilema;
    
    private bool _inAction = false;
    private bool _wasMoving = false;
    
    // LISTE D'ACTIONS DISPONIBLES A EFFECTUER A LA CHAINE //
    private List<SOActions> actionsToDo = new List<SOActions>();
    private SOActions _currentAction;
    private ActionBase _currentActionBase;
    
    // LIST D'INTERACTIONS POSSIBLES //
    private List<SOInteraction> availableInteractions = new List<SOInteraction>();
    
    [Header("EXPOSED VARIABLE")]
    [SerializeField] private NavMeshAgent agentComponent;
    
    private int _currentActionIndex = 0;

    #region Delegate
    
    public Action OnActionCompleted;
    public Action OnDestinationReached;
    public Action OnActionStarted;
    
    #endregion
    
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

    public void Initialize(SOActions newAction)
    {
        AddAction(newAction);
        CheckActions();
    }
    
    private void Update()
    {
        if (_currentActionBase != null && _inAction)
        {
            if(agentComponent.velocity.magnitude > 0.1f)
            {
                if (!_wasMoving)
                {
                    _wasMoving = true;
                }
            }
            else
            {
                if (_wasMoving)
                {
                    _wasMoving = false;
                    DestinationReached();
                }
            }
        }
    }

    #endregion

    #region AI Methods

    public void MoveToPosition(Vector3 targetPosition, string animationString = "")
    {
        agentComponent.SetDestination(targetPosition);
        if (!string.IsNullOrEmpty(animationString))
        {
            StartHumanAnimation();
        }
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
        Debug.Log("Starting Human Animation");
        CallTriggerAnimation("Walk");
    }
    
    public void StopHumanAnimation()
    {
        Debug.Log("Stopping Human Animation");
        CallTriggerAnimation("Stop");
    }

    #endregion

    #region ActionManagement

    public void AddAction(List<SOActions> actions)
    {
        actionsToDo.AddRange(actions);
        CheckActions();
    }
    public void AddAction(SOActions action)
    {
        actionsToDo.Add(action);
        CheckActions();
    }
    private void DestinationReached()
    {
        StopHumanAnimation();
        OnDestinationReached?.Invoke();
    }
    private void CheckActions()
    {
        if (_inAction)
        {
            return;
        }
        if (actionsToDo.Count == 0)
        {
            Debug.Log("No more actions to do.");
            return;
        }
        
        _inAction = true;
        
        DoAction(actionsToDo[0]);
    }
    private void DoAction(SOActions action)
    {
        if (action == null)
        {
            return;
        }
        
        OnActionStarted?.Invoke();
        _currentAction = action;

        _currentActionBase = ActionFactory.CreateAction(action._actionKey, this.gameObject);
        _currentActionIndex++;
        _currentActionBase.Initialize(this, _currentActionIndex);
    }
    public void ActionCompleted()
    {
        if(!_inAction){return;}
        
        OnActionCompleted?.Invoke();
        _inAction = false;

        Destroy(_currentActionBase);

        if (actionsToDo.Count != 1 && !actionsToDo[0]._canBeRepeated)
        {
            actionsToDo.RemoveAt(0);
        }

        StartCoroutine(WaitForNextAction());
    }

    #endregion

    public SODilema GetCurrentDilema()
    {
        return currentDilema;
    }
    
    public bool CanReachDestination(Vector3 targetDestination)
    {
        NavMeshPath path = new NavMeshPath();
        if (agentComponent.CalculatePath(targetDestination, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        Debug.Log("Cannot reach destination");
        return false;
    }

    private IEnumerator WaitForNextAction()
    {
        yield return new WaitForSeconds(1f);
        CheckActions();
    }

}

public enum HumanState
{
    Questioning,
    Answering,
    Roaming,
    DoingTask,
}

using EditorAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorController : MonoBehaviour
{
    // DILEM ACTUEL //
    private SODilemma currentDilema;
    
    private bool inAction = false;
    private bool interacting = false;
    private bool canInteract = true;
    
    // LISTE D'ACTIONS DISPONIBLES A EFFECTUER A LA CHAINE //
    [SerializeField, ReadOnly] private List<SOActions> actionsToDo = new List<SOActions>();
    private SOActions _currentAction;
    private ActionBase _currentActionBase;
    
    // LIST D'INTERACTIONS POSSIBLES //
    private List<SOInteraction> availableInteractions = new List<SOInteraction>();
    
    [Header("EXPOSED VARIABLE")]
    [SerializeField] private NavMeshAgent agentComponent;
    [SerializeField] private BoxCollider _interactionCollider;

    BehaviorController _otherHumanInteractingWith;
    private int _currentActionIndex = 0;
    
    private Transform _followTargetTransform;
    
    // UI //
    [Header("UI")]
    [SerializeField] private CanvasHumanController _canvasHumanController;
    

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
        _currentState = newState;
    }
    
    #endregion

    #region Lyfe Cycle Methods

    public void Initialize(SOActions newAction, string myName = "Human")
    {
        this.gameObject.name = myName;
        AddAction(newAction);
        CheckActions();
    }
    
    private void Update()
    {
        animator.SetBool("isMoving", agentComponent.velocity.magnitude > 0.1f);

        if (_currentActionBase != null && inAction && !_currentActionBase.bHasReachedDestination)
        {
            if(agentComponent.remainingDistance <= agentComponent.stoppingDistance)
            {
                _followTargetTransform = null;
                DestinationReached();
            }
        }
        
        if (_followTargetTransform != null)
        {
            agentComponent.SetDestination(_followTargetTransform.position);
        }
    }

    #endregion

    #region AI Meth

    public void FollowTarget(Transform targetTransform)
    {
        _followTargetTransform = targetTransform;
        StartHumanAnimation();
    }
    public void MoveToPosition(Vector3 targetPosition, string animationString = "")
    {
        agentComponent.SetDestination(targetPosition);
        if (!string.IsNullOrEmpty(animationString))
        {
            StartHumanAnimation();
        }
    }
    
    public void StopAiSpeed()
    {
        agentComponent.ResetPath();
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
    
    [Header("ANIMATION")]

    [SerializeField] private Animator animator;

    private void SpawnTextAboveHead(string text)
    {
        _canvasHumanController.ShowTextAboveHead(text);
    }

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
    public void AddAction(SOActions action, int index)
    {
        if (index == 0) return;
        actionsToDo.Insert(index, action);
    }
    private void DestinationReached()
    {
        StopHumanAnimation();
        OnDestinationReached?.Invoke();
    }
    private void CheckActions()
    {
        if (inAction)
        {
            return;
        }
        if (actionsToDo.Count == 0)
        {
            return;
        }
        
        inAction = true;
        
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

    private void DestroyCurrentAction()
    {
        Destroy(_currentActionBase);
      

        if (actionsToDo.Count != 1 && !actionsToDo[0]._canBeRepeated)
        {
            actionsToDo.RemoveAt(0);
        }
        else
        {
            actionsToDo.Add(actionsToDo[0]);
            actionsToDo.RemoveAt(0);
        }
    }
    
    private IEnumerator WaitForOtherHumanToEndAction()
    {
        Debug.Log(gameObject.name + " Waiting for other human to end action");
        yield return new WaitUntil(() => !_otherHumanInteractingWith.inAction);
        ActionCompleted();
    }
    public void ActionCompleted()
    {
        if (!inAction)
        {
            Debug.LogError("Trying to complete an action while not in action");
            return;
        }
        
        interacting = false;

        if (_currentAction._isAnInteraction)
        {
            if (_otherHumanInteractingWith.IsInteractingWithOtherHuman())
            {
                StartCoroutine(WaitForOtherHumanToEndAction());
                return;
            }
            StartCoroutine(CoolDownInteraction());
        }
        
        OnActionCompleted?.Invoke();
        
        
        inAction = false;
        
        DestroyCurrentAction();

        StartCoroutine(WaitForNextAction());
    }
    
    public bool IsInteractingWithOtherHuman()
    {
        return interacting;
    }
    

    #endregion
    
    


    public void SetDilemma(SODilemma dilema)
    {
        currentDilema = dilema;
    }

    public void ContactOntoOtherHuman(BehaviorController otherHuman)
    {
        if(!canInteract){return;}
        if(interacting){return;}
        
        _otherHumanInteractingWith = otherHuman;
        
        SetCanInteractionState(false);
        StopCurrentAction();
        
        StopAiSpeed();
        StartCoroutine(RotateTowardsTarget(otherHuman.transform));

        SpawnTextAboveHead("!");
    }
    
    private void SetCanInteractionState(bool state)
    {
        canInteract = state;
        _interactionCollider.enabled = state;
    }
    public SODilemma GetCurrentDilema()
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
        return false;
    }

    private IEnumerator WaitForNextAction()
    {
        yield return new WaitForSeconds(1f);
        CheckActions();
    }
    
    public void StopCurrentAction()
    {
        if (_currentActionBase != null)
        {
            if (!_currentActionBase.StopAction())
            {
                return;
            }
        }
        StartInteractionBeetweenHumans();
    }
    
    private void StartInteractionBeetweenHumans()
    {
        interacting = true;
        inAction = false;
                
        DestroyCurrentAction();

        StopHumanAnimation();
                
        actionsToDo.Insert(0, ActionDataDrop.GetBasicGreetActions());
        CheckActions();
    }
    
    private IEnumerator RotateTowardsTarget(Transform targetTransform)
    {
        Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
        
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alpha);
            yield return null;
        }
    }
    
    public void ShowSpecialTextAboveHead(string text)
    {
        _canvasHumanController.ShowTextAboveHead(text);
    }
    
    private IEnumerator CoolDownInteraction()
    {
        Debug.Log(gameObject.name + " Cooling down interaction");
        yield return new WaitForSeconds(3f);
        SetCanInteractionState(true);
    }


    #region Life

    public void Die()
    {
        Destroy(gameObject);
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

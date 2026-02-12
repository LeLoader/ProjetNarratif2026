using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBuilderManager : MonoBehaviour
{
    public static CharacterBuilderManager Instance;

    [SerializeField] private GameObject _humanPrefab;
    [SerializeField] private GameObject _dogPrefab;


    private List<BehaviorController> _characters = new List<BehaviorController>();
    public BehaviorController dog;

    [Header("Testing")]
    [SerializeField] private SOActions _testAction;

    public static event Action<int> OnCharactersCreationFinished;

    public List<BehaviorController> GetCharacters()
    {
        return _characters;
    }

    public BehaviorController GetRandomBehaviorController(bool onlyEmptyHand = false)
    {
        if (_characters.Count == 0) return null;

        if (onlyEmptyHand)
        {
            List<BehaviorController> _emptyHandCharacters = _characters.FindAll((x) => x.currentObject == null);
            return _emptyHandCharacters[UnityEngine.Random.Range(0, _emptyHandCharacters.Count)];
        }

        return _characters[UnityEngine.Random.Range(0, _characters.Count)];
    }

    public BehaviorController GetRandomBehaviorControllerNotInteracting(bool onlyEmptyHand = false)
    {
        if (_characters.Count == 0) return null;

        if (onlyEmptyHand)
        {
            List<BehaviorController> _emptyHandCharacters = _characters.FindAll((x) => x.currentObject == null && !x.IsInteracting());
            return _emptyHandCharacters[UnityEngine.Random.Range(0, _emptyHandCharacters.Count)];
        }

        List<BehaviorController> temp = _characters.FindAll((x) => !x.IsInteracting());
        return temp[UnityEngine.Random.Range(0, temp.Count)];
    }

    public void Start()
    {
        BuildCharacters();
    }

    [Button]
    public void BuildCharacterWithTestingAction()
    {
        BuildCharacters(1, _testAction);
    }

    private void BuildCharacter(SOActions startingAction)
    {
        var bc = Instantiate(_humanPrefab, SceneManager.instance.GetSpawnPoint(), Quaternion.identity).GetComponent<BehaviorController>();
        if (bc != null)
        {
            bc.Initialize(startingAction, (_characters.Count + 1).ToString("D3"));
            _characters.Add(bc);
        }
    }
    public void BuildDog(BehaviorController owner)
    {
        SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(owner.transform.position, 1f, 2f, out Vector3 spawnPoint);
        var bc = Instantiate(_dogPrefab, spawnPoint, Quaternion.identity).GetComponent<BehaviorController>();
        if (bc != null)
        {
            bc.Initialize(ActionDataDrop.GetActionRoam(), "DOG");
            bc.AddAction(ActionDataDrop.GetActionRoam());
            bc.OnDestinationReached += () =>
            {
                bc.FollowTarget(owner.transform);
                bc.AddAction(ActionDataDrop.GetActionRoam(), 0);
            };
            dog = bc;
        }
    }

    public void AssignADilemmaToRandomCharacter(SODilemma dilema)
    {
        var character = GetRandomBehaviorController();
        if (character != null)
        {
            character.SetDilemma(dilema);
        }
    }

    public void AssignAnActionToRandomCharacter(SOActions action = null, bool onlyEmptyHand = false)
    {
        var character = GetRandomBehaviorController(onlyEmptyHand);
        if (action == null) action = ActionDataDrop.GetActionRoam();
        if (character != null)
        {
            Debug.LogWarning($"Assigned {action} to {character}");
#if UNITY_EDITOR
            Selection.activeGameObject = character.gameObject;
            Selection.activeTransform = character.gameObject.transform;
            SceneView.lastActiveSceneView.FrameSelected();
#endif
            character.AddAction(action);
        }
    }

    public void BuildCharacters(int x = 1, SOActions action = null)
    {
        if (!action) action = ActionDataDrop.GetActionRoam();

        for (int i = 0; i < x; i++)
        {
            BuildCharacter(action);
        }

        OnCharactersCreationFinished?.Invoke(x);
    }

    [Button]
    public void BuildCharacterButton(int numToSpawn = 1)
    {
        BuildCharacters(numToSpawn);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame()
    {
        Debug.Log("[CHARACTER BUILDER] is endiing game");
        foreach (BehaviorController character in _characters)
        {
            if (SceneManager.instance.GetRandomPointInNavMeshInRadiusRange(0f, 10f, out Vector3 Position))
            {
                character.AddAction(ActionDataDrop.GetActionByID("ACT_GoToLocation"), 0);
            }
        }
    }
}

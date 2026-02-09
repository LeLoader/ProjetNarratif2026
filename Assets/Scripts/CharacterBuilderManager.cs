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
    private List<BehaviorController> _characters = new List<BehaviorController>();

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
            Selection.activeGameObject = character.gameObject;
            Selection.activeTransform = character.gameObject.transform;
            SceneView.lastActiveSceneView.FrameSelected();
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
}

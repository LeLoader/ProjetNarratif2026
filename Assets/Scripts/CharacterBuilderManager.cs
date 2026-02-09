using System;
using System.Collections.Generic;
using EditorAttributes;
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

    public BehaviorController GetRandomBehaviorController()
    {
        if (_characters.Count == 0) return null;

        int randomIndex = UnityEngine.Random.Range(0, _characters.Count);
        return _characters[randomIndex];
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
            bc.Initialize(startingAction, "Character_" + (_characters.Count + 1));
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

    public void AssignAnActionToRandomCharacter(SOActions action = null)
    {
        var character = GetRandomBehaviorController();
        if (action == null)
        {
            action = ActionDataDrop.GetActionRoam();
        }
        if (character != null)
        {
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

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
        BuildCharacter(ActionDataDrop.GetActionRoam());
    }

    [Button]
    public void BuildCharacterWithTestingAction()
    {
        BuildCharacter(_testAction);
    }
    public void BuildCharacter(SOActions startingAction)
    {
        var bc = Instantiate(_humanPrefab, SceneManager.instance.GetSpawnPoint(), Quaternion.identity).GetComponent<BehaviorController>();
        if (bc != null)
        {
            bc.Initialize(startingAction);
            _characters.Add(bc);
        }
    }

    public void BuildCharacters(int x)
    {
        for (int i = 0; i < x; i++)
        {
            BuildCharacter(ActionDataDrop.GetActionRoam());
        }
    }
    
    [Button]
    public void BuildCharacterButton(int numToSpawn)
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

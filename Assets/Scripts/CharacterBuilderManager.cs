using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBuilderManager : MonoBehaviour
{
    public static CharacterBuilderManager Instance;
    
    [SerializeField] private GameObject _humanPrefab;
    [SerializeField] private SOActions _startingAction;

    public void Start()
    {
        BuildCharacter(DilemaManager.GetRandomDilema());
    }

    public void BuildCharacter(SODilema dilema)
    {
        var bc = Instantiate(_humanPrefab, SceneManager.instance.GetSpawnPoint(), Quaternion.identity).GetComponent<BehaviorController>();
        if (bc != null)
        {
            bc.Initialize(dilema, _startingAction);
        }
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

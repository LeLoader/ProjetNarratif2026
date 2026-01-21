using System;
using EditorAttributes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;
    
    [Header("REFERENCE")]
    [SerializeField] private GameObject humanPrefab;

    public static SceneManager instance;


    #region MyRegion
    
    public Transform GetPcTransform()
    {
        return pcTransform;
    }

    #endregion
    private void Start()
    {
        SpawnHuman();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Button]
    private void SpawnHuman()
    {
        var bc = Instantiate(humanPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<BehaviorController>();
        if (bc != null)
        {
        }
    }
}

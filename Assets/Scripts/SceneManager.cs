using System;
using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SODilemma startDilema;
    [SerializeField] private SOActions startAction;
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;
    
    [SerializeField] private GameObject _debugRoamPointPrefab;

    [SerializeField] private float roamRadius = 10f;

    public static SceneManager instance;

    public GameObject SpawnDebugRoamPoint(Vector3 position)
    {
        if (_debugRoamPointPrefab) 
        { 
            return Instantiate(_debugRoamPointPrefab, position, Quaternion.identity);
        }
        else
        {
            return null;
        }
    }

    #region Get Positions
    
    public Vector3 GetRandomRoamPoint()
    {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * roamRadius;
        Vector3 roamPoint = new Vector3(pcTransform.position.x + randomPoint.x, pcTransform.position.y, pcTransform.position.z + randomPoint.y);
        return roamPoint;
    }
    
    public Transform GetPcTransform()
    {
        return pcTransform;
    }
    
    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }

    #endregion

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
}

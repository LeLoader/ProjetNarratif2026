using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SceneManager : MonoBehaviour
{
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;
    
    [SerializeField] private GameObject _debugRoamPointPrefab;

    [SerializeField] private float roamRadius = 10f;
    [SerializeField] private float maxRadius = 50f;

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


    /// <summary>
    /// Overload to find a point on the NavMesh around the pc
    /// </summary>
    /// <param name="maxAllowedTries">Max iteration count before returning</param>
    /// <returns></returns>
    public bool GetRandomPointInNavMesh(out Vector3 result, int maxAllowedTries = 10)
    {
        return GetRandomPointInNavMesh(pcTransform.position, out result, maxAllowedTries);
    }

    /// <summary>
    /// Overload to find a point on the NavMesh around a position
    /// </summary>
    /// <param name="center">Center of the query</param>
    /// <param name="maxAllowedTries">Max iteration count before returning</param>
    /// <returns></returns>
    public bool GetRandomPointInNavMesh(Vector3 center, out Vector3 result, int maxAllowedTries = 10)
    {
        for (int i = 0; i < maxAllowedTries; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle * roamRadius;
            Vector3 randomPosition = new(center.x + randomDirection.x, center.y, center.z + randomDirection.y);
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
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

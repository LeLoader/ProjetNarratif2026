using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneManager : MonoBehaviour
{
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;
    [SerializeField] private RectTransform mainCanvasTransform;
    
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


    public GameObject GetNearestObjects(GameObject from, GameObject[] objects)
    {
        GameObject toReturn = objects[0];
        for (int i = 1; i < objects.Length; i++) {
            GameObject gameObject = objects[i];
            if ((from.transform.position - gameObject.transform.position).magnitude > (from.transform.position - toReturn.transform.position).magnitude)
            {
                toReturn = gameObject;
            }
        }
        return toReturn;
    }

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
            //if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public bool GetRandomPointInNavMeshInRadius(float radius, out Vector3 result, int maxAllowedTries = 10)
    {
        return GetRandomPointInNavMeshInRadius(pcTransform.position, radius, out result, maxAllowedTries);
    }

    public bool GetRandomPointInNavMeshInRadius(Vector3 center, float radius, out Vector3 result, int maxAllowedTries = 10)
    {
        return GetRandomPointInNavMeshInRadiusRange(center, radius, radius, out result, maxAllowedTries);
    }

    public bool GetRandomPointInNavMeshInRadiusRange(float minRadius, float maxRadius, out Vector3 result, int maxAllowedTries = 10)
    {
        return GetRandomPointInNavMeshInRadiusRange(pcTransform.position, minRadius, maxRadius, out result, maxAllowedTries);
    }

    public bool GetRandomPointInNavMeshInRadiusRange(Vector3 center, float minRadius, float maxRadius, out Vector3 result, int maxAllowedTries = 10)
    {
        for (int i = 0; i < maxAllowedTries; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(minRadius, maxRadius);
            Vector3 randomPosition = new(center.x + randomDirection.x, center.y, center.z + randomDirection.y);
            //if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
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

    [SerializeField] LayerMask groundMask;

    public Vector3 GetSpawnPoint()
    {
        List<Vector3> positions = new();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                Vector3 vec = new((Screen.width / 2) + (Screen.width * i) + (1 * i), (Screen.height / 2) + (Screen.height * j) + (1 * j));
                if (Physics.Raycast(Camera.main.ScreenToWorldPoint(vec), Camera.main.transform.forward, out RaycastHit HitResult, Mathf.Infinity, groundMask))
                {
                    if (NavMesh.SamplePosition(HitResult.point, out NavMeshHit hit, 2, NavMesh.AllAreas))
                    {
                        positions.Add(hit.position);
                    }
                }  
            }
        }
        if (positions.Count == 0) return spawnPoint.position;

        return positions[UnityEngine.Random.Range(0, positions.Count)];
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

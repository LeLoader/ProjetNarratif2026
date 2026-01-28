using System;
using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SODilema startDilema;
    [SerializeField] private SOActions startAction;
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;
    
    [SerializeField] private GameObject _debugRoamPointPrefab;

    public static SceneManager instance;

    public GameObject SpawnDebugRoamPoint(Vector3 position)
    {
        return Instantiate(_debugRoamPointPrefab, position, Quaternion.identity);
    }

    #region Get Positions
    
    public Vector3 GetRandomRoamPoint()
    {
        float roamRadius = 10f;
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

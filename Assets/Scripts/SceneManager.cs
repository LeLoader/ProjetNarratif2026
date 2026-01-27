using System;
using EditorAttributes;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private SODilema startDilema;
    [SerializeField] private SOActions startAction;
    [Header("Object in Scene")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform pcTransform;

    public static SceneManager instance;


    #region Get Positions
    
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

using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DilemaManager : MonoBehaviour
{
    public static DilemaManager instance;

    [SerializeField] public DBDilema dilemaDatabase;
    [SerializeField] DilemmaManagerParams dilemaManagerParams;
    [SerializeField] SODilema ExtremePositiveDilema;
    [SerializeField] SODilema ExtremeNegativeDilema;

    private int dilemmaCount = 0;
    
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

        dilemaDatabase = (DBDilema)Resources.Load("Databases/DBDilema");
        dilemaDatabase.Init();
    }

    public SODilema GetCurrentDilema()
    {
        ++dilemmaCount;

        if (dilemaManagerParams.FindDilemmaForCount(dilemmaCount, out SODilema dilemma))
        {
            return dilemma;
        }

        return dilemaDatabase.GetRandomDilema(); 
    }

    public SODilema GetDilema(string key)
    {
        return dilemaDatabase.GetDilema(key);
    }

    public void OnMetricReachedExtreme(EMetricState state)
    {
        dilemaDatabase.ClearDilemaPool();
        if (state == EMetricState.POSITIVE)
        {
            dilemaDatabase.AddDilema(ExtremePositiveDilema);
        }
        else if (state == EMetricState.NEGATIVE)
        {
            dilemaDatabase.AddDilema(ExtremeNegativeDilema);
        }
    }
}

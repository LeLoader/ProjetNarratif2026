using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DilemaManager : MonoBehaviour
{
    public static DilemaManager instance;

    [SerializeField] public DBDilema dilemmaDatabase;
    [SerializeField] DilemmaManagerParams dilemaManagerParams;
    [SerializeField] SODilema ExtremePositiveDilema;
    [SerializeField] SODilema ExtremeNegativeDilema;

    [SerializeField, ReadOnly] List<SODilema> dilemmasPool = new();

    private int dilemmaCount = 0;

    private void Reset()
    {
        dilemmaDatabase = (DBDilema)Resources.Load("Databases/DBDilema");
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

        dilemmasPool.Clear();
    }

    public SODilema GetCurrentDilema()
    {
        ++dilemmaCount;

        if (dilemaManagerParams.FindDilemmaForCount(dilemmaCount, out SODilema dilemma))
        {
            return dilemma;
        }

        return GetRandomDilema();
    }

    public SODilema GetDilema(string key)
    {
        return dilemmaDatabase.GetDilema(key);
    }

    public void OnMetricReachedExtreme(EMetricState state)
    {
        ClearDilemaPool();
        if (state == EMetricState.POSITIVE)
        {
            dilemmaDatabase.AddDilema(ExtremePositiveDilema);
        }
        else if (state == EMetricState.NEGATIVE)
        {
            dilemmaDatabase.AddDilema(ExtremeNegativeDilema);
        }
    }

    public void AddDilemmaInPool(SODilema dilema)
    {
        if (dilema)
        {
            dilemmasPool.Add(dilema);
        }
        else
        {
            Debug.LogWarning("Tried to add dilema in pool, but it was null");
        }
    }

    public void AddDilemmaInPool(string key)
    {
        SODilema dilema = dilemmaDatabase.dilemmas.Find((dilema) => dilema.key == key);
        if (dilema)
        {
            dilemmasPool.Add(dilema);
        }
        else
        {
            Debug.LogWarning("Tried to add dilema in pool, but the key was invalid");
        }
    }

    public void AddDilemaInPool(List<SODilema> dilemas)
    {
        if (dilemas.Count > 0)
        {
            dilemmasPool.AddRange(dilemas);
        }
        else
        {
            Debug.LogWarning("Tried to add dilemas in pool, but the List was empty");
        }
    }

    public void RemoveDilemaInPool(SODilema dilema)
    {
        dilemmasPool.Remove(dilema);
    }

    public void ClearDilemaPool()
    {
        dilemmasPool.Clear();
    }

    public List<SODilema> GetAllAvalaibleDilemas()
    {
        Debug.Log("Dilemas in pool: " + dilemmasPool.Count);
        RemoveNullDilemasFromPool();
        return dilemmasPool.FindAll(d => d != null && d.IsDilemaAvalaible());
    }

    private void RemoveNullDilemasFromPool()
    {
        dilemmasPool.RemoveAll(d => d == null);
    }

    public SODilema GetRandomDilema()
    {
        List<SODilema> avalaibleDilemas = GetAllAvalaibleDilemas();
        if (avalaibleDilemas.Count == 0) return null;

        foreach (var dilema in avalaibleDilemas)
        {
            if (dilema == null) continue;
            Debug.Log("Dilema: " + dilema.key + " Avalaible: " + dilema.IsDilemaAvalaible());
        }

        return avalaibleDilemas[Random.Range(0, avalaibleDilemas.Count)];
    }
}

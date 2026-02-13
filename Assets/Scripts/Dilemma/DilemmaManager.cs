using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DilemmaManager : MonoBehaviour
{
    public static DilemmaManager instance;

    [SerializeField] public readonly DBDilemma dilemmaDatabase;
    [SerializeField] DilemmaManagerParams dilemmaManagerParams;
    [SerializeField] SODilemma ExtremePositiveDilemma;
    [SerializeField] SODilemma ExtremeNegativeDilemma;

    [SerializeField, ReadOnly] List<SODilemma> dilemmasPool = new();
    // @TODO Waiting pool to avoid duplicate

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

        dilemmasPool.Clear();
    }

    public SODilemma GetCurrentDilema()
    {
        ++dilemmaCount;

        if (dilemmaManagerParams.FindDilemmaForCount(dilemmaCount, out SODilemma dilemma))
        {
            return dilemma;
        }

        return GetRandomDilema();
    }

    public SODilemma GetDilema(string key)
    {
        return dilemmaDatabase.GetDilema(key);
    }

    public void OnMetricReachedExtreme(EMetricState state)
    {
        ClearDilemaPool();
        if (state == EMetricState.POSITIVE)
        {
            AddDilemmaInPool(ExtremePositiveDilemma);
        }
        else if (state == EMetricState.NEGATIVE)
        {
            AddDilemmaInPool(ExtremeNegativeDilemma);
        }
    }

    public void AddDilemmaInPool(SODilemma dilemma)
    {
        if (dilemma)
        {
            dilemmasPool.Add(dilemma);
        }
        else
        {
            Debug.LogWarning("Tried to add dilemma in pool, but it was null");
        }
    }

    public void AddDilemmaInPool(string key)
    {
        SODilemma dilemma = dilemmaDatabase.dilemmas.Find((dilemma) => dilemma.key == key);
        if (dilemma)
        {
            dilemmasPool.Add(dilemma);
        }
        else
        {
            Debug.LogWarning("Tried to add dilemma in pool, but the key was invalid");
        }
    }

    public void AddDilemaInPool(List<SODilemma> dilemmas)
    {
        if (dilemmas.Count > 0)
        {
            dilemmasPool.AddRange(dilemmas);
        }
        else
        {
            Debug.LogWarning("Tried to add dilemmas in pool, but the List was empty");
        }
    }

    public void RemoveDilemaInPool(SODilemma dilemma)
    {
        dilemmasPool.Remove(dilemma);
    }

    public void ClearDilemaPool()
    {
        dilemmasPool.Clear();
    }

    public List<SODilemma> GetAllAvalaibleDilemas()
    {
        Debug.Log("Dilemas in pool: " + dilemmasPool.Count);
        RemoveNullDilemasFromPool();
        return dilemmasPool.FindAll(d => d != null && d.IsDilemaAvalaible());
    }

    private void RemoveNullDilemasFromPool()
    {
        dilemmasPool.RemoveAll(d => d == null);
    }

    public SODilemma GetRandomDilema()
    {
        List<SODilemma> avalaibleDilemmas = GetAllAvalaibleDilemas();
        if (avalaibleDilemmas.Count == 0) return null;

        foreach (var dilemma in avalaibleDilemmas)
        {
            if (dilemma == null) continue;
            Debug.Log("Dilema: " + dilemma.key + " Avalaible: " + dilemma.IsDilemaAvalaible());
        }

        return avalaibleDilemmas[Random.Range(0, avalaibleDilemmas.Count)];
    }
}

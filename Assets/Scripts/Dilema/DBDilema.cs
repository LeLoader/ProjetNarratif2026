using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DBDilema", menuName = "DataTables/Dilemas Data Table")]
public class DBDilema : ScriptableObject
{
    [SerializeField] public List<SODilema> dilemas = new List<SODilema>();
    [SerializeField] public SODilema startDilema;
    List<SODilema> dilemasPool = new List<SODilema>();

    [Button]
    public void Init()
    {
        dilemasPool.Clear();
        dilemasPool.Add(startDilema);
    }

    [Button]
    public void FindAllDilemas()
    {
        dilemas = FindAssetsByType<SODilema>();
    }

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();

        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }

    public List<SODilema> GetAllAvalaibleDilemas()
    {
        Debug.Log("Dilemas in pool: " + dilemasPool.Count);
        RemoveNullDilemasFromPool();
        return dilemasPool.FindAll(d => d != null && d.IsDilemaAvalaible());
    }
    
    private void RemoveNullDilemasFromPool()
    {
        dilemasPool.RemoveAll(d => d == null);
    }

    public SODilema GetRandomDilema()
    {
        List<SODilema> avalaibleDilemas = GetAllAvalaibleDilemas();
        if (avalaibleDilemas.Count == 0) return null;
        return avalaibleDilemas[Random.Range(0, avalaibleDilemas.Count)];
    }

    public void AddDilema(SODilema dilema)
    {
        if (dilema)
        {
            dilemas.Add(dilema);
        }
        else
        {
            Debug.LogWarning("Tried to add dilema in database, but it was null");
        }
    }

    public void AddDilemaInPool(SODilema dilema)
    {
        if (dilema)
        {
            dilemasPool.Add(dilema);
        }
        else
        {
            Debug.LogWarning("Tried to add dilema in pool, but it was null");
        }
    }

    public void AddDilemaInPool(string key)
    {
        SODilema dilema = dilemas.Find((dilema) => dilema.key == key);
        if (dilema) {
            dilemasPool.Add(dilema);
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
            dilemasPool.AddRange(dilemas);
        }
        else
        {
            Debug.LogWarning("Tried to add dilemas in pool, but the List was empty");
        }
    }

    public void RemoveDilema(SODilema dilema)
    {
        dilemasPool.Remove(dilema);
    }

    public void ClearDilemaPool()
    {
        dilemasPool.Clear();
    }

    public SODilema GetDilema(string key)
    {
        return dilemas.Find((dilema) => dilema.key == key);
    }
}

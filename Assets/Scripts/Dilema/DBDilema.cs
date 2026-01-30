using EditorAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "DBDilema", menuName = "DataTables/Dilemas Data Table")]
public class DBDilema : ScriptableObject
{
    [SerializeField] public List<SODilema> dilemmas = new List<SODilema>();

    [Button]
    public void FindAllDilemmas()
    {
        dilemmas = FindAssetsByType<SODilema>();
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

    public void AddDilema(SODilema dilema)
    {
        if (dilema)
        {
            dilemmas.Remove(dilema);
            dilemmas.Add(dilema);
        }
        else
        {
            Debug.LogWarning("Tried to add dilema in database, but it was null");
        }
    }

    public SODilema GetDilema(string key)
    {
        return dilemmas.Find((dilema) => dilema.key == key);
    }
}

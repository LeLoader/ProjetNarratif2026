using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DBDilemma", menuName = "DataTables/Dilemmas Data Table")]
public class DBDilemma : ScriptableObject
{
    [SerializeField] public List<SODilemma> dilemmas = new List<SODilemma>();

    public void AddDilema(SODilemma dilemma)
    {
        if (dilemma)
        {
            dilemmas.Remove(dilemma);
            dilemmas.Add(dilemma);
        }
        else
        {
            Debug.LogWarning("Tried to add dilema in database, but it was null");
        }
    }

    public SODilemma GetDilema(string key)
    {
        return dilemmas.Find((dilemma) => dilemma.key == key);
    }
}

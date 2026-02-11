using UnityEngine;

[CreateAssetMenu(fileName = "PrefabRefSO", menuName = "Scriptable Objects/PrefabRefSO")]
public class PrefabRefSO : ScriptableObject
{
    [Header("Map objects")]
    public GameObject saplingPrefab;
    public GameObject lakePrefab;

    [Header("Character objects")]
    public GameObject bookPrefab;
    public GameObject candlePrefab;
    public GameObject cakePrefab;
    public GameObject glassPrefab;
    public GameObject pistolPrefab;
    public GameObject axePrefab;
    public GameObject wateringCanPrefab;
    public GameObject canPrefab;
}

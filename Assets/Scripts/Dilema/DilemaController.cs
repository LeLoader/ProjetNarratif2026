using EditorAttributes;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DilemaController : MonoBehaviour
{
    [SerializeField] SODilema TestDilema;
    [SerializeField] SODilemaLinker dilemaLinker;

    [Button]
    public void LoadTestDilema()
    {
        LoadDilema(TestDilema);
    }

    public void LoadDilema(SODilema dilema = null)
    {
        if (dilema)
        {
            dilemaLinker.SetDilema(dilema);
        }

        Debug.Log($"Loaded dilema: {dilema}");
    }

    [Button]
    public void PlayRandomDilema()
    {
        SODilema dilema = DilemaManager.GetRandomDilema();
        LoadDilema(dilema);
    }

    public void Awake()
    {
        // bind to the local change to trigger SetDilema again
        DilemaManager.dilemaDatabase.Init();
    }
}

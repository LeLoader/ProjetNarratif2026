using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void LoadAllSounds()
    {
        List<SOSound> Sounds = new List<SOSound>();
        Sounds = Resources.LoadAll<SOSound>("/Sounds/").ToList();
    }
}

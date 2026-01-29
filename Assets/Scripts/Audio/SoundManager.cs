using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SOSounds _sounds;
    
    private static SoundManager _instance;
    
    private AudioSource _source;
    private Dictionary<string, Sound> AllSounds = new Dictionary<string, Sound>();

    [SerializeField] private SoundsVolume _volumes;

    private void Awake()
    {
        Reset();
        foreach (Sound currentSound in _sounds.GetSounds())
        {
            if (AllSounds.TryAdd(currentSound.GetClip().name, currentSound))
            {
                Debug.Log($"[SOUND MANAGER] new sound added in database with key {currentSound.GetClip().name}");
            }
        }
        _volumes = new SoundsVolume(1, 1, 1);
    }

    private void OnValidate()
    {
        if (!gameObject.TryGetComponent<AudioSource>(out AudioSource _foundsource))
        {
            _source = gameObject.AddComponent<AudioSource>();
        } else
        {
            _source = _foundsource;
        }
    }

    private void Reset()
    {
        _sounds = Resources.Load<SOSounds>("Databases/SoundDatabase");
    }

    private void OnEnable()
    {
        if (_instance == null)
        {
            _instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string key)
    {
        if (AllSounds.TryGetValue(key, out Sound soundToPlay))
        {
            _source.clip = soundToPlay.GetClip();
            _source.volume = _volumes.GetVolume(soundToPlay.SoundType) * _volumes.GetVolume(ESoundType.Master);
            _source.Play();
        }
        else
        {
            Debug.LogError($"[SOUND MANAGER] sound not found for key {key}");
        }
    }

}

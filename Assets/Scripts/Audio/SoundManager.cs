using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SOSounds _sounds;
    
    public static SoundManager Instance;


    #region Sources
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _SFXSource;
    #endregion

    private Dictionary<string, Sound> AllSounds = new Dictionary<string, Sound>();

    [SerializeField] private SoundsVolume _volumes;


    private void Awake()
    {
        foreach (Sound currentSound in _sounds.GetSounds())
        {
            if (AllSounds.TryAdd(currentSound.GetClip().name, currentSound))
            {
                Debug.Log($"[SOUND MANAGER] new sound added in database with key {currentSound.GetClip().name}");
            }
        }
        _volumes = new SoundsVolume(1, 1, 1);
    }

    private void Reset()
    {
        _sounds = Resources.Load<SOSounds>("Databases/SoundDatabase");
        foreach (Transform children in transform)
        {
            Destroy(children.gameObject);
        }
        GameObject GO = new GameObject("Music Player");
        GO.transform.SetParent(transform);
        _musicSource = GO.AddComponent<AudioSource>();
        GO = new GameObject("SFX Player");
        GO.transform.SetParent(transform);
        _SFXSource = GO.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string key)
    {
        if (AllSounds.TryGetValue(key, out Sound soundToPlay))
        {
            AudioSource audioSource = GetAudioSource(soundToPlay.SoundType);
            audioSource.clip = soundToPlay.GetClip();
            audioSource.volume = _volumes.GetVolume(soundToPlay.SoundType) * _volumes.GetVolume(ESoundType.Master);
            audioSource.Play();
        }
        else
        {
            Debug.LogError($"[SOUND MANAGER] sound not found for key {key}");
        }
    }

    private AudioSource GetAudioSource(ESoundType type)
    {
        switch (type)
        {
            case ESoundType.Music:
                return _musicSource;
            case ESoundType.SFX:
                return _SFXSource;
            default:
                return _SFXSource;
        }
    }

}

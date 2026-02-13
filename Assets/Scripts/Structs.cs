using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR 
using AYellowpaper.SerializedCollections.KeysGenerators;
#endif

[Serializable]
public class Metric
{
    public Metric(LocalizedString label, EMetricType type)
    {
        this.label = label;
        this.type = type;
    }

    [SerializeField] public LocalizedString label;
    [SerializeField] public EMetricType type;

    public event Action<EMetricState> OnMetricReachedExtreme;

    [field: SerializeField, ReadOnly] public int Positive { get; private set; } = 0;
    [field: SerializeField, ReadOnly] public int Neutral { get; private set; } = 100;
    [field: SerializeField, ReadOnly] public int Negative { get; private set; } = 0;

    private void CheckExtreme()
    {
        if (Positive >= 100) OnMetricReachedExtreme?.Invoke(EMetricState.POSITIVE);
        else if (Negative >= 100) OnMetricReachedExtreme?.Invoke(EMetricState.NEGATIVE);
    }

    public void Set(int positive, int neutral, int negative)
    {
        if (positive + neutral + negative == 100)
        {
            Positive = positive;
            Neutral = neutral;
            Negative = negative;
            CheckExtreme();
        }
        else
        {
            Debug.LogWarning($"Tried to set values to metric {label.GetLocalizedString()}, but the total is different than 100");
        }
    }

    public void Add(EMetricState state, int value)
    {
        switch (state)
        {
            case EMetricState.NEUTRAL:
                Neutral += value;
                if (Positive - (int)Mathf.Ceil((float)value / 2) < 0)
                {
                    int deltaUnderZero = Positive - value;
                    Positive = 0;
                    Negative -= deltaUnderZero;
                }
                else if (Negative - (int)Mathf.Floor((float)value / 2) < 0)
                {
                    int deltaUnderZero = Negative - value;
                    Negative = 0;
                    Positive -= deltaUnderZero;
                }
                else
                {
                    Negative -= (int)Mathf.Floor(value / 2);
                    Positive -= (int)Mathf.Ceil(value / 2);
                }
                break;
            case EMetricState.POSITIVE:
                Positive += value;
                if (Neutral - value < 0)
                {
                    int deltaUnderZero = Neutral - value;
                    Neutral = 0;
                    Negative -= deltaUnderZero;
                }
                else
                {
                    Neutral -= value;
                }
                CheckExtreme();
                break;
            case EMetricState.NEGATIVE:
                Negative += value;
                if (Neutral - value < 0)
                {
                    int deltaUnderZero = Neutral - value;
                    Neutral = 0;
                    Positive -= deltaUnderZero;
                }
                else
                {
                    Neutral -= value;
                }
                    CheckExtreme();
                break;
        }
    }

    public void Add(int positive, int neutral, int negative)
    {
        if (positive + neutral + negative != 0)
        {
            Debug.LogWarning($"Tried to add values to metric {label.GetLocalizedString()}, but the total is different than 0");
        }
        else
        {
            Positive += positive;
            Neutral += neutral;
            Negative += negative;
            CheckExtreme();
        }
    }

    public int Get(EMetricState state)
    {
        switch (state)
        {
            case EMetricState.NEUTRAL:
                return Neutral;
            case EMetricState.POSITIVE:
                return Positive;
            case EMetricState.NEGATIVE:
                return Negative;
            default:
                return -1;
        }
    }

    public override string ToString()
    {
        return $"Positive: {Positive}%, Neutral: {Neutral}%, Negative: {Negative}%";
    }
}

[Serializable]
public struct Choice
{
    [SerializeField] public LocalizedString shortAnswerLabel;
    [SerializeField] public LocalizedString longAnswerLabel;
    [SerializeField] public List<string> actionsKeys;
    [SerializeField] public List<Consequence> consequences;
    [SerializeField] public List<SODilemma> newDilemmas;

    public void Activate(BehaviorController controller)
    {
        DilemmaManager.instance.AddDilemaInPool(newDilemmas);
        foreach (Consequence consequence in consequences)
        {
            GameManager.instance.globalMetrics.UpdateMetrics(consequence);
        }
        foreach (string key in actionsKeys)
        {
            controller.AddAction(ActionDataDrop.GetActionByID(key), 1);
        }
    }
}


public abstract class SpecialCondition : ScriptableObject 
{
    abstract public bool IsConditionReached();
}

[Serializable] // Trop de condition diff�rentes, en faire un SO?
public struct Condition
{
    [SerializeField] Metric metric;
    [SerializeField] EMetricState state;
    [SerializeField] int minimum;
    [SerializeField] int maximum;

    public bool IsConditionReached()
    {
        EMetricType type = metric.type;
        Metric globalMetric = GameManager.instance.globalMetrics.metrics.Find((currentMetric) => currentMetric.type == type);
        return globalMetric.Get(state) < maximum && globalMetric.Get(state) > minimum;
    }
}

[Serializable]
public struct Consequence
{
    [SerializeField] public EMetricType metricType;
    [SerializeField] public EMetricState state;
    [SerializeField] public int toAdd;

    public Consequence(EMetricType metricType, EMetricState state, int toAdd)
    {
        this.metricType = metricType;
        this.state = state;
        this.toAdd = toAdd;
    }
}

[Serializable]
public struct Sound
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] public ESoundType SoundType;

    public Sound(AudioClip _inClip, ESoundType _inSoundType)
    {
        _clip = _inClip;
        SoundType = _inSoundType;
    }

    public AudioClip GetClip()
    {
        return _clip;
    }

    public bool IsNull()
    {
        return _clip == null;
    }
}

[Serializable]
public struct SoundsVolume
{

    private Dictionary<ESoundType, float> _allVolumes;

    public SoundsVolume(float _inMasterVolume, float _inMusicVolume, float _inSFXVolume)
    {
        _allVolumes = new Dictionary<ESoundType, float>();
        foreach (ESoundType type in Enum.GetValues(typeof(ESoundType)))
        {
            switch (type)
            {
                case ESoundType.Master:
                    _allVolumes.Add(type, _inMasterVolume); ;
                    break;
                case ESoundType.Music:
                    _allVolumes.Add(type, _inMusicVolume);
                    break;
                case ESoundType.SFX:
                    _allVolumes.Add(type, _inSFXVolume);
                    break;
                default:
                    break;
            }
        }
    }

    public float GetVolume(ESoundType type)
    {
        return _allVolumes[type];
    }

    public void SetVolumes(ESoundType typeToModify, float newVolume)
    {
        _allVolumes[typeToModify] = newVolume;
    }
}

[Serializable]
public struct MetricsWrapper
{
    public EMetricState IndoctrinatedMetricState;
    public EMetricState ViolenceMetricState;

    public MetricsWrapper(EMetricState InIndoctrinatedState,  EMetricState InViolenceMetricType)
    {
        IndoctrinatedMetricState = InIndoctrinatedState;
        ViolenceMetricState = InViolenceMetricType;
    }
}

#if UNITY_EDITOR 
[KeyListGenerator("Wrapper Range", typeof(MetricsWrapper))]
public class MetricRangeGenerator : KeyListGenerator
{
    public override IEnumerable GetKeys(Type type)
    {
        for (int i = 0; i < 3; i++)
        {
            EMetricState firstState;
            switch (i)
            {
                case 0:
                    firstState = EMetricState.NEUTRAL;
                    break;
                case 1:
                    firstState = EMetricState.NEGATIVE;
                    break;
                case 2:
                    firstState = EMetricState.POSITIVE;
                    break;
                default:
                    firstState = EMetricState.NEUTRAL;
                    break;

            }
            for (int j = 0; j < 3; j++)
            {
                EMetricState secondState;
                switch (j)
                {
                    case 0:
                        secondState = EMetricState.NEUTRAL;
                        break;
                    case 1:
                        secondState = EMetricState.NEGATIVE;
                        break;
                    case 2:
                        secondState = EMetricState.POSITIVE;
                        break;
                    default:
                        secondState = EMetricState.NEUTRAL;
                        break;
                }
                yield return new MetricsWrapper(firstState, secondState);
            }
        }
    }
}

#endif
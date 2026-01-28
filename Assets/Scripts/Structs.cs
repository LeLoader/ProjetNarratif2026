using Codice.Client.Common.FsNodeReaders.Watcher;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public struct Metric
{
    [SerializeField] public LocalizedString label;
    [SerializeField] public EMetricType type;

    public event Action<EMetricState> OnMetricReachedExtreme;

    public int Positive { get; private set; }
    public int Neutral { get; private set; }
    public int Negative { get; private set; }

    private void CheckExtreme()
    {
        if (Positive == 100) OnMetricReachedExtreme?.Invoke(EMetricState.POSITIVE);
        else if (Negative == 100) OnMetricReachedExtreme?.Invoke(EMetricState.NEGATIVE);
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
        return $"Positive: {Positive}%, Neutral: {Neutral}%, Negative: {Negative}";
    }
}

[Serializable]
public struct Choice
{
    [SerializeField] public LocalizedString label;
    [SerializeField] public List<ActionBase> actions;
    [SerializeField] public List<Consequence> consequences;
    [SerializeField] public List<SODilema> newDilemas;

    public void Activate()
    {
        DilemaManager.dilemaDatabase.AddDilemaInPool(newDilemas);
        foreach (Consequence consequence in consequences)
        {
            DilemaManager.globalMetrics.UpdateMetrics(consequence);
        }
    }
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
public struct Sounds
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private ESoundType _soundType;

    public AudioClip GetClip()
    {
        return _clip;
    }

    public ESoundType GetSoundType()
    {
        return _soundType;
    }
}
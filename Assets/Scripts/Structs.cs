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

    public MetricValues Values { get; }
}

[Serializable]
public struct MetricValues
{
    public MetricValues(float positive, float neutral, float negative)
    {
        Positive = positive;
        Neutral = neutral;
        Negative = negative;
    }

    public void Set(float positive, float neutral, float negative)
    {
        if (positive + neutral + negative == 0)
        {
            Positive = positive;
            Neutral = neutral;
            Negative = negative;
        }
        else
        {
            Debug.LogWarning("Tried to set values to metric, but the total is different than 0");
        }
    }

    public void Add(float positive, float neutral, float negative)
    {
        if (positive + neutral + negative == 0)
        {
            Positive += positive;
            Neutral += neutral;
            Negative += negative;
        }
        else
        {
            Debug.LogWarning("Tried to add values to metric, but the total is different than 0");
        }
    }

    public float Get(EMetricState state)
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

    public float Positive { get; private set; }
    public float Neutral { get; private set; }
    public float Negative { get; private set; }
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
    }
}

[Serializable] // Trop de condition différentes, en faire un SO?
public struct Condition
{
    [SerializeField] Metric metric;
    [SerializeField] EMetricState state;
    [SerializeField] float minimum;
    [SerializeField] float maximum;

    public bool IsConditionReached()
    {
        EMetricType type = metric.type;
        Metric globalMetric = GameManager.globalMetrics.metrics.Find((currentMetric) => currentMetric.type == type);
        return globalMetric.Values.Get(state) < maximum && globalMetric.Values.Get(state) > minimum;
    }
}

[Serializable]
public struct Consequence
{
    [SerializeField] EMetricType metricType;
    [SerializeField] EMetricState metricState;
    [SerializeField] float toAdd;

    public Consequence(EMetricType metricType, EMetricState metricState, float toAdd)
    {
        this.metricType = metricType;
        this.metricState = metricState;
        this.toAdd = toAdd;
    }
}
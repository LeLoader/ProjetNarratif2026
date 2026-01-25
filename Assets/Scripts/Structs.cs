using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public struct Metric
{
    [SerializeField] LocalizedString label;
    [field: SerializeField] public EMetricType Type { get; }
    [field: SerializeField] public float Value { get; }

    // public static Metric operator +(Metric left, float right) => left;
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
    [SerializeField] EMetricType metricType;
    [SerializeField] float minimum;
    [SerializeField] float maximum;
    public bool IsConditionReached() 
    {
        EMetricType type = metricType;
        Metric metric = GameManager.globalMetrics.metrics.Find((metric) => metric.Type == type);
        return metric.Value < maximum && metric.Value > minimum;
    }
}

[Serializable]
public struct Consequence
{
    [SerializeField] EMetricType metricType;
    [SerializeField] float toAdd;
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
public struct Metric
{
    [SerializeField] LocalizedString label;
    [SerializeField] EMetricType type;
    float value;

    // public static Metric operator +(Metric left, float right) => left;
}

[Serializable]
public struct Choice
{
    
    [SerializeField] LocalizedString label;
    [SerializeField] List<Consequence> Consequences;
    
}

[Serializable]
public struct Condition
{
    [SerializeField] EMetricType metricType;
    [SerializeField] float minimum;
    [SerializeField] float maximum;
    bool bIsConditionReached() 
    {
        return true; 
    }
}

[Serializable]
public struct Consequence
{
    [SerializeField] EMetricType metricType;
    [SerializeField] float toAdd;
}
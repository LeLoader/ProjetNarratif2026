using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOGlobalMetrics", menuName = "ScriptableObjects/SOGlobalMetrics")]
public class SOGlobalMetrics : ScriptableObject
{
    public List<Metric> metrics = new();

    public void UpdateMetrics(Consequence consequence)
    {
        // Metric metric = metrics.Find((m) => m.type == consequence.metricType);
        // metric.Values.Add(consequence.state, consequence.toAdd);
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOGlobalMetrics", menuName = "ScriptableObjects/SOGlobalMetrics")]
public class SOGlobalMetrics : ScriptableObject
{
    [SerializeField] public List<Metric> Metrics = new();
}

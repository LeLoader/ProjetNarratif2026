using System;
using UnityEngine;

[Serializable]
public enum EMetricType
{
    NONE      = 0,
    FREEWILL  = 1 << 0, // INDOCTRINATED
    PEACE     = 1 << 1, // ANGER
}

public enum EMetricState
{
    NEUTRAL  = 0,
    POSITIVE = 1,
    NEGATIVE = 2
}

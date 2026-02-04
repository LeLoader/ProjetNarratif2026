using System;
using UnityEngine;

[Serializable]
public enum EMetricType
{
    NONE      = 0,
    INDOCTRINATED = 1 << 0, // FREEDOM
    VIOLENCE     = 1 << 1, // PEACE
}

public enum EMetricState
{
    NEUTRAL  = 0,
    POSITIVE = 1,
    NEGATIVE = 2
}

public enum ESoundType
{
    None,
    Master,
    Music,
    SFX,
    Foley,
    AMB,
    Vocals,
}
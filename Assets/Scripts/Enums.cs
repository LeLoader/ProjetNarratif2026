using System;
using UnityEngine;

[Flags, Serializable]
public enum EMetricType
{
    NONE      = 0,
    FREEWILL     = 1 << 0, // INDOCTRINATED
    PEACE     = 1 << 1, // ANGER
    HAPPINESS = 1 << 2 // SADNESS
}

public enum ESoundType
{
    None,
    Music,
    SFX,
}
﻿using UnityEngine;

public class DesireForSweets : CharacterStat<float>
{
    public new float Value
    {
        get => base.Value;
        set => base.Value = Mathf.Clamp(value, MinValue, MaxValue);
    }

    public float MaxValue { get; set; } = 100;
    public float MinValue { get; set; } = 0;
}
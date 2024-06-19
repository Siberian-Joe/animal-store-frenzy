using System;
using UnityEngine;

[Serializable]
public abstract class CharacterStat<T> : ICharacterStat
{
    [SerializeField] private T _value;

    public T Value
    {
        get => _value;
        set => _value = value;
    }
}
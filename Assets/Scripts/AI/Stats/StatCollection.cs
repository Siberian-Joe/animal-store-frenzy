using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StatCollection : IStatCollection
{
    [SerializeReference] private List<ICharacterStat> _stats = new();

    private Dictionary<Type, ICharacterStat> _statsDictionary;

    public T GetStat<T>() where T : ICharacterStat
    {
        UpdateStatsDictionary();

        if (_statsDictionary.TryGetValue(typeof(T), out var stat))
            return (T)stat;

        Debug.LogWarning($"Stat of type {typeof(T)} not found.");
        return default;
    }

    public void AddStat<T>(T stat) where T : ICharacterStat
    {
        if (stat == null)
        {
            Debug.LogWarning("Stat is null.");
            return;
        }

        var type = typeof(T);
        UpdateStatsDictionary();

        if (_statsDictionary.ContainsKey(type))
        {
            Debug.LogWarning($"Stat of type {type} already exists.");
            return;
        }

        _stats.Add(stat);
        _statsDictionary[type] = stat;
    }

    private void UpdateStatsDictionary() =>
        _statsDictionary ??= _stats.ToDictionary(stat => stat.GetType(), stat => stat);
}
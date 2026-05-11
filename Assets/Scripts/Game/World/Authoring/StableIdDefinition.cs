using System;
using UnityEngine;

namespace Game.World.Authoring
{
    public abstract class StableIdDefinition : ScriptableObject
    {
        [SerializeField] private string _id;

        protected string StableIdValue => _id;

        protected virtual void OnValidate() => _id = _id?.Trim();

        public void ValidateStableId()
        {
            if (string.IsNullOrWhiteSpace(_id))
                throw new InvalidOperationException($"{GetType().Name} '{name}' has empty stable id.");
        }
    }
}
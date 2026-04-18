using System;
using Game.World.Shop;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct CustomerNeedHandle : IEquatable<CustomerNeedHandle>
    {
        public string NeedId { get; }
        public ProductId ProductId { get; }
        public float Intensity { get; }

        public CustomerNeedHandle(string needId, ProductId productId, float intensity)
        {
            NeedId = string.IsNullOrWhiteSpace(needId)
                ? throw new ArgumentException("Need id cannot be empty.", nameof(needId))
                : needId.Trim();

            ProductId = productId;
            Intensity = Mathf.Clamp01(intensity);
        }

        public bool Equals(CustomerNeedHandle other) =>
            string.Equals(NeedId, other.NeedId, StringComparison.Ordinal);

        public override bool Equals(object obj) =>
            obj is CustomerNeedHandle other && Equals(other);

        public override int GetHashCode() =>
            StringComparer.Ordinal.GetHashCode(NeedId);
    }
}
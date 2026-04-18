using System;
using Game.World.Shop.Products;
using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    [Serializable]
    public struct CustomerNeedProfileEntry
    {
        public string NeedId;
        public ProductDefinition Product;

        [Min(0)] public int Weight;

        [Range(0f, 1f)] public float MinIntensity;
        [Range(0f, 1f)] public float MaxIntensity;
    }
}
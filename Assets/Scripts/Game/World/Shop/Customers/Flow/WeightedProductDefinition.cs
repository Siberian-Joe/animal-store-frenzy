using System;
using Game.World.Shop.Products;
using UnityEngine;

namespace Game.World.Shop.Customers.Flow
{
    [Serializable]
    public struct WeightedProductDefinition
    {
        public ProductDefinition Product;

        [Min(0)] public int Weight;
    }
}
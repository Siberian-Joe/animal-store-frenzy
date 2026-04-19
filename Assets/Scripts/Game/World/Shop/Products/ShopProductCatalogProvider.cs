using System;
using System.Collections.Generic;

namespace Game.World.Shop.Products
{
    public sealed class ShopProductCatalogProvider :
        IShopProductCatalogProvider,
        IShopProductCatalogInitializer
    {
        public IReadOnlyList<ProductDefinition> All
        {
            get
            {
                EnsureInitialized();
                return _all;
            }
        }

        private readonly Dictionary<ProductId, ProductDefinition> _productsById = new();

        private ProductDefinition[] _all = Array.Empty<ProductDefinition>();
        private bool _initialized;

        void IShopProductCatalogInitializer.Initialize(IReadOnlyList<ProductDefinition> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (_initialized)
            {
                throw new InvalidOperationException(
                    $"{nameof(ShopProductCatalogProvider)} is already initialized.");
            }

            _productsById.Clear();
            _all = new ProductDefinition[products.Count];

            for (var index = 0; index < products.Count; index++)
            {
                var product = products[index];
                if (product == false)
                {
                    throw new InvalidOperationException(
                        $"{nameof(ShopProductCatalogProvider)} received a null product definition at index {index}.");
                }

                var productId = product.ProductId;

                if (_productsById.TryAdd(productId, product) == false)
                {
                    throw new InvalidOperationException(
                        $"Duplicate product definition '{productId}' detected in preloaded catalog.");
                }

                _all[index] = product;
            }

            _initialized = true;
        }

        public ProductDefinition Get(ProductId productId)
        {
            EnsureInitialized();

            if (_productsById.TryGetValue(productId, out var product))
                return product;

            throw new InvalidOperationException(
                $"Product catalog does not contain product '{productId}'.");
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            throw new InvalidOperationException(
                $"{nameof(ShopProductCatalogProvider)} was used before initialization.");
        }
    }
}
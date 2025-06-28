using System;
using System.Collections.Generic;
using System.Linq;
using NewCore.Data;
using NewCore.Domain;
using ObservableCollections;
using R3;

namespace NewCore.Extensions
{
    public static class ObservableListExtensions
    {
        public static void InitializeFromModels<TModel, TProxy>(
            this ObservableList<TProxy> list,
            ICollection<TModel> models,
            CompositeDisposable disposables)
            where TModel : EntityData
            where TProxy : Entity<TModel>, new()
        {
            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            models ??= new List<TModel>();

            foreach (var model in models)
            {
                var proxy = new TProxy();
                proxy.Initialize(model);
                list.Add(proxy);
            }

            list.ObserveAdd()
                .Subscribe(addEvent => models.Add(addEvent.Value.ToModel()))
                .AddTo(disposables);

            list.ObserveRemove()
                .Subscribe(removeEvent => models.Remove(removeEvent.Value.ToModel()))
                .AddTo(disposables);
        }

        public static void AddModel<TModel, TProxy>(
            this ObservableList<TProxy> list,
            TModel model)
            where TModel : IModel
            where TProxy : Proxy<TModel>, new()
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var proxy = new TProxy();
            proxy.Initialize(model);
            list.Add(proxy);
        }

        public static void RemoveModel<TModel, TProxy>(
            this ObservableList<TProxy> list,
            TModel model)
            where TModel : EntityData
            where TProxy : Entity<TModel>, new()
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var proxy = list.FirstOrDefault(p => p.IsEquivalentTo(model));
            if (proxy != null)
                list.Remove(proxy);
        }

        public static List<TModel> ToModelList<TModel, TProxy>(
            this IEnumerable<TProxy> proxies)
            where TModel : EntityData
            where TProxy : Entity<TModel> =>
            proxies.Select(proxy => proxy.ToModel()).ToList();

        public static void ClearAndDispose<T>(this ICollection<T> items) where T : IDisposable
        {
            foreach (var item in items)
                item.Dispose();

            items.Clear();
        }
    }
}
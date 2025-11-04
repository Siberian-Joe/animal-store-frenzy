using System;
using System.Collections.Generic;
using System.Linq;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Modules.Interaction;
using ObservableCollections;
using R3;
using UnityEngine;

namespace NewCore.Extensions
{
    public static class ObservableListExtensions
    {
        public static IDisposable InitializeFromModels<TModel, TProxy>(
            this ObservableList<TProxy> list,
            ICollection<TModel> models, IProxyFactory proxyFactory)
            where TModel : IModel
            where TProxy : Proxy<TModel>
        {
            models ??= new List<TModel>();

            var disposables = new CompositeDisposable();

            foreach (var model in models)
                list.Add(proxyFactory.Create<TProxy>(model));

            list.ObserveAdd()
                .Subscribe(addEvent => models.Add(addEvent.Value.ToModel()))
                .AddTo(disposables);

            list.ObserveRemove()
                .Subscribe(remEvent => models.Remove(remEvent.Value.ToModel()))
                .AddTo(disposables);

            return disposables;
        }

        public static void AddModel<TProxy>(
            this ObservableList<TProxy> list,
            IModel model,
            IProxyFactory proxyFactory)
            where TProxy : IProxy
        {
            if (model == null)
            {
                Debug.LogError("Model is null. Cannot create proxy for null model");
                return;
            }

            if (proxyFactory == null)
            {
                Debug.LogError("Proxy factory is null. Cannot create proxy for model");
                return;
            }

            list.Add(proxyFactory.Create<TProxy>(model));
        }

        public static void RemoveModel<TModel, TProxy>(
            this ObservableList<TProxy> list,
            TModel model)
            where TModel : EntityData
            where TProxy : Entity<TModel>, new()
        {
            if (model == null)
            {
                Debug.LogError("Model is null. Cannot remove proxy for null model");
                return;
            }

            var proxy = list.FirstOrDefault(proxy => proxy.Equals(model));
            if (proxy != null)
                list.Remove(proxy);
        }

        public static void ClearAndDispose<T>(this ICollection<T> items) where T : IDisposable
        {
            foreach (var item in items)
                item.Dispose();

            items.Clear();
        }
    }
}
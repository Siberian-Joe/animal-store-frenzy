using System;
using System.Collections.Generic;
using System.Linq;
using NewCore.Domain;
using NewCore.Extensions;
using ObservableCollections;
using R3;

namespace NewCore.Data
{
    public class ProxyCollection<TModel, TProxy> : ObservableList<TProxy>, IDisposable
        where TModel : Entity
        where TProxy : EntityProxy<TModel>, new()
    {
        private readonly CompositeDisposable _disposables = new();

        public ProxyCollection(List<TModel> models)
        {
            models ??= new List<TModel>();

            foreach (var model in models)
            {
                var proxy = new TProxy();
                proxy.Initialize(model);
                Add(proxy);
            }

            this.ObserveAdd().Subscribe(added =>
            {
                var proxy = added.Value;
                models.Add(proxy.ToModel());
            }).AddTo(_disposables);

            this.ObserveRemove().Subscribe(removed =>
            {
                var proxy = removed.Value;
                models.RemoveAll(model => proxy.IsEquivalentTo(model));
            }).AddTo(_disposables);
        }

        public void AddModel(TModel model)
        {
            var proxy = new TProxy();
            proxy.Initialize(model);
            Add(proxy);
        }

        public void RemoveModel(TModel model)
        {
            var proxy = this.FirstOrDefault(p => p.IsEquivalentTo(model));
            if (proxy != null)
                Remove(proxy);
        }

        public void Dispose() => _disposables?.Dispose();
    }
}
using System;
using NewCore.Domain;
using R3;

namespace NewCore.Data
{
    public abstract class Proxy : IProxy, IDisposable
    {
        protected readonly CompositeDisposable Disposables = new();

        IModel IProxy.ToModel() => CreateModelCore();

        protected abstract IModel CreateModelCore();

        public virtual void Dispose() => Disposables.Dispose();
    }

    public abstract class Proxy<TModel> : Proxy
        where TModel : IModel
    {
        protected TModel Model { get; }

        protected Proxy(TModel model) => Model = model;

        public TModel ToModel() => CreateModel();

        protected sealed override IModel CreateModelCore() => CreateModel();

        protected abstract TModel CreateModel();
    }
}
using NewCore.Data;
using NewCore.Domain;
using Zenject;

namespace NewCore.Modules.Interaction
{
    public class ProxyFactory : IProxyFactory
    {
        private readonly IInstantiator _instantiator;

        public ProxyFactory(IInstantiator instantiator) => _instantiator = instantiator;

        public TProxy Create<TProxy>(IModel model)
            where TProxy : IProxy =>
            _instantiator.Instantiate<TProxy>(new object[] { model });
    }
}
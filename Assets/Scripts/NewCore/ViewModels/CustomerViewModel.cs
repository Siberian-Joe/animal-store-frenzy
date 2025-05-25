using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public class CustomerViewModel : EntityViewModel<Customer>
    {
        public ReadOnlyReactiveProperty<Vector3Int> Position => Proxy.Position;

        public CustomerViewModel(Customer proxy) : base(proxy)
        {
        }

        public override void Dispose()
        {
            base.Dispose();

            Proxy?.Dispose();
        }
    }
}
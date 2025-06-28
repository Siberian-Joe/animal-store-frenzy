using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public class CustomerViewModel : EntityViewModel<Customer>
    {
        public ReadOnlyReactiveProperty<Vector2> Position => Proxy.Position;

        public CustomerViewModel(Customer proxy) : base(proxy)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            Position.Dispose();
        }
    }
}
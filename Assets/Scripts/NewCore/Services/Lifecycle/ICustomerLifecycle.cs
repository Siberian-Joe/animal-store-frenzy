using NewCore.Data;
using NewCore.ViewModels;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public interface ICustomerLifecycle : IEntityLifecycle<Customer, CustomerViewModel>
    {
        bool TrySpawnCustomer(string customerType, Vector2 position);
    }
}
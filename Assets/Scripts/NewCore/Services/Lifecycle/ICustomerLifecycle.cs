using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.ViewModels;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public interface ICustomerLifecycle : IEntityLifecycle<Customer, CustomerProxy, CustomerViewModel>
    {
        bool TrySpawnCustomer(string customerType, Vector3Int position);
    }
}
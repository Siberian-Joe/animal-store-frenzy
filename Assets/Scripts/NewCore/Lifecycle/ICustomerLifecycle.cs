using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.ViewModels;
using ObservableCollections;
using UnityEngine;

namespace NewCore.Lifecycle
{
    public interface ICustomerLifecycle : IEntityLifecycle<Customer, CustomerProxy, CustomerViewModel>
    {
        UniTask<bool> TrySpawnCustomer(string customerType, Vector3Int position);
    }
}
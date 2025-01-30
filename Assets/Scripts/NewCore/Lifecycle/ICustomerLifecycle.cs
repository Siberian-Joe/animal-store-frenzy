using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.ViewModels;
using ObservableCollections;
using UnityEngine;

namespace NewCore.Lifecycle
{
    public interface ICustomerLifecycle
    {
        IObservableCollection<CustomerViewModel> Customers { get; }
        void Initialize(GameStateProxy gameStateStateProxy);
        UniTask<bool> TrySpawnCustomer(string customerType, Vector3Int position);
    }
}
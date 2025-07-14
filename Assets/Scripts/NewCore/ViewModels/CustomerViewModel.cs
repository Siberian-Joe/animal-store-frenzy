using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public class CustomerViewModel : EntityViewModel<Customer>
    {
        public CustomerViewModel(Customer proxy) : base(proxy)
        {
        }
    }
}
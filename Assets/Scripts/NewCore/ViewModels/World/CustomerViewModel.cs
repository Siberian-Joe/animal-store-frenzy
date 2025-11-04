using NewCore.Data;

namespace NewCore.ViewModels.World
{
    public class CustomerViewModel : EntityViewModel<Customer>
    {
        public CustomerViewModel(Customer proxy) : base(proxy)
        {
        }
    }
}
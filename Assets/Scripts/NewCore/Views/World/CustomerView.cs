using NewCore.ViewModels;

namespace NewCore.Views.World
{
    public class CustomerView : View<CustomerViewModel>
    {
        protected override void OnBind()
        {
            transform.position =
                ViewModel.Position
                    .CurrentValue; // TODO: Need to make dynamic assignment, now this is just a placeholder
        }
    }
}
using NewCore.ViewModels;

namespace NewCore.Views.World
{
    public class CustomerView : EntityView<CustomerViewModel>
    {
        protected override void OnBind()
        {
            base.OnBind();
            // TODO: Need to make dynamic assignment, now this is just a placeholder
            transform.position = ViewModel.Position.CurrentValue;
        }
    }
}
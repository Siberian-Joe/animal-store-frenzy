using NewCore.Factories;
using NewCore.ViewBinders;
using NewCore.ViewModels;
using UnityEngine;
using Zenject;

namespace NewCore.Views.World
{
    public class CustomersWorldView : NodeView<CustomersWorldViewModel>
    {
        [SerializeField] private CustomerView _customerViewPrefab;

        [Inject] private IViewFactory _viewFactory;
        [Inject] private IViewBinder _viewBinder;

        protected override void OnBind() => _viewBinder.Bind(ViewModel.Customers, _customerViewPrefab, transform);
    }
}
using NewCore.ViewModels;
using R3;
using UnityEngine;

namespace NewCore.Views
{
    public interface INavigableEntityViewModel : IEntityViewModel
    {
        ReactiveProperty<Vector3> TargetPosition { get; }
    }
}
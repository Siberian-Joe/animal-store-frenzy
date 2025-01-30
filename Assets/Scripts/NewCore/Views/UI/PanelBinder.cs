using R3;
using UnityEngine;

namespace NewCore.Views.UI
{
    public abstract class PanelBinder : MonoBehaviour
    {
        public Observable<Unit> Clicked => _clicked;

        private readonly Subject<Unit> _clicked = new();

        public void OnClick() => _clicked?.OnNext(Unit.Default);
    }
}
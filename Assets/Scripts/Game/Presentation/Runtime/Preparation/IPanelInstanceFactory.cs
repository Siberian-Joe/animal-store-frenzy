using System;
using Game.Presentation.Runtime.Panels;
using UnityEngine;

namespace Game.Presentation.Runtime.Preparation
{
    public interface IPanelInstanceFactory
    {
        TPresenter CreatePresenter<TPresenter>()
            where TPresenter : PanelPresenter;

        PanelPresenter CreatePresenter(Type presenterType);

        GameObject CreatePanelRoot(GameObject prefab);

        TComponent CreateComponent<TComponent>(GameObject prefab)
            where TComponent : Component;
    }
}
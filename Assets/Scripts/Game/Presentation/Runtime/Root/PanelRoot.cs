using System;
using System.Collections.Generic;
using Game.Presentation.Runtime.Layers;
using Game.Presentation.Runtime.Panels;
using UnityEngine;

namespace Game.Presentation.Runtime.Root
{
    public sealed class PanelRoot : MonoBehaviour, IPanelRoot
    {
        private readonly List<IPanelLayer> _layers = new();
        private bool _initialized;

        private void Awake()
        {
            Initialize();
        }

        public IPanelLayer GetLayerFor(Panel panel)
        {
            if (panel == false)
                throw new ArgumentNullException(nameof(panel));

            Initialize();

            IPanelLayer result = null;

            foreach (var layer in _layers)
            {
                if (layer.CanHandle(panel) == false)
                    continue;

                if (result != null)
                {
                    throw new InvalidOperationException(
                        $"Panel '{panel.GetType().FullName}' matches more than one presentation layer.");
                }

                result = layer;
            }

            return result ?? throw new InvalidOperationException(
                $"No presentation layer found for panel '{panel.GetType().FullName}'.");
        }

        private void Initialize()
        {
            if (_initialized)
                return;

            var layers = GetComponentsInChildren<IPanelLayer>(true);

            foreach (var layer in layers)
            {
                if (layer == null)
                    continue;

                if (_layers.Contains(layer))
                    continue;

                _layers.Add(layer);
            }

            if (_layers.Count == 0)
                throw new InvalidOperationException($"{nameof(PanelRoot)} has no presentation layers.");

            _initialized = true;
        }
    }
}
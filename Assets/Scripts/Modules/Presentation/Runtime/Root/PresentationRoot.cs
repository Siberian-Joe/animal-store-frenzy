using System;
using System.Collections.Generic;
using Modules.Presentation.Runtime.Layers;
using Modules.Presentation.Runtime.Panels;
using UnityEngine;

namespace Modules.Presentation.Runtime.Root
{
    public sealed class PresentationRoot : MonoBehaviour, IPresentationRoot
    {
        private readonly List<IPanelLayer> _layers = new();
        private readonly Dictionary<Type, Panel> _preplacedPanelsByType = new();

        private bool _initialized;

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

        public bool TryTakePreplacedPanel(Type panelType, out Panel panel)
        {
            if (panelType == null)
                throw new ArgumentNullException(nameof(panelType));

            Initialize();

            if (_preplacedPanelsByType.Remove(panelType, out panel))
                return panel;

            panel = null;
            return false;
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
                throw new InvalidOperationException($"{nameof(PresentationRoot)} has no presentation layers.");

            CollectPreplacedPanels();

            _initialized = true;
        }

        private void CollectPreplacedPanels()
        {
            var panels = GetComponentsInChildren<Panel>(true);

            foreach (var panel in panels)
            {
                if (panel == false)
                    continue;

                var panelType = panel.GetType();

                if (_preplacedPanelsByType.TryAdd(panelType, panel) == false)
                {
                    throw new InvalidOperationException(
                        $"{nameof(PresentationRoot)} contains more than one preplaced panel '{panelType.FullName}'.");
                }
            }
        }
    }
}
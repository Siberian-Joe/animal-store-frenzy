using System.Collections.Generic;
using NewCore.Factories;
using NewCore.Views.World;
using Zenject;

namespace NewCore.ViewBinders
{
    public class SceneNodeComposer : ISceneNodeComposer
    {
        private readonly IEnumerable<INodeView> _nodes;
        private readonly IViewModelFactory _factory;

        public SceneNodeComposer(
            IEnumerable<INodeView> nodes,
            IViewModelFactory factory)
        {
            _nodes = nodes;
            _factory = factory;
        }

        public void Initialize()
        {
            foreach (var node in _nodes)
                node.Initialize(_factory);
        }
    }
}
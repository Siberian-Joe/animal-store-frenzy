using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.Store
{
    public sealed class OpenStoreInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Open Store Interaction")] [SerializeField]
        private string _interactionActionId = "open-store";

        [SerializeField] private StoreStatusPart _storeStatus;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _storeStatus ??= GetComponent<StoreStatusPart>();
            if (_storeStatus == false)
                return;

            if (_storeStatus.IsOpen)
                return;

            var storeRoot = GetComponentInParent<EntityRoot>();
            if (storeRoot == false)
                return;

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                new OpenStoreCommand(storeRoot.Id),
                storeRoot));
        }
    }
}

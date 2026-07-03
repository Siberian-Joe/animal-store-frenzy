using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.Store
{
    public sealed class CloseStoreInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Close Store Interaction")] [SerializeField]
        private string _interactionActionId = "close-store";

        [SerializeField] private StoreShiftPart _storeShift;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _storeShift ??= GetComponent<StoreShiftPart>();
            if (_storeShift == false || _storeShift.CanCloseStore == false)
                return;

            var storeRoot = GetComponentInParent<EntityRoot>();
            if (storeRoot == false)
                return;

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                new CloseStoreCommand(storeRoot.Id),
                storeRoot));
        }
    }
}

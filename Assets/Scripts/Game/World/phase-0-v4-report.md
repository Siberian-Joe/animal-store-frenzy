# Phase 0 Report

## Status

Phase 0 inventory is complete enough to expose the current v3 baseline, the scene/prefab blast radius, and the remaining horizontal ownership problem.

Phase 0 is not safe to close yet because one spec/plan conflict was found: `Game.World.Debugging` exists in the codebase, but the current target package model in `spec.md` and the owner-map rule in `development-plan.md` only allow `Core`, `Composition`, `Interactions`, or a concrete capability slice.

## Baseline

Current runtime baseline is confirmed as:

- `SceneCompositionBootstrap -> EntityComposer -> ICompositionModule`
- no legacy `Step + Factory`
- no legacy shim layer

User-confirmed manual Unity baseline for `Assets/Content/Scenes/Core 1.unity`:

- scene opens without missing scripts
- `SceneContext`, `Man`, and `Chest` pick up the current components correctly
- composition starts on scene init
- click-to-move works
- take-from-shelf works

Accepted out-of-scope constraints remain unchanged:

- external `NuGetForUnity.dll` build blocker
- accepted `restock` validation gap

## Current Horizontal Ownership Inventory

Current horizontal namespace buckets still present in `Game.World`:

- `Game.World.Features`: 18 files
- `Game.World.Parts`: 12 files
- `Game.World.Installers`: 7 files
- `Game.World.State`: 3 files

This matches the architectural smell described in `spec.md`: folders are already mostly vertical, but logical ownership is still horizontal.

## Owner Map

### `Core`

Target owner package:

- `Core/EntityId.cs` -> `EntityId`
- `Core/EntityFeature.cs` -> `EntityFeature`
- `Core/IEntityFeature.cs` -> `IEntityFeature`
- `Core/EntityIdentifier.cs` -> `EntityIdentifier`
- `Core/EntityRoot.cs` -> `EntityRoot`
- `Core/EntityState.cs` -> `EntityState`
- `Core/IEntityStateStore.cs` -> `IEntityStateStore`
- `Core/InMemoryEntityStateStore.cs` -> `InMemoryEntityStateStore`

Current namespace mismatch inside this package:

- `EntityFeature`
- `IEntityFeature`

Both still declare `Game.World.Features` and will need owner-aligned namespace cleanup in migration work.

### `Composition`

Target owner package:

- `Composition/ICompositionModule.cs` -> `ICompositionModule`
- `Composition/FeatureModule.cs` -> `FeatureModule`
- `Composition/EntityComposer.cs` -> `EntityComposer`
- `Composition/IEntityComposer.cs` -> `IEntityComposer`
- `Composition/EntityCompositionContext.cs` -> `EntityCompositionContext`
- `Composition/SceneCompositionBootstrap.cs` -> `SceneCompositionBootstrap`
- `Composition/EntityCompositionInstaller.cs` -> `EntityCompositionInstaller`
- `Composition/IFeaturePart.cs` -> `IFeaturePart`
- `Composition/FeaturePart.cs` -> `FeaturePart`

Current namespace mismatch inside this package:

- `EntityCompositionInstaller` still declares `Game.World.Installers`
- `IFeaturePart` still declares `Game.World.Parts`
- `FeaturePart` still declares `Game.World.Parts`

### `Interactions`

Target owner package:

- `Interactions/IEntityInteraction.cs` -> `IEntityInteraction`
- `Interactions/IInteractionResolver.cs` -> `IInteractionResolver`
- `Interactions/InteractionResolverPart.cs` -> `InteractionResolverPart`

Current namespace mismatch inside this package:

- `IEntityInteraction` still declares `Game.World.Features`
- `IInteractionResolver` still declares `Game.World.Features`
- `InteractionResolverPart` still declares `Game.World.Parts`

### `Transform`

Target owner package:

- `Modules/Transform/ITransformFeature.cs` -> `ITransformFeature`
- `Modules/Transform/TransformFeature.cs` -> `TransformFeature`
- `Modules/Transform/TransformState.cs` -> `TransformState`
- `Modules/Transform/TransformModule.cs` -> `TransformModule`
- `Modules/Transform/TransformPart.cs` -> `TransformPart`
- `Modules/Transform/TransformModuleInstaller.cs` -> `TransformModuleInstaller`

### `Navigation`

Target owner package:

- `Modules/Navigation/INavigationFeature.cs` -> `INavigationFeature`
- `Modules/Navigation/NavigationFeature.cs` -> `NavigationFeature`
- `Modules/Navigation/NavigationState.cs` -> `NavigationState`
- `Modules/Navigation/NavigationModule.cs` -> `NavigationModule`
- `Modules/Navigation/NavigationPart.cs` -> `NavigationPart`
- `Modules/Navigation/NavigationModuleInstaller.cs` -> `NavigationModuleInstaller`
- `Modules/Navigation/NavMeshAgentExtensions.cs` -> `NavMeshAgentExtensions`

### `ProductContainer`

Target owner package:

- `Modules/ProductContainer/IProductContainerFeature.cs` -> `IProductContainerFeature`
- `Modules/ProductContainer/ProductContainerFeature.cs` -> `ProductContainerFeature`
- `Modules/ProductContainer/ProductContainerState.cs` -> `ProductContainerState`
- `Modules/ProductContainer/ProductContainerModule.cs` -> `ProductContainerModule`
- `Modules/ProductContainer/ProductContainerPart.cs` -> `ProductContainerPart`
- `Modules/ProductContainer/ProductContainerModuleInstaller.cs` -> `ProductContainerModuleInstaller`

### `ShelfConsumer`

Target owner package:

- `Modules/ShelfConsumer/IShelfConsumerFeature.cs` -> `IShelfConsumerFeature`
- `Modules/ShelfConsumer/ShelfConsumerFeature.cs` -> `ShelfConsumerFeature`
- `Modules/ShelfConsumer/ShelfConsumerModule.cs` -> `ShelfConsumerModule`
- `Modules/ShelfConsumer/ShelfConsumerPart.cs` -> `ShelfConsumerPart`
- `Modules/ShelfConsumer/ShelfConsumerModuleInstaller.cs` -> `ShelfConsumerModuleInstaller`

### `ShelfRestocker`

Target owner package:

- `Modules/ShelfRestocker/IShelfRestockerFeature.cs` -> `IShelfRestockerFeature`
- `Modules/ShelfRestocker/ShelfRestockerFeature.cs` -> `ShelfRestockerFeature`
- `Modules/ShelfRestocker/ShelfRestockerModule.cs` -> `ShelfRestockerModule`
- `Modules/ShelfRestocker/ShelfRestockerPart.cs` -> `ShelfRestockerPart`
- `Modules/ShelfRestocker/ShelfRestockerModuleInstaller.cs` -> `ShelfRestockerModuleInstaller`

### `InteractionTarget`

Target owner package:

- `Modules/InteractionTarget/IInteractionTargetFeature.cs` -> `IInteractionTargetFeature`
- `Modules/InteractionTarget/InteractionTargetFeature.cs` -> `InteractionTargetFeature`
- `Modules/InteractionTarget/InteractionTargetModule.cs` -> `InteractionTargetModule`
- `Modules/InteractionTarget/InteractionTargetPart.cs` -> `InteractionTargetPart`
- `Modules/InteractionTarget/InteractionTargetModuleInstaller.cs` -> `InteractionTargetModuleInstaller`
- `Modules/InteractionTarget/TakeFromShelfInteraction.cs` -> `TakeFromShelfInteraction`
- `Modules/InteractionTarget/RestockShelfInteraction.cs` -> `RestockShelfInteraction`
- `Modules/InteractionTarget/TakeFromShelfInteractionResolverPart.cs` -> `TakeFromShelfInteractionResolverPart`
- `Modules/InteractionTarget/RestockShelfInteractionResolverPart.cs` -> `RestockShelfInteractionResolverPart`

### Unresolved Owner Package

Types that do not fit the current target package model from `spec.md`:

- `Debugging/ClickToMoveTester.cs` -> `ClickToMoveInputTester`
- `Debugging/PlayerShelfInteractionTester.cs` -> `PlayerShelfInteractionTester`

These files are real `Game.World` scripts and are scene-referenced, but `spec.md` does not define `Debugging` as a target package and `development-plan.md` currently requires every `Game.World` type to map to `Core`, `Composition`, `Interactions`, or a concrete capability slice.

## Scene / Prefab Blast Radius

Current asset references for `Game.World` scripts are narrower than expected:

- all current scene-facing `Game.World` script references are in `Assets/Content/Scenes/Core 1.unity`
- no current prefab references were found for `Game.World` scripts

Scene-referenced scripts in `Core 1.unity`:

- `Composition/EntityCompositionInstaller.cs`
- `Core/EntityIdentifier.cs`
- `Core/EntityRoot.cs`
- `Debugging/ClickToMoveTester.cs`
- `Debugging/PlayerShelfInteractionTester.cs`
- `Modules/Transform/TransformModuleInstaller.cs`
- `Modules/Transform/TransformPart.cs`
- `Modules/Navigation/NavigationModuleInstaller.cs`
- `Modules/Navigation/NavigationPart.cs`
- `Modules/ProductContainer/ProductContainerModuleInstaller.cs`
- `Modules/ProductContainer/ProductContainerPart.cs`
- `Modules/ShelfConsumer/ShelfConsumerModuleInstaller.cs`
- `Modules/ShelfConsumer/ShelfConsumerPart.cs`
- `Modules/ShelfRestocker/ShelfRestockerModuleInstaller.cs`
- `Modules/InteractionTarget/InteractionTargetModuleInstaller.cs`
- `Modules/InteractionTarget/InteractionTargetPart.cs`
- `Modules/InteractionTarget/TakeFromShelfInteractionResolverPart.cs`
- `Modules/InteractionTarget/RestockShelfInteractionResolverPart.cs`

Notable asset-safety observations:

- `ShelfRestockerPart` currently has no scene or prefab reference
- `ShelfRestockerModuleInstaller` is referenced in `Core 1.unity`
- shared bases `FeaturePart` and `InteractionResolverPart` have no direct asset references, only derived types do

## Blocker

### Conflict with current target package model

The current target architecture in `spec.md` and the current owner-map rule in `development-plan.md` do not account for `Game.World.Debugging`.

That leaves Phase 0 with an incomplete owner map under the current rules.

This is not safe to silently resolve in code or in the next phase, because it changes the migration boundary:

- either `Debugging` must become an explicit out-of-scope support package for this migration;
- or each debugging type must be explicitly assigned to one of the allowed owner packages or slices.

Recommended resolution:

- treat `Game.World.Debugging` as an explicit support package outside the runtime architecture migration scope, and exclude it from the package-ownership definition of done;
- keep its direct dependencies constrained, but do not force it into `Core`, `Composition`, `Interactions`, or a gameplay slice.

## Phase Decision

No code was changed.

Phase 0 findings are recorded, but the phase is not safe to mark complete for migration purposes until the `Debugging` ownership conflict is resolved in the docs.

# Game.World Phase 0 Report

## Phase

Phase 0: Preparation and Safety Net

## Status

Inventory work is complete.
Validation is not complete enough to move to Phase 1 safely.

## What Was Recorded

### Composition infrastructure

- `EntityComposer`
- `EntityContext`
- `FeatureCompositionStep<TPart, TFeature>`
- `IEntityComposer`
- `IEntityCompositionStep`
- `IFeatureFactory<TPart, TFeature>`
- `TransformCompositionStep`
- `TransformFeatureFactory`
- `NavigationCompositionStep`
- `NavigationFeatureFactory`
- `ProductContainerCompositionStep`
- `ProductContainerFeatureFactory`
- `ShelfConsumerCompositionStep`
- `ShelfConsumerFeatureFactory`
- `ShelfRestockerCompositionStep`
- `ShelfRestockerFeatureFactory`
- `InteractionTargetCompositionStep`
- `InteractionTargetFeatureFactory`
- `SceneEntityCompositionService`

### Scene-side feature parts

- `TransformFeaturePart`
- `NavigationFeaturePart`
- `ProductContainerFeaturePart`
- `ShelfConsumerFeaturePart`
- `ShelfRestockerFeaturePart`
- `InteractionTargetFeaturePart`

### Interaction rules

- `InteractionRulePart`
- `TakeFromShelfInteractionRule`
- `RestockShelfInteractionRule`

### Installers

- `EntityCompositionInstaller`
- `TransformFeatureInstaller`
- `NavigationFeatureInstaller`
- `ProductContainerFeatureInstaller`
- `ShelfConsumerFeatureInstaller`
- `ShelfRestockerFeatureInstaller`
- `InteractionTargetFeatureInstaller`

### Debug and test entry points

- `ClickToMoveTester.cs`
- `PlayerShelfInteractionTester.cs`

## Scene and Prefab Usage

Current references for:

- `EntityRoot`
- concrete `*FeaturePart`
- concrete `*InteractionRule`

were found only in:

- `Assets/Content/Scenes/Core 1.unity`

No current prefab references were found for those concrete `Game.World` authoring components.

## `Core 1.unity` Baseline Setup

### `SceneContext`

- `EntityCompositionInstaller`
- `TransformFeatureInstaller`
- `NavigationFeatureInstaller`
- `ProductContainerFeatureInstaller`
- `ShelfConsumerFeatureInstaller`
- `ShelfRestockerFeatureInstaller`
- `InteractionTargetFeatureInstaller`

### `Debugging`

- `ClickToMoveTester`
  - `_targetEntity -> Man`
- `PlayerShelfInteractionTester`
  - `_player -> Man`

### `Man`

- `EntityRoot`
- `TransformFeaturePart`
- `NavigationFeaturePart`
- `ProductContainerFeaturePart`
  - `_capacity = 10`
  - `_initialQuantity = 5`
- `ShelfConsumerFeaturePart`
  - `_transferAmount = 1`

### `Chest`

- `EntityRoot`
- `TransformFeaturePart`
- `ProductContainerFeaturePart`
  - `_capacity = 10`
  - `_initialQuantity = 5`
- `InteractionTargetFeaturePart`
  - `_localInteractionOffset = (0, 0, -0.75)`
- `TakeFromShelfInteractionRule`
  - `_order = 0`
- `RestockShelfInteractionRule`
  - `_order = 0`

## Baseline Manual Smoke Check

1. Open `Assets/Content/Scenes/Core 1.unity`.
2. Enter Play Mode.
3. Confirm both debug entry points target `Man`.
4. Click a walkable point and verify click-to-move still works.
5. Click `Chest` and verify interaction resolution still works.
6. Verify the reachable interaction path is `take from shelf`.

Expected initial state:

- `Man` product container: `5 / 10`
- `Chest` product container: `5 / 10`
- `Man` has `ShelfConsumerFeaturePart`
- `Man` does not have `ShelfRestockerFeaturePart`

## Compatibility Decision for Phase 1

Temporary compatibility adapters are likely needed in Phase 1.

Reason:

- `EntityComposer` still depends on `IEntityCompositionStep`
- all current slices still use `*CompositionStep + *FeatureFactory`
- the plan says Phase 1 should introduce the new composition infrastructure without forcing all slices to migrate immediately

## Validation

### Build check performed

Command:

```text
dotnet build animal-store-frenzy.sln -nologo
```

Result:

- failed before `Game.World` compilation
- current blocker is outside the scope of this phase

Observed error:

- solution contains duplicate project display names such as `Unity.Timeline`

## Validation Gaps and Blockers

### Blocker 1. No reachable `restock` smoke path in assets

The development plan expects manual verification for both:

- `take`
- `restock`

Current asset setup only provides a reachable `take` path.

No current scene or prefab contains:

- an initiator with `ShelfRestockerFeaturePart`
- a playable setup that can exercise `restock` end to end

### Blocker 2. Repository-wide build is blocked outside `Game.World`

The current repository build fails before the `Game.World` refactor can be validated cleanly.

At the moment this is an external repository issue, not a `Game.World` implementation issue.

## Recommendation

Do not start Phase 1 yet.

Before moving on, one of these needs to be decided explicitly:

1. Accept a temporary validation gap for the missing `restock` smoke path.
2. Add a dedicated scene or object setup for `restock` verification first.

And separately:

1. Accept the external repository build blocker as out of scope for this refactor.
2. Or fix the repository build baseline before Phase 1 starts.

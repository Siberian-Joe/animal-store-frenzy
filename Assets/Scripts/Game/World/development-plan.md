# Game.World Migration Plan

## Status

Completed.

This file is kept as the execution record for the v3-to-v4 ownership migration.

Accepted out-of-scope constraints remain unchanged after completion:

- external `NuGetForUnity.dll` build blocker;
- accepted `restock` validation limitation until dedicated coverage is added.

## Goal

Migrate the current v3 `Game.World` codebase to the target package architecture defined in [spec.md](./spec.md).

The plan starts from the current state, not from the removed legacy pipeline.

The migration goal is to:

- turn physical slices into real architectural components;
- remove horizontal ownership centered on `Features`, `Parts`, `Installers`, and `State`;
- keep composition on the current `ICompositionModule` pipeline;
- preserve runtime behavior and Unity serialization safety throughout the migration;
- keep every phase small enough for one commit or one PR.

## Execution principles

1. Start from the current v3 baseline.
2. Keep the active `ICompositionModule` runtime pipeline unchanged.
3. Preserve gameplay behavior. This migration changes ownership and boundaries, not mechanics.
4. Migrate pure C# types before scene-referenced `MonoBehaviour` and `MonoInstaller` types.
5. Migrate package and namespace ownership before any physical file move that is not already required.
6. Do not combine `MonoBehaviour` or `MonoInstaller` renames with package ownership changes. If a rename becomes necessary, it must be isolated into a dedicated follow-up phase.
7. Keep each phase reviewable with explicit before/after ownership changes.
8. After any phase that touches scene-referenced scripts, run Unity scene and prefab validation before starting the next phase.
9. Repository-wide `animal-store-frenzy.sln` build is not a phase gate. The accepted external `NuGetForUnity.dll` blocker remains out of scope.
10. The accepted `restock` validation gap remains out of scope unless a phase explicitly adds or validates a restock scenario.
11. If a file already sits under its owning folder, prefer namespace and dependency cleanup over extra file moves.
12. Do not mix boundary cleanup with unrelated refactors.
13. Promote a type into `Core`, `Composition`, or `Interactions` only if it satisfies the shared-package rule from [spec.md](./spec.md).
14. Preserve `.meta` files and script GUIDs when moving any scene- or prefab-referenced script.
15. A package move is not complete until file placement, declared namespace, and direct type references all reflect the new owner package.
16. When a phase touches scenes or prefabs, reserialize only the assets that reference the moved scripts. Do not bundle unrelated asset cleanup into the same phase.
17. `Game.World.Debugging` is outside the runtime package migration scope. Only dependency hygiene changes are allowed there if moved runtime APIs require it.

## Current baseline

- The project is Unity-based and serialization safety for scenes and prefabs matters.
- The active runtime path is `SceneCompositionBootstrap -> EntityComposer -> ICompositionModule`.
- The legacy `Step + Factory` model is already removed.
- The legacy shim layer is already removed.
- The codebase already uses the physical roots `Core`, `Composition`, `Interactions`, and `Modules/<Slice>`.
- `Game.World.Debugging` exists as a support package for manual runtime testing and is outside the runtime ownership model.
- The main remaining architectural smell is logical ownership: many types still live under horizontal namespaces such as `Game.World.Features`, `Game.World.Parts`, `Game.World.Installers`, and `Game.World.State`.
- Shared infrastructure and slice-local code are still too mixed conceptually, even where folders already look vertical.
- Current manual smoke coverage is centered on `Core 1.unity`, `SceneContext`, `Man`, `Chest`, composition startup, click-to-move, and take interaction.
- The accepted external `NuGetForUnity.dll` blocker remains outside the scope of this migration.
- The accepted `restock` validation gap remains outside the scope of this migration unless a later phase explicitly expands validation for it.

## Migration strategy

The migration is executed in four waves.

### Wave 1. Freeze the baseline and ownership map

Create an explicit runtime type-to-package move map before code churn starts. This keeps the migration reviewable and prevents ad hoc moves by file role instead of package ownership.

### Wave 2. Move pure code first

Migrate shared infrastructure, slice runtime contracts, slice runtime implementations, slice state, and slice `*Module` classes before touching scene-facing scripts.

This wave removes most horizontal ownership without creating Unity serialization risk.

### Wave 3. Move scene-facing scripts in isolated steps

After pure code is stable, migrate shared authoring bases, slice `Part` types, resolver parts, and `*ModuleInstaller` types in dependency order.

This wave is intentionally split so that scene and prefab script-reference risk stays contained to one slice group at a time.

### Wave 4. Lock boundaries, then update docs

Only after code ownership is stable should the migration perform final boundary checks and refresh docs.

## Phases

### Phase 0. Baseline freeze and owner map

**Intent**

Create a stable baseline and an explicit runtime ownership map before code changes.

**Tasks**

- Inventory all runtime `Game.World` types and assign each one to exactly one target owner package: `Core`, `Composition`, `Interactions`, or a concrete slice.
- Classify each type by ownership role: shared infrastructure, slice-local runtime contract, slice-local runtime implementation, scene authoring, or composition orchestration.
- Classify `Game.World.Debugging` as a support package outside the runtime ownership model.
- Inventory all scene- and prefab-referenced `MonoBehaviour` and `MonoInstaller` types in `Game.World`.
- Record which scenes and prefabs reference each risky scene-facing script.
- Record which files are already in the correct physical folder and which files still need a later file move.
- Record the baseline manual validation path for `Core 1.unity`.
- Record the accepted external `NuGetForUnity.dll` blocker and the accepted `restock` validation gap as out-of-scope constraints.

**Outcome**

- A reviewed runtime owner map exists before code migration begins.
- The risky scene-facing types are known before any package move touches them.
- The asset blast radius for each risky scene-facing script is known before any serialization-sensitive phase starts.

**Validation**

- No code changes are required in this phase.
- The owner map is complete enough that every runtime `Game.World` type has one target owner.
- Baseline Unity smoke validation is recorded for `Core 1.unity`.

**Rollback / containment**

- No rollback needed.
- If the inventory is incomplete, do not start Phase 1.

### Phase 1. Shared pure infrastructure ownership

**Intent**

Align the shared pure C# infrastructure with the target package boundaries without touching scene-referenced scripts.

**Tasks**

- Move shared pure runtime primitives into `Core` ownership where needed.
- Move shared composition orchestration types into `Composition` ownership where needed.
- Move shared interaction contracts into `Interactions` ownership where needed.
- Apply the shared-package promotion rule from `spec.md` when deciding whether a type stays slice-local or becomes shared.
- Remove horizontal ownership for pure shared types that still read as `Features`, `State`, or other role buckets.
- Update dependent `using` directives and namespace references.
- Leave `FeaturePart`, `InteractionResolverPart`, `EntityCompositionInstaller`, all slice `Part` types, and all slice installers unchanged in this phase.

**Outcome**

- Shared pure infrastructure reflects `Core`, `Composition`, and `Interactions` ownership directly.
- No Unity scene or prefab script identity risk is introduced in this phase.

**Validation**

- Compile validation for the changed code path.
- Search-based validation that moved shared pure types no longer live under horizontal ownership buckets.
- Composition startup smoke validation only if the changed API surface affects startup wiring.

**Rollback / containment**

- Revert only the shared pure infrastructure move if compile or startup breaks.
- Do not absorb scene-facing script changes into this phase.

### Phase 2. Foundation slice runtime ownership

**Intent**

Move the lowest-level slice runtime code into slice ownership first, starting with the slices that other slices depend on.

**Tasks**

- Migrate `Transform` runtime contracts, runtime implementation, state, and `TransformModule`.
- Migrate `Navigation` runtime contracts, runtime implementation, state, `NavigationModule`, and slice-local pure helpers such as `NavMeshAgentExtensions`.
- Update cross-slice references so that they depend on slice runtime contracts instead of horizontal role buckets.
- Leave `TransformPart`, `NavigationPart`, `TransformModuleInstaller`, and `NavigationModuleInstaller` unchanged in this phase.

**Outcome**

- `Transform` and `Navigation` own their runtime API and composition entry points.
- Downstream slices can depend on package-owned runtime contracts instead of horizontal shared buckets.

**Validation**

- Compile validation for the changed code path.
- Search-based validation that moved runtime types are no longer owned by `Game.World.Features` or `Game.World.State`.
- Composition startup validation if public runtime contracts used at startup changed.

**Rollback / containment**

- Revert only the foundation slice runtime move if compile or startup breaks.
- Do not mix `MonoBehaviour` or installer changes into this phase.

### Phase 3A. ProductContainer runtime ownership

**Intent**

Move the container runtime slice before the shelf-actor runtime slices that depend on it indirectly through interactions.

**Tasks**

- Migrate `ProductContainer` runtime contracts, runtime implementation, state, and `ProductContainerModule`.
- Update references so that downstream slices can consume product-container contracts through slice ownership instead of horizontal role buckets.
- Leave all related `Part` types and `*ModuleInstaller` types unchanged in this phase.

**Outcome**

- `ProductContainer` owns its runtime API and `*Module` type.
- The inventory runtime dependency base is stable before shelf-actor runtime code moves.

**Validation**

- Compile validation for the changed code path.
- Search-based validation that moved runtime types are no longer owned by horizontal role buckets.
- Composition startup validation if changed runtime contracts affect scene composition.

**Rollback / containment**

- Revert only the `ProductContainer` runtime move if compile or startup breaks.
- Do not combine shelf-actor runtime migration into this phase.

### Phase 3B. Shelf actor runtime ownership

**Intent**

Move the shelf-actor runtime slices only after `ProductContainer` runtime ownership is stable.

**Tasks**

- Migrate `ShelfConsumer` runtime contracts, runtime implementation, and `ShelfConsumerModule`.
- Migrate `ShelfRestocker` runtime contracts, runtime implementation, and `ShelfRestockerModule`.
- Update slice-to-slice references to use package-owned runtime contracts only.
- Leave all related `Part` types and `*ModuleInstaller` types unchanged in this phase.

**Outcome**

- Shelf-actor slices own their runtime API and `*Module` types.
- The runtime dependency graph is cleaner before any scene-facing scripts move.

**Validation**

- Compile validation for the changed code path.
- Search-based validation that moved runtime types are no longer owned by horizontal role buckets.
- Composition startup validation if changed runtime contracts affect scene composition.

**Rollback / containment**

- Revert only the shelf-actor runtime move if compile or startup breaks.
- Keep `restock` gameplay validation out of scope for this phase.

### Phase 4. Interaction runtime ownership

**Intent**

Move the interaction runtime slice after its dependent runtime slices are already owner-aligned.

**Tasks**

- Migrate `InteractionTarget` runtime contracts, runtime implementation, concrete interactions, and `InteractionTargetModule`.
- Keep shared interaction contracts in `Interactions`.
- Ensure that the interaction runtime depends only on shared interaction contracts and public runtime contracts from other slices.
- Leave `InteractionTargetPart`, resolver parts, and `InteractionTargetModuleInstaller` unchanged in this phase.

**Outcome**

- The interaction runtime is owner-aligned without touching scene-facing scripts yet.
- The package boundary between `Interactions` and `InteractionTarget` becomes explicit.

**Validation**

- Compile validation for the changed code path.
- Search-based validation that concrete interaction runtime code no longer sits in horizontal shared buckets.
- Composition startup validation.
- Take interaction smoke validation, because this phase changes interaction runtime behavior ownership even though it does not change scene scripts.

**Rollback / containment**

- Revert the interaction runtime move if compile or take interaction breaks.
- Do not continue to scene-facing interaction scripts until this phase is stable.

### Phase 5. Boundary-only cleanup and dependency audit

**Intent**

Run a dedicated boundary pass that only removes accidental horizontal dependencies and verifies the target ownership graph.

**Tasks**

- Remove any remaining accidental `using` directives that point runtime code back to horizontal ownership buckets.
- Remove accidental cross-slice dependencies that point to another slice's `Part`, `Module`, or `ModuleInstaller`.
- Verify that cross-slice dependencies stay within the allowed dependency edges from `spec.md` instead of growing into new convenience links.
- Verify that only `*Module` classes read `EntityCompositionContext`.
- Verify that runtime feature and state classes do not depend on parts, installers, or composition bootstrap types.
- Verify that `Part` classes remain scene-side adapters and do not absorb long-lived runtime state.
- If `Debugging` needs namespace or `using` fixes because runtime APIs moved, keep those changes limited to dependency hygiene and do not treat `Debugging` as a runtime owner package.
- Leave all scene-facing script identities unchanged in this phase.

**Outcome**

- The pure-code dependency graph matches the package rules before the migration touches more Unity-serialized scripts.
- The project has one reviewable phase focused only on boundaries.

**Validation**

- Compile validation for the changed code path.
- Search-based validation against forbidden dependency directions.
- Reviewer pass over the package ownership checklist from `spec.md`.

**Rollback / containment**

- Revert only the boundary cleanup if it destabilizes compile or startup.
- Do not fold scene-facing script migration into this phase.

### Phase 6. Shared scene-authoring infrastructure ownership

**Intent**

Move the shared scene-facing base types after pure code boundaries are stable.

**Tasks**

- Migrate `IFeaturePart` and `FeaturePart` into `Composition` ownership.
- Migrate `InteractionResolverPart` into `Interactions` ownership.
- Update dependent slice parts and resolver parts to reference the new owners.
- Preserve script GUIDs and avoid renaming or relocating derived slice `Part` and `*ModuleInstaller` types in this phase.
- Keep class names and file names stable in this phase.

**Outcome**

- Shared scene-facing base types reflect their actual owner packages.
- Slice parts can move later without dragging shared horizontal ownership along.

**Validation**

- Compile validation for the changed code path.
- Unity scene and prefab validation for touched script types.
- Composition startup validation.

**Rollback / containment**

- If any scene or prefab loses a script binding, revert this phase before proceeding.
- Do not combine this phase with slice-specific scene script moves or derived script renames.

### Phase 7. Foundation scene slices

**Intent**

Move the lowest-level scene-facing slice scripts before moving interaction-facing scene scripts.

**Tasks**

- Migrate `TransformPart`, `TransformModuleInstaller`, and only the transform scene-authoring support code already classified in the Phase 0 owner map.
- Migrate `NavigationPart` and `NavigationModuleInstaller`.
- Keep class names and file names stable in this phase.
- Reserialize only the scenes and prefabs that reference the moved scripts.

**Outcome**

- The foundation scene-facing slices are owner-aligned.
- Movement and composition remain testable before inventory and interaction scene scripts move.

**Validation**

- Compile validation for the changed code path.
- Unity scene and prefab validation.
- Composition startup validation.
- Click-to-move validation.

**Rollback / containment**

- If missing scripts or movement regressions appear, revert only this slice group.
- Do not mix inventory or interaction scene scripts into this phase.

### Phase 8A. ProductContainer scene slice

**Intent**

Move the container scene-facing slice before the shelf-actor scene-facing slices that depend on the same interaction smoke path.

**Tasks**

- Migrate `ProductContainerPart` and `ProductContainerModuleInstaller`.
- Preserve script GUIDs and keep class names and file names stable in this phase.

**Outcome**

- `ProductContainer` is owner-aligned end-to-end before shelf-actor scene scripts move.

**Validation**

- Compile validation for the changed code path.
- Unity scene and prefab validation.
- Composition startup validation.
- Take interaction validation.

**Rollback / containment**

- If missing scripts or take interaction regressions appear, revert only the `ProductContainer` scene slice move.
- Do not combine shelf-actor scene script migration into this phase.

### Phase 8B. Shelf actor scene slices

**Intent**

Move the shelf-actor scene-facing slices only after `ProductContainer` scene-facing ownership is stable.

**Tasks**

- Migrate `ShelfConsumerPart` and `ShelfConsumerModuleInstaller`.
- Migrate `ShelfRestockerPart` and `ShelfRestockerModuleInstaller`.
- Preserve script GUIDs and keep class names and file names stable in this phase.

**Outcome**

- Shelf-actor scene-facing slices are owner-aligned.
- The remaining risky scene-facing interaction work is isolated to the next phase.

**Validation**

- Compile validation for the changed code path.
- Unity scene and prefab validation.
- Composition startup validation.
- Take interaction validation.
- `restock` remains an accepted validation limitation in this phase unless an explicit restock scenario is added.

**Rollback / containment**

- If missing scripts or take interaction regressions appear, revert only the shelf-actor scene slice move.
- Do not expand this phase into interaction-target scene scripts.

### Phase 9. Interaction-target scene slice

**Intent**

Move the remaining interaction-target scene-facing scripts after all dependent slices are stable.

**Tasks**

- Migrate `InteractionTargetPart`.
- Migrate concrete resolver parts owned by the `InteractionTarget` slice.
- Migrate `InteractionTargetModuleInstaller`.
- Preserve script GUIDs and keep class names and file names stable in this phase.

**Outcome**

- The `InteractionTarget` slice is owner-aligned end-to-end.
- Shared interaction contracts remain in `Interactions`, while concrete target-side authoring stays slice-local.

**Validation**

- Compile validation for the changed code path.
- Unity scene and prefab validation.
- Composition startup validation.
- Take interaction validation.
- `restock` remains an accepted validation limitation unless this phase explicitly adds a restock validation scenario.

**Rollback / containment**

- If missing scripts or interaction regressions appear, revert only this slice.
- Do not continue to root bootstrap changes until this phase is stable.

### Phase 10. Root bootstrap alignment and residual package moves

**Intent**

Finish the code migration by aligning the root composition bootstrap path and only the residual package moves already identified in the Phase 0 owner map.

**Tasks**

- Align `EntityCompositionInstaller` with its shared owner package.
- Move only the residual files explicitly recorded in the Phase 0 owner map and still left outside their owner folder after earlier phases.
- Remove only the remaining direct references to horizontal ownership buckets that were already identified by the earlier boundary phases.
- Keep scene-referenced class names stable in this phase.

**Outcome**

- The root composition bootstrap path matches the final package architecture.
- No meaningful code ownership remains in horizontal role buckets.

**Validation**

- Compile validation for the changed code path.
- Unity scene and prefab validation.
- Composition startup validation.
- Click-to-move validation.
- Take interaction validation.

**Rollback / containment**

- If scene startup breaks, revert this phase immediately.
- If the residual list is larger than one focused PR, split the residual work before implementation and do not treat it as one phase.
- Do not mix documentation cleanup into this phase.

### Phase 11. Final docs-only cleanup

**Intent**

Update docs only after the code shape is stable.

**Tasks**

- Update `spec.md` if small wording corrections are needed to reflect the final code shape.
- Update this plan to reflect completed migration status or archive it.
- Update any architecture notes that still describe the old horizontal ownership shape.
- Do not change runtime code in this phase.

**Outcome**

- Docs match the final migrated code.
- The migration closes with a docs-only phase instead of mixing docs into code stabilization work.

**Validation**

- Review that docs match the final code ownership and dependency shape.
- Confirm that no code files changed in this phase.

**Rollback / containment**

- Docs-only rollback if wording is wrong.

## Verification per phase

- Phase 0 is inventory and baseline only. No compile gate, but the inventory and baseline smoke path must be complete.
- Phases 1 through 5 are pure-code and boundary phases. They require compile validation and ownership/dependency review. Unity manual validation is required only when startup behavior may be affected.
- Phases 6 through 10 are scene-facing phases. They require compile validation, Unity scene and prefab validation, composition startup validation, and the gameplay smoke checks relevant to the touched slices.
- Phase 11 is docs only. It does not change code and should not be used to hide unfinished migration work.

## Validation matrix

| Check | When it is mandatory | Pass condition | Notes |
| --- | --- | --- | --- |
| Build / compile validation | Every code-changing phase | Changed code compiles through the project path available for the touched code | Repository-wide `animal-store-frenzy.sln` build is not a gate. The accepted external `NuGetForUnity.dll` blocker remains out of scope and must be recorded, not fixed, if encountered. |
| Unity scene / prefab validation | Every phase that touches `MonoBehaviour`, `MonoInstaller`, file placement of scene scripts, or script namespaces | `Core 1.unity` and any touched prefabs open without missing scripts | This is the primary serialization safety check. Preserve `.meta` files and script GUIDs on file moves. |
| Composition startup validation | Every phase that touches `Core`, `Composition`, bootstrap, parts, modules, or installers | Scene init still composes `SceneContext`, `Man`, and `Chest` correctly | Run after any change that can affect startup wiring. |
| Click-to-move validation | Every phase that touches `Transform`, `Navigation`, or root bootstrap wiring | Player click-to-move still works in `Core 1.unity` | This is the movement smoke path. |
| Take interaction validation | Every phase that touches `ProductContainer`, `ShelfConsumer`, `InteractionTarget`, their installers, or interaction runtime | Take-from-shelf still works in `Core 1.unity` | This is the main interaction smoke path currently available. |
| Known accepted restock limitation | Any phase that touches `ShelfRestocker` or interaction ownership but does not introduce a dedicated restock scenario | The phase report explicitly records that restock validation is still an accepted limitation | This remains outside scope unless a phase explicitly adds restock validation coverage. |

## Risks

- Unity script references can break when `MonoBehaviour` or `MonoInstaller` package identity changes are combined with too much unrelated churn.
- Runtime ownership changes can silently widen dependencies if `using` cleanup is incomplete.
- The root composition bootstrap path is a high-impact area; a mistake there blocks all scene startup.
- Pure code can look vertically organized while still violating ownership rules through horizontal namespaces and direct type references.
- Interaction code is easy to over-share; concrete interaction logic must not drift back into the shared `Interactions` package.
- The accepted external `NuGetForUnity.dll` blocker can hide unrelated compile noise if phase reports do not separate it clearly from migration issues.

## Stop-and-fix rule

Stop the migration immediately if any phase causes one of the following:

- compile failure in the changed package set;
- missing scripts in touched scenes or prefabs;
- broken composition startup;
- broken click-to-move after a movement-related phase;
- broken take interaction after an interaction-related phase;
- a new forbidden dependency direction against `spec.md`.

When that happens:

- fix the issue inside the same phase, or rollback the whole phase;
- do not continue to the next phase on a partially stable state;
- do not hide the regression inside a later cleanup phase.

The accepted external `NuGetForUnity.dll` blocker and the accepted `restock` validation gap do not count as migration regressions unless the current phase explicitly expands into that scope.

## Definition of done

The migration is done when all of the following are true:

- the final code shape is readable in terms of `Core`, `Composition`, `Interactions`, and concrete capability slices;
- every runtime `Game.World` type has one clear owner package: `Core`, `Composition`, `Interactions`, or one concrete slice;
- `Game.World.Debugging` is explicitly treated as a support package outside the runtime ownership model;
- horizontal ownership buckets such as `Game.World.Features`, `Game.World.Parts`, `Game.World.Installers`, and `Game.World.State` are no longer used as architecture centers;
- slice runtime contracts, runtime implementations, scene authoring, `*Module`, and `*ModuleInstaller` types are owned by their slices;
- shared infrastructure is limited to `Core`, `Composition`, and `Interactions`;
- `Composition` remains generic shared orchestration and does not depend on concrete slices;
- interactions remain part of the same composition model and do not introduce a second pipeline;
- `Part` remains a scene-side adapter and may implement authoring-local runtime-facing contracts without introducing a second adapter layer;
- cross-slice dependencies go only through public runtime contracts or shared interaction contracts, and stay within the dependency edges defined by `spec.md`;
- runtime packages and capability slices do not depend on `Game.World.Debugging`;
- `EntityCompositionContext` is only read by composition infrastructure and slice `*Module` classes;
- package ownership and forbidden dependency directions are reviewable from declared namespaces, file placement, and direct type references;
- scenes and touched prefabs open without missing scripts;
- composition startup still works;
- click-to-move still works;
- take interaction still works;
- the accepted restock limitation is still recorded if no explicit restock validation scenario was added;
- the final code ownership and dependency shape match the slice template and dependency rules from `spec.md` closely enough that a new capability can be added without reopening package-boundary decisions;
- docs were updated only after the code shape became stable.

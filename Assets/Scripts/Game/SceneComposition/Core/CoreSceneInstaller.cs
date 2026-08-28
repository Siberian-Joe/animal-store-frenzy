using System;
using System.Collections.Generic;
using System.IO;
using Game.Presentation.Screens.Core;
using Game.Presentation.Screens.Core.Startup;
using Game.Presentation.Startup;
using Game.SceneComposition.Shared;
using Game.World.Commands;
using Game.World.Pickups;
using Game.World.Processing;
using Game.World.Quests.Authoring;
using Game.World.Quests.Progress;
using Game.World.Quests.Targets;
using Game.World.Shop.Shelves;
using Game.World.Store;
using Game.World.Supplies;
using Game.World.GameTime;
using Game.World.Upgrades;
using Modules.Presentation.Runtime.Contracts.Preparation;
using UnityEngine;

namespace Game.SceneComposition.Core
{
    public sealed class CoreSceneInstaller : SceneStartupInstaller
    {
        [Header("Quests")] [SerializeField] private QuestDefinition[] _activeQuestDefinitions;
        [SerializeField] private string _questProgressFileName = "quest-progress.json";

        protected override void InstallSceneBindings()
        {
            BindPresentation();
            BindGameplay();
            BindStartupTask<PrepareConfiguredPanelsStartupTask>();
            BindStartupTask<InitializeQuestProgressStartupTask>();
            BindStartupTask<OpenCoreScreenStartupTask>();
        }

        private void BindPresentation()
        {
            Container
                .Bind<IPanelPreparationPlan>()
                .To<CorePresentationPlan>()
                .AsSingle();
        }

        private void BindGameplay()
        {
            var quests = new List<QuestDefinition>(_activeQuestDefinitions ?? Array.Empty<QuestDefinition>());
            ValidateQuestDefinitions(quests);

            if (Container.HasBinding<GameCommandDispatcher>() == false)
            {
                Container
                    .Bind<GameCommandDispatcher>()
                    .AsSingle();
            }

            StoreRuntimeInstaller.InstallStoreRuntimeBindings(Container);

            Container
                .BindInterfacesTo<GameTimeAdvancer>()
                .AsSingle();

            Container
                .Bind<List<QuestDefinition>>()
                .FromInstance(quests)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<QuestTargetRegistry>()
                .AsSingle();

            Container
                .Bind<IQuestProgressStateStore>()
                .To<JsonQuestProgressStateStore>()
                .AsSingle()
                .WithArguments(ResolveQuestProgressPath());

            Container
                .BindInterfacesAndSelfTo<QuestProgressService>()
                .AsSingle();

            Container
                .Bind<IGameCommandPostProcessor>()
                .To<QuestProgressCommandPostProcessor>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<PickupItemCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<ProcessItemsCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<CollectSupplyCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<AdvanceUpgradeStageCommandHandler>()
                .AsSingle();

            Container
                .Bind<IGameCommandHandler>()
                .To<StockShelfCommandHandler>()
                .AsSingle();
        }


        private static void ValidateQuestDefinitions(IReadOnlyList<QuestDefinition> quests)
        {
            if (quests == null || quests.Count == 0)
                throw new InvalidOperationException(
                    $"{nameof(CoreSceneInstaller)} requires at least one active quest definition.");

            for (var index = 0; index < quests.Count; index++)
            {
                var quest = quests[index];
                if (quest == false)
                    throw new InvalidOperationException(
                        $"{nameof(CoreSceneInstaller)} has null active quest definition at index {index}.");

                quest.Validate();
            }
        }

        private string ResolveQuestProgressPath()
        {
            if (string.IsNullOrWhiteSpace(_questProgressFileName))
                throw new InvalidOperationException(
                    $"{nameof(CoreSceneInstaller)} requires non-empty quest progress file name.");

            return Path.Combine(Application.persistentDataPath, _questProgressFileName.Trim());
        }
    }
}
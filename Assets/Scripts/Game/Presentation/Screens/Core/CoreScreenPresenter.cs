using System;
using System.Text;
using Game.SceneNavigation.Routes;
using Game.World.Inventory;
using Game.World.PlayerInteraction;
using Game.World.Quests.Progress;
using Game.World.Store;
using Modules.Presentation.Runtime.Panels;
using Modules.SceneNavigation.Runtime.Contracts;
using R3;
using UnityEngine;

namespace Game.Presentation.Screens.Core
{
    public sealed class CoreScreenPresenter : PanelPresenter<CoreScreen>
    {
        private readonly ISceneNavigator _sceneNavigator;
        private readonly IQuestProgressReader _questProgressReader;
        private readonly IPlayerInventoryReader _playerInventoryReader;
        private readonly IPlayerFeedbackReader _feedbackReader;
        private readonly IPlayerFeedback _feedback;
        private readonly IStoreRuntimeResolver _storeResolver;
        private readonly CompositeDisposable _disposables = new();

        public CoreScreenPresenter(
            ISceneNavigator sceneNavigator,
            IQuestProgressReader questProgressReader,
            IPlayerInventoryReader playerInventoryReader,
            IPlayerFeedbackReader feedbackReader,
            IPlayerFeedback feedback,
            IStoreRuntimeResolver storeResolver)
        {
            _sceneNavigator = sceneNavigator ?? throw new ArgumentNullException(nameof(sceneNavigator));
            _questProgressReader = questProgressReader ?? throw new ArgumentNullException(nameof(questProgressReader));
            _playerInventoryReader =
                playerInventoryReader ?? throw new ArgumentNullException(nameof(playerInventoryReader));
            _feedbackReader = feedbackReader ?? throw new ArgumentNullException(nameof(feedbackReader));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
            _storeResolver = storeResolver ?? throw new ArgumentNullException(nameof(storeResolver));
        }

        protected override void OnPanelAttached(CoreScreen panel)
        {
            panel.MainMenuRequested
                .Subscribe(_ => ReturnToMainMenu())
                .AddTo(_disposables);

            _questProgressReader.ProgressChanged
                .Subscribe(snapshot => RenderQuest(snapshot))
                .AddTo(_disposables);

            _questProgressReader.QuestCompleted
                .Subscribe(_ => _feedback.ShowMessage("Objective completed"))
                .AddTo(_disposables);

            _playerInventoryReader.Changed
                .Subscribe(_ => RenderInventory())
                .AddTo(_disposables);

            _feedbackReader.MessageShown
                .Subscribe(panel.SetFeedbackText)
                .AddTo(_disposables);

            if (_storeResolver.TryGetAnyStatus(out var storeStatus))
            {
                storeStatus.Changed
                    .Subscribe(_ => RenderStoreStatus())
                    .AddTo(_disposables);
            }

            RenderQuest(_questProgressReader.Current);
            RenderInventory();
            RenderStoreStatus();
            panel.SetFeedbackText(_feedbackReader.CurrentMessage);
        }

        protected override void OnOpened()
        {
            if (Panel == false)
                return;

            RenderQuest(_questProgressReader.Current);
            RenderInventory();
            RenderStoreStatus();
            Panel.SetFeedbackText(_feedbackReader.CurrentMessage);
        }

        protected override void OnReleased() => _disposables.Dispose();

        private void RenderQuest(QuestProgressSnapshot snapshot)
        {
            if (Panel == false)
                return;

            if (TryGetActiveQuest(snapshot, out var quest) == false)
            {
                Panel.SetObjectiveText(string.Empty);
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine(quest.Title);

            if (string.IsNullOrWhiteSpace(quest.Summary) == false)
                builder.AppendLine(quest.Summary);

            foreach (var objective in quest.Objectives)
            {
                builder
                    .Append(objective.IsCompleted ? "[x] " : "[ ] ")
                    .AppendLine(objective.Text);
            }

            Panel.SetObjectiveText(builder.ToString().TrimEnd());
        }

        private static bool TryGetActiveQuest(QuestProgressSnapshot snapshot, out QuestSnapshot quest)
        {
            if (snapshot.Quests != null)
            {
                foreach (var candidate in snapshot.Quests)
                {
                    if (candidate.IsCompleted)
                        continue;

                    quest = candidate;
                    return true;
                }
            }

            quest = default;
            return false;
        }

        private void RenderStoreStatus()
        {
            if (Panel == false)
                return;

            if (_storeResolver.TryGetAnyStatus(out var status) == false)
            {
                Panel.SetStoreStatusText(string.Empty);
                return;
            }

            Panel.SetStoreStatusText(status.IsOpen ? "Store: OPEN" : "Store: CLOSED");
        }

        private void RenderInventory()
        {
            if (Panel == false)
                return;

            var inventory = _playerInventoryReader.Inventory;
            if (inventory.Items.Count == 0)
            {
                Panel.SetInventoryText("Inventory: (empty)");
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("Inventory:");

            foreach (var item in inventory.Items)
                builder.AppendLine($"{item.ItemId}: {item.Amount}");

            Panel.SetInventoryText(builder.ToString().TrimEnd());
        }

        private void ReturnToMainMenu()
        {
            if (_sceneNavigator.TryGoTo<MainMenuRoute>() == false)
                Debug.LogWarning($"{nameof(ISceneNavigator)} rejected transition to {nameof(MainMenuRoute)}. " +
                                 "Transition is probably already running.");
        }
    }
}